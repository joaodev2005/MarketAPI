using MarketAPI.Domain.Security.PasswordHashing;
using Moq;

namespace CommonTestUtilities.Security.PasswordHashing;

public class IPasswordHasherBuilder
{
    private readonly Mock<IPasswordHasher> _mock;
    public IPasswordHasherBuilder()
    {
        _mock = new Mock<IPasswordHasher>();

        _mock.Setup(passwordHasher => passwordHasher.HashPassword(It.IsAny<string>())).Returns("hashed-Password");
    }

    public void VerifyPassword(string password)
    {
        _mock.Setup(repository => repository.VerifyPassword(password, It.IsAny<string>())).Returns(true);
    }

    public IPasswordHasher Build() => _mock.Object;
}
