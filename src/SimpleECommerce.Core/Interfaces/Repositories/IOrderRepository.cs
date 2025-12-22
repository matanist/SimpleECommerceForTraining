using SimpleECommerce.Core.DTOs.Common;
using SimpleECommerce.Core.Entities;

namespace SimpleECommerce.Core.Interfaces.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByIdWithDetailsAsync(int id);
    Task<Order?> GetByOrderNumberAsync(string orderNumber);
    Task<PaginatedResult<Order>> GetPaginatedAsync(int pageNumber, int pageSize, int? userId = null, OrderStatus? status = null);
    Task<IEnumerable<Order>> GetByUserIdAsync(int userId);
}
