namespace MarketAPI.Communication.Responses;

public class ResponseCartJson
{
    public List<ResponseCartItemJson> Items { get; set; } = [];
    public decimal Total { get; set; } 
}