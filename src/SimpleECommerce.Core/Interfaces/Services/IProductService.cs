using SimpleECommerce.Core.DTOs.Common;
using SimpleECommerce.Core.DTOs.Product;

namespace SimpleECommerce.Core.Interfaces.Services;

public interface IProductService
{
    Task<ApiResponse<PaginatedResult<ProductDto>>> GetAllAsync(int pageNumber = 1, int pageSize = 10, int? categoryId = null, string? searchTerm = null);
    Task<ApiResponse<ProductDto>> GetByIdAsync(int id);
    Task<ApiResponse<ProductDto>> CreateAsync(CreateProductDto dto);
    Task<ApiResponse<ProductDto>> UpdateAsync(int id, UpdateProductDto dto);
    Task<ApiResponse> DeleteAsync(int id);
}
