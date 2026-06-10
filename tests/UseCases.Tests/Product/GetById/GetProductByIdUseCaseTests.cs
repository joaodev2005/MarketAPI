using CommonTestUtilities.Repositories;
using FluentAssertions;
using MarketAPI.Application.UseCases.Product.GetById;
using MarketAPI.Domain.Entities;
using MarketAPI.Exception.ExceptionsBase;

namespace UseCases.Tests.Product.GetById;

public class GetProductByIdUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var product = new MarketAPI.Domain.Entities.Product
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Description = "Test Description",
            Price = 100,
            Stock = 10,
            Category = new MarketAPI.Domain.Entities.Category { Name = "Test Category" }
        };

        var useCase = CreateUseCase(product);
        var result = await useCase.Execute(product.Id);

        result.Should().NotBeNull();
        result.Name.Should().Be(product.Name);
    }

    [Fact]
    public async Task Error_Product_Not_Found()
    {
        var useCase = CreateUseCase();
        var act = async () => await useCase.Execute(Guid.NewGuid());

        await act.Should().ThrowAsync<NotFoundException>();
    }

    private GetProductByIdUseCase CreateUseCase(MarketAPI.Domain.Entities.Product? product = null)
    {
        var productRepositoryBuilder = new IProductRepositoryBuilder();

        if (product is not null)
            productRepositoryBuilder.GetById(product);

        return new GetProductByIdUseCase(productRepositoryBuilder.Build());
    }
}
