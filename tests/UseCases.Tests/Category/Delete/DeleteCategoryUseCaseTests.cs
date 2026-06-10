using CommonTestUtilities.Repositories;
using FluentAssertions;
using MarketAPI.Application.UseCases.Category.Delete;
using MarketAPI.Exception.ExceptionsBase;

namespace UseCases.Tests.Category.Delete;

public class DeleteCategoryUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var category = new MarketAPI.Domain.Entities.Category { Id = Guid.NewGuid(), Name = "Test" };

        var useCase = CreateUseCase(category);
        var act = async () => await useCase.Execute(category.Id);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Category_Not_Found()
    {
        var useCase = CreateUseCase();
        var act = async () => await useCase.Execute(Guid.NewGuid());

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Error_Category_Has_Products()
    {
        var category = new MarketAPI.Domain.Entities.Category { Id = Guid.NewGuid(), Name = "Test" };

        var useCase = CreateUseCase(category, hasProducts: true);
        var act = async () => await useCase.Execute(category.Id);

        await act.Should().ThrowAsync<ConflictException>();
    }

    private DeleteCategoryUseCase CreateUseCase(MarketAPI.Domain.Entities.Category? category = null, bool hasProducts = false)
    {
        var categoryRepositoryBuilder = new ICategoryRepositoryBuilder();
        var productRepositoryBuilder = new IProductRepositoryBuilder();

        if (category is not null)
            categoryRepositoryBuilder.GetById(category);

        if (hasProducts)
            productRepositoryBuilder.ExistsByCategoryId(true);

        return new DeleteCategoryUseCase(
            categoryRepositoryBuilder.Build(),
            IUnitOfWorkBuilder.Build(),
            productRepositoryBuilder.Build());
    }
}
