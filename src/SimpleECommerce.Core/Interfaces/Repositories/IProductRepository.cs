using SimpleECommerce.Core.DTOs.Common;
using SimpleECommerce.Core.Entities;

namespace SimpleECommerce.Core.Interfaces.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetByIdWithCategoryAsync(int id);
    Task<PaginatedResult<Product>> GetPaginatedAsync(int pageNumber, int pageSize, int? categoryId = null, string? searchTerm = null);
    Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
}
