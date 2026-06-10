using MarketAPI.Domain.Entities;
using MarketAPI.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories;

public class ICategoryRepositoryBuilder
{
    private readonly Mock<ICategoryRepository> _mock = new();

    public ICategoryRepositoryBuilder GetById(Category category)
    {
        _mock.Setup(r => r.GetByIdAsync(category.Id))
            .ReturnsAsync(category);
        return this;
    }
    public ICategoryRepositoryBuilder ExistsByName(bool exists)
    {
        _mock.Setup(r => r.ExistsByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(exists);
        return this;
    }

    public ICategoryRepositoryBuilder GetAll(List<Category> categories)
    {
        _mock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(categories);
        return this;
    }

    public ICategoryRepository Build() => _mock.Object;

}
