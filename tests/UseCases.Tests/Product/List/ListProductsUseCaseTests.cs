using CommonTestUtilities.Cache;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using MarketAPI.Application.UseCases.Product.List;
using MarketAPI.Communication.Responses;
using MarketAPI.Domain.Entities;

namespace UseCases.Tests.Product.List;

public class ListProductsUseCaseTests
{
    [Fact]
    public async Task Success_From_Database()
    {
        var products = new List<MarketAPI.Domain.Entities.Product>
        {
            new() { Id = Guid.NewGuid(), Name = "Product 1", Price = 100, Stock = 10, Category = new Category { Name = "Cat 1" } },
            new() { Id = Guid.NewGuid(), Name = "Product 2", Price = 200, Stock = 5, Category = new Category { Name = "Cat 2" } }
        };

        var useCase = CreateUseCase(products);
        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Success_From_Cache()
    {
        var cachedProducts = new List<ResponseProductJson>
    {
        new() { Id = Guid.NewGuid(), Name = "Cached Product", Price = 100 }
    };

        var useCase = new ListProductsUseCase(
            new IProductRepositoryBuilder().Build(),
            new IDistributedCacheBuilder().WithCachedProducts(cachedProducts).Build());

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Cached Product");
    }

    [Fact]
    public async Task Success_When_No_Products_Exist()
    {
        var useCase = CreateUseCase([]);
        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    private ListProductsUseCase CreateUseCase(List<MarketAPI.Domain.Entities.Product>? products = null)
    {
        var productRepositoryBuilder = new IProductRepositoryBuilder();

        if (products is not null)
            productRepositoryBuilder.GetAll(products);

        return new ListProductsUseCase(
            productRepositoryBuilder.Build(),
            new IDistributedCacheBuilder().Build());
    }
}
