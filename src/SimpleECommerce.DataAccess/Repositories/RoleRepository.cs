using Microsoft.EntityFrameworkCore;
using SimpleECommerce.Core.Entities;
using SimpleECommerce.Core.Interfaces.Repositories;
using SimpleECommerce.DataAccess.Context;

namespace SimpleECommerce.DataAccess.Repositories;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(r => r.Name == name);
    }
}
