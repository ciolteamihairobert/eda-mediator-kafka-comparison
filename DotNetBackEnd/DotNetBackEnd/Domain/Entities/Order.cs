namespace DotNetBackEnd.Domain.Entities;

public enum OrderStatus
{
    Pending = 0,
    Paid = 1,
    InventoryReserved = 2,
    Completed = 3,
    Cancelled = 4
}

public class Order
{
    public Guid Id { get; private set; }
    public string CustomerId { get; private set; } = default!;
    public decimal TotalAmount { get; private set; }
    public string Currency { get; private set; } = "EUR";
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }
    public ICollection<OrderStateHistory> History { get; private set; } = new List<OrderStateHistory>();

    private Order() { }

    public Order(string customerId, decimal totalAmount, string currency = "EUR")
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        TotalAmount = totalAmount;
        Currency = currency;
        Status = OrderStatus.Pending;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void MarkPaid()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException($"Cannot mark order {Id} as Paid from status {Status}.");

        Status = OrderStatus.Paid;
    }

    public void MarkInventoryReserved()
    {
        if (Status != OrderStatus.Paid)
            throw new InvalidOperationException($"Cannot mark order {Id} as InventoryReserved from status {Status}.");

        Status = OrderStatus.InventoryReserved;
    }

    public void MarkCompleted()
    {
        if (Status != OrderStatus.InventoryReserved)
            throw new InvalidOperationException($"Cannot complete order {Id} from status {Status}.");

        Status = OrderStatus.Completed;
        CompletedAtUtc = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException($"Cannot cancel already completed order {Id}.");

        Status = OrderStatus.Cancelled;
    }
}
