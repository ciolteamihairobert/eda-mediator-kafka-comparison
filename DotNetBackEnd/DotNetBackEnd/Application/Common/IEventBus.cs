namespace DotNetBackEnd.Application.Common;

public interface IEventBus
{
    Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default);
}
