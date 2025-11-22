using MediatR;

namespace DotNetBackEnd.Application.Orders.Queries;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<OrderDto?>;
