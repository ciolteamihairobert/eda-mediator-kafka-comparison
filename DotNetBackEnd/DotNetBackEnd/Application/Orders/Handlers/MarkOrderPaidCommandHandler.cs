using MediatR;
using DotNetBackEnd.Application.Common;
using DotNetBackEnd.Application.IntegrationEvents;
using DotNetBackEnd.Application.Orders.Commands;
using DotNetBackEnd.Domain.Repositories;

namespace DotNetBackEnd.Application.Orders.Handlers;

public class MarkOrderPaidCommandHandler : IRequestHandler<MarkOrderPaidCommand, Unit>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IEventBus _eventBus;
    private const string PaymentAuthorizedTopic = "payment-authorized";

    public MarkOrderPaidCommandHandler(IOrderRepository orderRepository, IEventBus eventBus)
    {
        _orderRepository = orderRepository;
        _eventBus = eventBus;
    }

    public async Task<Unit> Handle(MarkOrderPaidCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken)
                    ?? throw new InvalidOperationException($"Order {request.OrderId} not found");

        order.MarkPaid();
        await _orderRepository.UpdateAsync(order, cancellationToken);

        var evt = new PaymentAuthorizedIntegrationEvent(
            order.Id,
            order.TotalAmount,
            DateTime.UtcNow
        );

        await _eventBus.PublishAsync(PaymentAuthorizedTopic, evt, cancellationToken);

        return Unit.Value;
    }
}
