using MediatR;

namespace DotNetBackEnd.Application.Orders.Commands;

public record MarkOrderCompletedCommand(Guid OrderId) : IRequest<Unit>;
