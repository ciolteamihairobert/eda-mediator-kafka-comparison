using MediatR;
using DotNetBackEnd.Application.Common;
using DotNetBackEnd.Application.IntegrationEvents;
using DotNetBackEnd.Application.Orders.Commands;
using DotNetBackEnd.Domain.Repositories;

namespace DotNetBackEnd.Application.Orders.Handlers;

public class MarkOrderCompletedCommandHandler: IRequestHandler<MarkOrderCompletedCommand, Unit>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IEventBus _eventBus;
    private const string OrderCompletedTopic = "order-completed";

    public MarkOrderCompletedCommandHandler(IOrderRepository orderRepository, IEventBus eventBus)
    {
        _orderRepository = orderRepository;
        _eventBus = eventBus;
    }

    public async Task<Unit> Handle(MarkOrderCompletedCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken)
                    ?? throw new InvalidOperationException($"Order {request.OrderId} not found");

        order.MarkCompleted();
        await _orderRepository.UpdateAsync(order, cancellationToken);

        var evt = new OrderCompletedIntegrationEvent(
            order.Id,
            DateTime.UtcNow
        );

        await _eventBus.PublishAsync(OrderCompletedTopic, evt, cancellationToken);

        return Unit.Value;
    }
}
