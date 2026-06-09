using MarketAPI.Domain.Entities;
using MarketAPI.Domain.Security.Tokens;
using Moq;

namespace CommonTestUtilities.Security.Tokens;

public class AccessTokenGeneratorBuilder
{
    public IAccessTokenGenerator Build()
    {
        var mock = new Mock<IAccessTokenGenerator>();

        mock.Setup(x => x.Generate(It.IsAny<User>()))
    .Returns("fake-access-token");

        return mock.Object;
    }
}
