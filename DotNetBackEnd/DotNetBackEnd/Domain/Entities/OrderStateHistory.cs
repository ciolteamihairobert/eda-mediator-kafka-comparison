namespace DotNetBackEnd.Domain.Entities;

public class OrderStateHistory
{
    public int Id { get; set; }
    public Guid OrderId { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime ChangedAtUtc { get; set; }
    public Order? Order { get; set; }
}
