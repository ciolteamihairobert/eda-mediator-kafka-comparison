using MediatR;
using DotNetBackEnd.Application.Common;
using DotNetBackEnd.Application.IntegrationEvents;
using DotNetBackEnd.Application.Orders.Commands;
using DotNetBackEnd.Domain.Repositories;

namespace DotNetBackEnd.Application.Orders.Handlers;

public class MarkOrderInventoryReservedCommandHandler : IRequestHandler<MarkOrderInventoryReservedCommand, Unit>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IEventBus _eventBus;
    private const string InventoryReservedTopic = "inventory-reserved";

    public MarkOrderInventoryReservedCommandHandler(IOrderRepository orderRepository, IEventBus eventBus)
    {
        _orderRepository = orderRepository;
        _eventBus = eventBus;
    }

    public async Task<Unit> Handle(MarkOrderInventoryReservedCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken)
                    ?? throw new InvalidOperationException($"Order {request.OrderId} not found");

        order.MarkInventoryReserved();
        await _orderRepository.UpdateAsync(order, cancellationToken);

        var evt = new InventoryReservedIntegrationEvent(
            order.Id,
            DateTime.UtcNow
        );

        await _eventBus.PublishAsync(InventoryReservedTopic, evt, cancellationToken);

        return Unit.Value;
    }
}
