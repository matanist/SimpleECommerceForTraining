using AutoMapper;
using SimpleECommerce.Core.DTOs.Auth;
using SimpleECommerce.Core.DTOs.Common;
using SimpleECommerce.Core.Entities;
using SimpleECommerce.Core.Interfaces.Repositories;
using SimpleECommerce.Core.Interfaces.Services;

namespace SimpleECommerce.Business.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public AuthService(IUnitOfWork unitOfWork, IJwtService jwtService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return ApiResponse<LoginResponseDto>.FailResult("Invalid email or password");
        }

        var token = _jwtService.GenerateToken(user);

        var response = new LoginResponseDto
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            User = _mapper.Map<UserInfoDto>(user)
        };

        return ApiResponse<LoginResponseDto>.SuccessResult(response, "Login successful");
    }

    public async Task<ApiResponse<LoginResponseDto>> RegisterAsync(RegisterRequestDto request)
    {
        if (await _unitOfWork.Users.EmailExistsAsync(request.Email))
        {
            return ApiResponse<LoginResponseDto>.FailResult("Email already exists");
        }

        var customerRole = await _unitOfWork.Roles.GetByNameAsync("Customer");
        if (customerRole == null)
        {
            return ApiResponse<LoginResponseDto>.FailResult("Default role not found");
        }

        var user = _mapper.Map<User>(request);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        user.RoleId = customerRole.Id;

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var userWithRole = await _unitOfWork.Users.GetByIdWithRoleAsync(user.Id);
        var token = _jwtService.GenerateToken(userWithRole!);

        var response = new LoginResponseDto
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            User = _mapper.Map<UserInfoDto>(userWithRole)
        };

        return ApiResponse<LoginResponseDto>.SuccessResult(response, "Registration successful");
    }

    public async Task<ApiResponse<UserInfoDto>> GetCurrentUserAsync(int userId)
    {
        var user = await _unitOfWork.Users.GetByIdWithRoleAsync(userId);

        if (user == null)
        {
            return ApiResponse<UserInfoDto>.FailResult("User not found");
        }

        return ApiResponse<UserInfoDto>.SuccessResult(_mapper.Map<UserInfoDto>(user));
    }
}
