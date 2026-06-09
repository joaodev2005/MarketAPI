using CommonTestUtilities.Cache;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using MarketAPI.Application.UseCases.Product.Delete;
using MarketAPI.Exception.ExceptionsBase;

namespace UseCases.Tests.Product.Delete;

public class DeleteProductUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var product = new MarketAPI.Domain.Entities.Product { Id = Guid.NewGuid(), Name = "Test" };

        var useCase = CreateUseCase(product);
        var act = async () => await useCase.Execute(product.Id);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_When_Product_Not_Found()
    {
        var useCase = CreateUseCase();
        var act = async () => await useCase.Execute(Guid.NewGuid());

        await act.Should().ThrowAsync<NotFoundException>();
    }

    private DeleteProductUseCase CreateUseCase(MarketAPI.Domain.Entities.Product? product = null)
    {
        var productRepositoryBuilder = new IProductRepositoryBuilder();

        if (product is not null)
            productRepositoryBuilder.GetById(product);

        return new DeleteProductUseCase(
            productRepositoryBuilder.Build(),
            IUnitOfWorkBuilder.Build(),
            new IDistributedCacheBuilder().Build());
    }
}
