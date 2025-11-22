using System.Text.Json;
using Confluent.Kafka;
using DotNetBackEnd.Application.IntegrationEvents;
using DotNetBackEnd.Application.Orders.Commands;
using MediatR;
using Microsoft.Extensions.Options;

namespace DotNetBackEnd.Infrastructure.Messaging.Consumers;

public class PaymentProcessorService : BackgroundService
{
    private readonly ILogger<PaymentProcessorService> _logger;
    private readonly KafkaSettings _settings;
    private readonly IServiceScopeFactory _scopeFactory;
    private const string InputTopic = "order-created";

    public PaymentProcessorService(
        ILogger<PaymentProcessorService> logger,
        IOptions<KafkaSettings> options,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _settings = options.Value;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(3000, stoppingToken);

        var config = new ConsumerConfig
        {
            BootstrapServers = _settings.BootstrapServers,
            GroupId = $"{_settings.ConsumerGroupId}-payment",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true
        };

        using var consumer = new ConsumerBuilder<Null, string>(config).Build();
        consumer.Subscribe(InputTopic);

        _logger.LogInformation("PaymentProcessorService started, subscribed to {Topic}", InputTopic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var cr = consumer.Consume(stoppingToken);
                var evt = JsonSerializer.Deserialize<OrderCreatedIntegrationEvent>(cr.Message.Value);

                if (evt is null)
                {
                    _logger.LogWarning("Received null OrderCreatedIntegrationEvent");
                    continue;
                }

                using var scope = _scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                await mediator.Send(new MarkOrderPaidCommand(evt.OrderId), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PaymentProcessorService loop");
                await Task.Delay(1000, stoppingToken);
            }
        }

        _logger.LogInformation("PaymentProcessorService stopping");
    }
}
