using SimpleECommerce.Core.Entities;

namespace SimpleECommerce.Core.Interfaces.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetByIdWithProductsAsync(int id);
    Task<IEnumerable<Category>> GetAllWithProductCountAsync();
}
