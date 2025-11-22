using MediatR;
using DotNetBackEnd.Application.Common;
using DotNetBackEnd.Application.IntegrationEvents;
using DotNetBackEnd.Application.Orders.Commands;
using DotNetBackEnd.Domain.Entities;
using DotNetBackEnd.Domain.Repositories;

namespace DotNetBackEnd.Application.Orders.Handlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IEventBus _eventBus;
    private const string OrderCreatedTopic = "order-created";

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IEventBus eventBus)
    {
        _orderRepository = orderRepository;
        _eventBus = eventBus;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order(request.CustomerId, request.TotalAmount, request.Currency);

        await _orderRepository.AddAsync(order, cancellationToken);

        var evt = new OrderCreatedIntegrationEvent(
            order.Id,
            order.CustomerId,
            order.TotalAmount,
            order.Currency,
            DateTime.UtcNow
        );

        await _eventBus.PublishAsync(OrderCreatedTopic, evt, cancellationToken);

        return order.Id;
    }
}
