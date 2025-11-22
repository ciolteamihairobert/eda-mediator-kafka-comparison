using MediatR;

namespace DotNetBackEnd.Application.Orders.Queries;

public record GetAllOrdersQuery() : IRequest<IReadOnlyList<OrderDto>>;
