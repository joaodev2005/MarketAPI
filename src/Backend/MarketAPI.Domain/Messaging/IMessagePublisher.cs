namespace MarketAPI.Domain.Messaging;

public interface IMessagePublisher
{
    void Publish<T>(T message, string queueName);
}