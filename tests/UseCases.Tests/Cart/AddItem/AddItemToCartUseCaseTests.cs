using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MarketAPI.Application.UseCases.Cart.AddItem;
using MarketAPI.Exception.ExceptionsBase;

namespace UseCases.Tests.Cart.AddItem;

public class AddItemToCartUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var product = new MarketAPI.Domain.Entities.Product
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Stock = 10
        };

        var request = RequestAddItemToCartJsonBuilder.Build();
        request.ProductId = product.Id;
        request.Quantity = 1;

        var useCase = CreateUseCase(product);
        var act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Product_Not_Found()
    {
        var request = RequestAddItemToCartJsonBuilder.Build();
        var useCase = CreateUseCase();
        var act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<ErrorOnValidationException>();
    }

    [Fact]
    public async Task Error_Insufficient_Stock()
    {
        var product = new MarketAPI.Domain.Entities.Product
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Stock = 1
        };

        var request = RequestAddItemToCartJsonBuilder.Build();
        request.ProductId = product.Id;
        request.Quantity = 5;

        var useCase = CreateUseCase(product);
        var act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<ErrorOnValidationException>();
    }

    private AddItemToCartUseCase CreateUseCase(MarketAPI.Domain.Entities.Product? product = null)
    {
        var productRepositoryBuilder = new IProductRepositoryBuilder();

        if (product is not null)
            productRepositoryBuilder.GetById(product);

        return new AddItemToCartUseCase(
            new ICartRepositoryBuilder().Build(),
            productRepositoryBuilder.Build(),
            new ILoggedUserBuilder().Build(),
            IUnitOfWorkBuilder.Build());
    }
}
