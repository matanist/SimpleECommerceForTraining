using SimpleECommerce.Core.DTOs.Auth;
using SimpleECommerce.Core.DTOs.Common;

namespace SimpleECommerce.Core.Interfaces.Services;

public interface IAuthService
{
    Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request);
    Task<ApiResponse<LoginResponseDto>> RegisterAsync(RegisterRequestDto request);
    Task<ApiResponse<UserInfoDto>> GetCurrentUserAsync(int userId);
}
