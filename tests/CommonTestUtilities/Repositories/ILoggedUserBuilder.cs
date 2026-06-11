using MarketAPI.Domain.Security;
using Moq;

namespace CommonTestUtilities.Repositories;

public class ILoggedUserBuilder
{
    private readonly Mock<ILoggedUser> _mock = new();

    public ILoggedUserBuilder()
    {
        _mock.Setup(u => u.GetUserId()).Returns(Guid.NewGuid());
        _mock.Setup(u => u.GetUserEmail()).Returns("test@email.com");
        _mock.Setup(u => u.GetUserName()).Returns("Test User");
    }

    public ILoggedUserBuilder GetUserId(Guid userId)
    {
        _mock.Setup(x => x.GetUserId())
             .Returns(userId);

        return this;
    }

    public ILoggedUser Build() => _mock.Object;
}
