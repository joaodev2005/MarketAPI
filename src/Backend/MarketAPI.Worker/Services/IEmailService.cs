using MarketAPI.Domain.Messaging.Messages;

namespace MarketAPI.Worker.Services;

public interface IEmailService
{
    Task SendOrderCreatedEmailAsync(OrderCreatedMessage message);
}