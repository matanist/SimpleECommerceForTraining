using SimpleECommerce.Core.DTOs.Common;
using SimpleECommerce.Core.DTOs.User;

namespace SimpleECommerce.Core.Interfaces.Services;

public interface IUserService
{
    Task<ApiResponse<IEnumerable<UserDto>>> GetAllAsync();
    Task<ApiResponse<UserDto>> GetByIdAsync(int id);
    Task<ApiResponse<UserDto>> UpdateAsync(int id, UpdateUserDto dto);
    Task<ApiResponse> DeleteAsync(int id);
}
