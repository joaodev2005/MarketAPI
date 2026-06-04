using System.Text;
using System.Text.Json;
using MarketAPI.Domain.Messaging.Messages;
using MarketAPI.Worker.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MarketAPI.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConnection _connection;
    private readonly IEmailService _emailService;

    public Worker(ILogger<Worker> logger, IConnection connection, IEmailService emailService)
    {
        _logger = logger;
        _connection = connection;
        _emailService = emailService;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(
            queue: Queues.OrderCreated,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = JsonSerializer.Deserialize<OrderCreatedMessage>(
                    Encoding.UTF8.GetString(body));

                if (message is null)
                {
                    _logger.LogWarning("Invalid message received");
                    await channel.BasicNackAsync(ea.DeliveryTag, false, false, stoppingToken);
                    return;
                }

                _logger.LogInformation("Order received: {OrderId} for {Email}",
                    message.OrderId, message.CustomerEmail);

                await _emailService.SendOrderCreatedEmailAsync(message);

                await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");
                await channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);
            }
        };

        await channel.BasicConsumeAsync(
            queue: Queues.OrderCreated,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
            await Task.Delay(1000, stoppingToken);
    }
}
