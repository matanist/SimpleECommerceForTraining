using SimpleECommerce.Core.Entities;

namespace SimpleECommerce.Core.DTOs.Order;

public class UpdateOrderStatusDto
{
    public OrderStatus Status { get; set; }
}
