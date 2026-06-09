using Bogus;
using MarketAPI.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestProductJsonBuilder
{
    public static RequestProductJson Build()
    {
        return new Faker<RequestProductJson>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Price, f => f.Random.Decimal(1, 10000))
            .RuleFor(p => p.Stock, f => f.Random.Int(1, 100))
            .RuleFor(p => p.CategoryId, f => f.Random.Guid())
            .Generate();
    }
}
