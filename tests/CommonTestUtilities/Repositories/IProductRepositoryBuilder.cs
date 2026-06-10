using MarketAPI.Domain.Entities;
using MarketAPI.Domain.Repositories.Product;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IProductRepositoryBuilder
{
    private readonly Mock<IProductRepository> _mock = new();

    public IProductRepositoryBuilder GetById(Product product)
    {
        _mock.Setup(r => r.GetByIdAsync(product.Id))
            .ReturnsAsync(product);
        return this;
    }

    public IProductRepositoryBuilder GetAll(List<MarketAPI.Domain.Entities.Product> products)
    {
        _mock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(products);
        return this;
    }

    public IProductRepositoryBuilder ExistsByCategoryId(bool exists)
    {
        _mock.Setup(r => r.ExistsByCategoryIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(exists);
        return this;
    }

    public IProductRepository Build() => _mock.Object;
}
