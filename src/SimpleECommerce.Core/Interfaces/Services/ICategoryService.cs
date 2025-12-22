using SimpleECommerce.Core.DTOs.Category;
using SimpleECommerce.Core.DTOs.Common;

namespace SimpleECommerce.Core.Interfaces.Services;

public interface ICategoryService
{
    Task<ApiResponse<IEnumerable<CategoryDto>>> GetAllAsync();
    Task<ApiResponse<CategoryDto>> GetByIdAsync(int id);
    Task<ApiResponse<CategoryDto>> CreateAsync(CreateCategoryDto dto);
    Task<ApiResponse<CategoryDto>> UpdateAsync(int id, UpdateCategoryDto dto);
    Task<ApiResponse> DeleteAsync(int id);
}
