namespace MarketAPI.Domain.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T message, string queueName);
}