using MediatR;

namespace DotNetBackEnd.Application.Orders.Commands;

public record CreateOrderCommand(
    string CustomerId,
    decimal TotalAmount,
    string Currency = "EUR"
) : IRequest<Guid>;
