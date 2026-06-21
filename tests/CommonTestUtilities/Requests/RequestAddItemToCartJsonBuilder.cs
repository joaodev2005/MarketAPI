using Bogus;
using MarketAPI.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestAddItemToCartJsonBuilder
{
    public static RequestAddItemToCartJson Build()
    {
        return CustomFaker(Guid.NewGuid()).Generate();
    }

    public static RequestAddItemToCartJson Build(Guid productId)
    {
        return CustomFaker(productId).Generate();
    }

    private static Faker<RequestAddItemToCartJson> CustomFaker(Guid productId)
    {
        return new Faker<RequestAddItemToCartJson>()
            .RuleFor(r => r.ProductId, _ => productId)
            .RuleFor(r => r.Quantity, f => f.Random.Int(1, 10));
    }
}