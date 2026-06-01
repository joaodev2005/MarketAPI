namespace MarketAPI.Communication.Requests;

public class RequestAddItemToCartJson
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}