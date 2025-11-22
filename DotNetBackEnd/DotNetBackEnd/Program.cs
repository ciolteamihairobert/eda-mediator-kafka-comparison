using DotNetBackEnd.Application.Common;
using DotNetBackEnd.Application.Orders.Commands;
using DotNetBackEnd.Domain.Repositories;
using DotNetBackEnd.Infrastructure.Data;
using DotNetBackEnd.Infrastructure.Messaging;
using DotNetBackEnd.Infrastructure.Messaging.Consumers;
using DotNetBackEnd.Infrastructure.Repositories.Ef;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Kafka config
builder.Services.Configure<KafkaSettings>(
    builder.Configuration.GetSection("Kafka"));

// EF Core + SQL Server
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderDb")));

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly);
});

// Repository
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();

// Event bus
builder.Services.AddSingleton<IEventBus, KafkaEventBus>();

// Kafka background consumers
builder.Services.AddHostedService<PaymentProcessorService>();
builder.Services.AddHostedService<InventoryProcessorService>();
builder.Services.AddHostedService<OrderCompletionProcessorService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapGet("/health", () => "OK");

app.Run();
