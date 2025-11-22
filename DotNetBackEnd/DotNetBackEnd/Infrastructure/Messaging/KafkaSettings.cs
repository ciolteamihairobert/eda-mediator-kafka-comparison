namespace DotNetBackEnd.Infrastructure.Messaging;

public class KafkaSettings
{
    public string BootstrapServers { get; set; } = "localhost:9092";
    public string ConsumerGroupId { get; set; } = "order-service-group";
}
