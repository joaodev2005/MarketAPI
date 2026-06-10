using MarketAPI.Domain.Repositories.Cart;
using Moq;

namespace CommonTestUtilities.Repositories;

public class ICartRepositoryBuilder
{
    private readonly Mock<ICartRepository> _mock = new();

    public ICartRepository Build() => _mock.Object;
}
