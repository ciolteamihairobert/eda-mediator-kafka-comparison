namespace DotNetBackEnd.Application.IntegrationEvents;

public record OrderCreatedIntegrationEvent(
    Guid OrderId,
    string CustomerId,
    decimal TotalAmount,
    string Currency,
    DateTime OccurredAtUtc
);

public record PaymentAuthorizedIntegrationEvent(
    Guid OrderId,
    decimal Amount,
    DateTime OccurredAtUtc
);

public record InventoryReservedIntegrationEvent(
    Guid OrderId,
    DateTime OccurredAtUtc
);

public record OrderCompletedIntegrationEvent(
    Guid OrderId,
    DateTime OccurredAtUtc
);
