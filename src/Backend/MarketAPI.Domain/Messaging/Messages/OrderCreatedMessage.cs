namespace MarketAPI.Domain.Messaging.Messages;

public class OrderCreatedMessage
{
    public Guid OrderId { get; set; }
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public List<OrderItemMessage> Items { get; set; } = [];
}