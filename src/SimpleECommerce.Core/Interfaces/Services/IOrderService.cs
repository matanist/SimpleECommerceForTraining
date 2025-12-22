using SimpleECommerce.Core.DTOs.Common;
using SimpleECommerce.Core.DTOs.Order;
using SimpleECommerce.Core.Entities;

namespace SimpleECommerce.Core.Interfaces.Services;

public interface IOrderService
{
    Task<ApiResponse<PaginatedResult<OrderDto>>> GetAllAsync(int pageNumber = 1, int pageSize = 10, int? userId = null, OrderStatus? status = null);
    Task<ApiResponse<OrderDto>> GetByIdAsync(int id);
    Task<ApiResponse<OrderDto>> GetByOrderNumberAsync(string orderNumber);
    Task<ApiResponse<IEnumerable<OrderDto>>> GetUserOrdersAsync(int userId);
    Task<ApiResponse<OrderDto>> CreateAsync(int userId, CreateOrderDto dto);
    Task<ApiResponse<OrderDto>> UpdateStatusAsync(int id, UpdateOrderStatusDto dto);
    Task<ApiResponse> CancelOrderAsync(int id, int userId);
}
