using DotNetBackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotNetBackEnd.Infrastructure.Data;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options)
        : base(options) { }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderStateHistory> OrderHistory => Set<OrderStateHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.CustomerId).IsRequired();
            entity.Property(o => o.TotalAmount).HasPrecision(18, 2);
            entity.Property(o => o.Currency).HasMaxLength(5);
            entity.Property(o => o.Status).HasConversion<int>();
        });

        modelBuilder.Entity<OrderStateHistory>(entity =>
        {
            entity.HasKey(h => h.Id);
            entity.Property(h => h.Status).HasConversion<int>();
            entity.HasIndex(h => h.OrderId);
        });
    }
}
