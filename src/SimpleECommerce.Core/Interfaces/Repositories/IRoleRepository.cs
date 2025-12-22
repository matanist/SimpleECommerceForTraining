using SimpleECommerce.Core.Entities;

namespace SimpleECommerce.Core.Interfaces.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetByNameAsync(string name);
}
