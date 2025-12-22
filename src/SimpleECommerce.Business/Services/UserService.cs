using AutoMapper;
using SimpleECommerce.Core.DTOs.Common;
using SimpleECommerce.Core.DTOs.User;
using SimpleECommerce.Core.Interfaces.Repositories;
using SimpleECommerce.Core.Interfaces.Services;

namespace SimpleECommerce.Business.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<UserDto>>> GetAllAsync()
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var userWithRole = await _unitOfWork.Users.GetByIdWithRoleAsync(user.Id);
            if (userWithRole != null)
            {
                userDtos.Add(_mapper.Map<UserDto>(userWithRole));
            }
        }

        return ApiResponse<IEnumerable<UserDto>>.SuccessResult(userDtos);
    }

    public async Task<ApiResponse<UserDto>> GetByIdAsync(int id)
    {
        var user = await _unitOfWork.Users.GetByIdWithRoleAsync(id);

        if (user == null)
        {
            return ApiResponse<UserDto>.FailResult("User not found");
        }

        return ApiResponse<UserDto>.SuccessResult(_mapper.Map<UserDto>(user));
    }

    public async Task<ApiResponse<UserDto>> UpdateAsync(int id, UpdateUserDto dto)
    {
        var user = await _unitOfWork.Users.GetByIdWithRoleAsync(id);

        if (user == null)
        {
            return ApiResponse<UserDto>.FailResult("User not found");
        }

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.PhoneNumber = dto.PhoneNumber;
        user.Address = dto.Address;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<UserDto>.SuccessResult(_mapper.Map<UserDto>(user), "User updated successfully");
    }

    public async Task<ApiResponse> DeleteAsync(int id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);

        if (user == null)
        {
            return ApiResponse.FailResult("User not found");
        }

        await _unitOfWork.Users.SoftDeleteAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse.SuccessResult("User deleted successfully");
    }
}
