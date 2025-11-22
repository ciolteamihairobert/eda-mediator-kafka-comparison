using MediatR;

namespace DotNetBackEnd.Application.Orders.Commands;

public record MarkOrderInventoryReservedCommand(Guid OrderId) : IRequest<Unit>;
