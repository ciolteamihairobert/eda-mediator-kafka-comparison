using DotNetBackEnd.Domain.Entities;
using DotNetBackEnd.Domain.Repositories;
using DotNetBackEnd.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DotNetBackEnd.Infrastructure.Repositories.Ef;

public class EFOrderRepository : IOrderRepository
{
    private readonly OrderDbContext _db;

    public EFOrderRepository(OrderDbContext db)
    {
        _db = db;
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _db.Orders.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        _db.Orders.Add(order);

        _db.OrderHistory.Add(new OrderStateHistory
        {
            OrderId = order.Id,
            Status = order.Status,
            ChangedAtUtc = DateTime.UtcNow
        });

        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        _db.Orders.Update(order);

        _db.OrderHistory.Add(new OrderStateHistory
        {
            OrderId = order.Id,
            Status = order.Status,
            ChangedAtUtc = DateTime.UtcNow
        });

        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _db.Orders.AsNoTracking().ToListAsync(cancellationToken);
}
