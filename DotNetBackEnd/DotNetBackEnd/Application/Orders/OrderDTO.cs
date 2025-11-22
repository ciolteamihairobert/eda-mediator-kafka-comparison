using DotNetBackEnd.Domain.Entities;

namespace DotNetBackEnd.Application.Orders;

public record OrderDto(
    Guid Id,
    string CustomerId,
    decimal TotalAmount,
    string Currency,
    OrderStatus Status,
    DateTime CreatedAtUtc,
    DateTime? CompletedAtUtc
)
{
    public static OrderDto FromOrder(Order order) =>
        new(order.Id, order.CustomerId, order.TotalAmount, order.Currency,
            order.Status, order.CreatedAtUtc, order.CompletedAtUtc);
}
