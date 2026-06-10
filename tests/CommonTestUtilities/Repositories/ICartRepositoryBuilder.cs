using MarketAPI.Domain.Entities;
using MarketAPI.Domain.Repositories.Cart;
using Moq;

namespace CommonTestUtilities.Repositories;

public class ICartRepositoryBuilder
{
    private readonly Mock<ICartRepository> _mock = new();

    public void GetByUserId(Cart cart)
    {
        _mock.Setup(repository =>
                repository.GetByUserIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(cart);
    }

    public ICartRepository Build() => _mock.Object;
}
