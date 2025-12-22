using AutoMapper;
using SimpleECommerce.Core.DTOs.Common;
using SimpleECommerce.Core.DTOs.Order;
using SimpleECommerce.Core.Entities;
using SimpleECommerce.Core.Interfaces.Repositories;
using SimpleECommerce.Core.Interfaces.Services;

namespace SimpleECommerce.Business.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<PaginatedResult<OrderDto>>> GetAllAsync(int pageNumber = 1, int pageSize = 10, int? userId = null, OrderStatus? status = null)
    {
        var paginatedOrders = await _unitOfWork.Orders.GetPaginatedAsync(pageNumber, pageSize, userId, status);

        var result = new PaginatedResult<OrderDto>
        {
            Items = _mapper.Map<List<OrderDto>>(paginatedOrders.Items),
            TotalCount = paginatedOrders.TotalCount,
            PageNumber = paginatedOrders.PageNumber,
            PageSize = paginatedOrders.PageSize
        };

        return ApiResponse<PaginatedResult<OrderDto>>.SuccessResult(result);
    }

    public async Task<ApiResponse<OrderDto>> GetByIdAsync(int id)
    {
        var order = await _unitOfWork.Orders.GetByIdWithDetailsAsync(id);

        if (order == null)
        {
            return ApiResponse<OrderDto>.FailResult("Order not found");
        }

        return ApiResponse<OrderDto>.SuccessResult(_mapper.Map<OrderDto>(order));
    }

    public async Task<ApiResponse<OrderDto>> GetByOrderNumberAsync(string orderNumber)
    {
        var order = await _unitOfWork.Orders.GetByOrderNumberAsync(orderNumber);

        if (order == null)
        {
            return ApiResponse<OrderDto>.FailResult("Order not found");
        }

        return ApiResponse<OrderDto>.SuccessResult(_mapper.Map<OrderDto>(order));
    }

    public async Task<ApiResponse<IEnumerable<OrderDto>>> GetUserOrdersAsync(int userId)
    {
        var orders = await _unitOfWork.Orders.GetByUserIdAsync(userId);
        return ApiResponse<IEnumerable<OrderDto>>.SuccessResult(_mapper.Map<IEnumerable<OrderDto>>(orders));
    }

    public async Task<ApiResponse<OrderDto>> CreateAsync(int userId, CreateOrderDto dto)
    {
        if (!dto.Items.Any())
        {
            return ApiResponse<OrderDto>.FailResult("Order must contain at least one item");
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var order = new Order
            {
                OrderNumber = GenerateOrderNumber(),
                UserId = userId,
                ShippingAddress = dto.ShippingAddress,
                Notes = dto.Notes,
                Status = OrderStatus.Pending
            };

            decimal totalAmount = 0;

            foreach (var item in dto.Items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);

                if (product == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<OrderDto>.FailResult($"Product with ID {item.ProductId} not found");
                }

                if (product.StockQuantity < item.Quantity)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<OrderDto>.FailResult($"Insufficient stock for product: {product.Name}");
                }

                product.StockQuantity -= item.Quantity;
                await _unitOfWork.Products.UpdateAsync(product);

                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                };

                order.OrderItems.Add(orderItem);
                totalAmount += orderItem.Quantity * orderItem.UnitPrice;
            }

            order.TotalAmount = totalAmount;

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            var createdOrder = await _unitOfWork.Orders.GetByIdWithDetailsAsync(order.Id);

            return ApiResponse<OrderDto>.SuccessResult(_mapper.Map<OrderDto>(createdOrder), "Order created successfully");
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<ApiResponse<OrderDto>> UpdateStatusAsync(int id, UpdateOrderStatusDto dto)
    {
        var order = await _unitOfWork.Orders.GetByIdWithDetailsAsync(id);

        if (order == null)
        {
            return ApiResponse<OrderDto>.FailResult("Order not found");
        }

        order.Status = dto.Status;

        await _unitOfWork.Orders.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return ApiResponse<OrderDto>.SuccessResult(_mapper.Map<OrderDto>(order), "Order status updated successfully");
    }

    public async Task<ApiResponse> CancelOrderAsync(int id, int userId)
    {
        var order = await _unitOfWork.Orders.GetByIdWithDetailsAsync(id);

        if (order == null)
        {
            return ApiResponse.FailResult("Order not found");
        }

        if (order.UserId != userId)
        {
            return ApiResponse.FailResult("You can only cancel your own orders");
        }

        if (order.Status != OrderStatus.Pending)
        {
            return ApiResponse.FailResult("Only pending orders can be cancelled");
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            foreach (var item in order.OrderItems)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.StockQuantity += item.Quantity;
                    await _unitOfWork.Products.UpdateAsync(product);
                }
            }

            order.Status = OrderStatus.Cancelled;

            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return ApiResponse.SuccessResult("Order cancelled successfully");
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    private static string GenerateOrderNumber()
    {
        return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }
}
