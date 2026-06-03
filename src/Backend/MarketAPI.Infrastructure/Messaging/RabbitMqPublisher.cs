using System.Text;
using System.Text.Json;
using MarketAPI.Domain.Messaging;
using RabbitMQ.Client;

namespace MarketAPI.Infrastructure.Messaging;

public class RabbitMqPublisher : IMessagePublisher
{
    private readonly IConnection _connection;

    public RabbitMqPublisher(IConnection connection)
    {
        _connection = connection;
    }

    public void Publish<T>(T message, string queueName)
    {
        using var channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

        channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false).GetAwaiter().GetResult();

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            body: body).GetAwaiter().GetResult();
    }
}