using System.Text.Json;
using Confluent.Kafka;
using DotNetBackEnd.Application.Common;
using Microsoft.Extensions.Options;

namespace DotNetBackEnd.Infrastructure.Messaging;

public class KafkaEventBus : IEventBus, IDisposable
{
    private readonly IProducer<Null, string> _producer;

    public KafkaEventBus(IOptions<KafkaSettings> options)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = options.Value.BootstrapServers,
            Acks = Acks.Leader
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default)
    {
        var payload = JsonSerializer.Serialize(message);
        var msg = new Message<Null, string> { Value = payload };
        await _producer.ProduceAsync(topic, msg, cancellationToken);
    }

    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(5));
        _producer.Dispose();
    }
}
