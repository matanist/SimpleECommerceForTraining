using SimpleECommerce.Core.Entities;

namespace SimpleECommerce.Core.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdWithRoleAsync(int id);
    Task<bool> EmailExistsAsync(string email);
}
