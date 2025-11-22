using MediatR;

namespace DotNetBackEnd.Application.Orders.Commands;

public record MarkOrderPaidCommand(Guid OrderId) : IRequest<Unit>;
