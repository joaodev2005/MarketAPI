using CommonTestUtilities.Repositories;
using FluentAssertions;
using MarketAPI.Application.UseCases.Order.Create;
using MarketAPI.Domain.Entities;

namespace UseCases.Tests.Order.Create;

public class CreateOrderUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var product = new MarketAPI.Domain.Entities.Product
        {
            Id = Guid.NewGuid(),
            Name = "Notebook",
            Price = 1000,
            Stock = 10
        };

        var cart = new MarketAPI.Domain.Entities.Cart
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Items =
            [
                new MarketAPI.Domain.Entities.CartItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    Quantity = 2,
                    Product = product
                }
            ]
        };

        var useCase = CreateUseCase(cart, product);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Total.Should().Be(2000);
    }

    [Fact]
    public async Task Error_Cart_Is_Empty()
    {
        var cart = new MarketAPI.Domain.Entities.Cart
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Items = []
        };

        var useCase = CreateUseCase(cart);

        var act = async () => await useCase.Execute();

        await act.Should().ThrowAsync<MarketAPI.Exception.ExceptionsBase.ErrorOnValidationException>();
    }

    [Fact]
    public async Task Error_Insufficient_Stock()
    {
        var product = new MarketAPI.Domain.Entities.Product
        {
            Id = Guid.NewGuid(),
            Name = "Notebook",
            Price = 1000,
            Stock = 1
        };

        var cart = new MarketAPI.Domain.Entities.Cart
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Items =
            [
                new MarketAPI.Domain.Entities.CartItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    Quantity = 5,
                    Product = product
                }
            ]
        };

        var useCase = CreateUseCase(cart, product);

        var act = async () => await useCase.Execute();

        await act.Should().ThrowAsync<MarketAPI.Exception.ExceptionsBase.ErrorOnValidationException>();
    }

    private CreateOrderUseCase CreateUseCase(
        MarketAPI.Domain.Entities.Cart? cart = null,
        MarketAPI.Domain.Entities.Product? product = null)
    {
        var cartRepositoryBuilder = new ICartRepositoryBuilder();
        var productRepositoryBuilder = new IProductRepositoryBuilder();

        if (cart is not null)
            cartRepositoryBuilder.GetByUserId(cart);

        if (product is not null)
            productRepositoryBuilder.GetById(product);

        return new CreateOrderUseCase(
            cartRepositoryBuilder.Build(),
            new IOrderRepositoryBuilder().Build(),
            productRepositoryBuilder.Build(),
            new ILoggedUserBuilder().Build(),
            IUnitOfWorkBuilder.Build(),
            new IMessagePublisherBuilder().Build());
    }
}
