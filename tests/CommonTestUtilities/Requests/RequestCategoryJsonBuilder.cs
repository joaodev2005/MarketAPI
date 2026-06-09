using Bogus;
using MarketAPI.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestCategoryJsonBuilder
{
    public static RequestCategoryJson Build()
    {
        return new Faker<RequestCategoryJson>()
            .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0])
            .Generate();
    }
}
