using CommonTestUtilities.Repositories;
using FluentAssertions;
using MarketAPI.Application.UseCases.Category.List;

namespace UseCases.Tests.Category.List;

public class ListCategoriesUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var categories = new List<MarketAPI.Domain.Entities.Category>
        {
            new() { Id = Guid.NewGuid(), Name = "Category 1" },
            new() { Id = Guid.NewGuid(), Name = "Category 2" }
        };

        var useCase = CreateUseCase(categories);
        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Success_When_Categories_Exist()
    {
        var useCase = CreateUseCase([]);
        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    private ListCategoriesUseCase CreateUseCase(List<MarketAPI.Domain.Entities.Category> categories)
    {
        var categoryRepositoryBuilder = new ICategoryRepositoryBuilder();
        categoryRepositoryBuilder.GetAll(categories);

        return new ListCategoriesUseCase(categoryRepositoryBuilder.Build());
    }
}
