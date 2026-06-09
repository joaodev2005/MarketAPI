using Bogus;
using MarketAPI.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestAddItemToCartJsonBuilder
{
    public static RequestAddItemToCartJson Build()
    {
        return new Faker<RequestAddItemToCartJson>()
            .RuleFor(r => r.ProductId, f => f.Random.Guid())
            .RuleFor(r => r.Quantity, f => f.Random.Int(1, 10))
            .Generate();
    }
}