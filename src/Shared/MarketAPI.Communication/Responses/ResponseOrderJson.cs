namespace MarketAPI.Communication.Responses;

public class ResponseOrderJson
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ResponseOrderItemJson> Items { get; set; } = [];
}