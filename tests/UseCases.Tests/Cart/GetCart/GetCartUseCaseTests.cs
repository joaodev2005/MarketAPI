using CommonTestUtilities.Repositories;
using FluentAssertions;
using MarketAPI.Application.UseCases.Cart.GetCart;
using MarketAPI.Domain.Entities;

namespace UseCases.Tests.Cart.GetCart;

public class GetCartUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var cart = new MarketAPI.Domain.Entities.Cart
        {
            UserId = Guid.NewGuid(),
            Items =
            [
                new CartItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.NewGuid(),
                    Quantity = 2,
                    Product = new MarketAPI.Domain.Entities.Product
                    {
                        Name = "Notebook",
                        Price = 5000
                    }
                }
            ]
        };

        var useCase = CreateUseCase(cart);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Total.Should().Be(10000);
    }

    [Fact]
    public async Task Success_Empty_Cart()
    {
        var useCase = CreateUseCase();

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.Total.Should().Be(0);
    }
    private GetCartUseCase CreateUseCase(
        MarketAPI.Domain.Entities.Cart? cart = null)
    {
        var cartRepositoryBuilder = new ICartRepositoryBuilder();

        if (cart is not null)
            cartRepositoryBuilder.GetByUserId(cart);

        return new GetCartUseCase(
            cartRepositoryBuilder.Build(),
            new ILoggedUserBuilder().Build());
    }
}
