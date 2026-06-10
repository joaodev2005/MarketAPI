using CommonTestUtilities.Repositories;
using FluentAssertions;
using MarketAPI.Application.UseCases.Cart.RemoveItem;
using MarketAPI.Domain.Entities;
using MarketAPI.Exception.ExceptionsBase;

namespace UseCases.Tests.Cart.RemoveItem;

public class RemoveItemFromCartUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var item = new CartItem
        {
            Id = Guid.NewGuid()
        };

        var cart = new MarketAPI.Domain.Entities.Cart
        {
            UserId = Guid.NewGuid(),
            Items = [item]
        };

        var useCase = CreateUseCase(cart);

        var act = async () => await useCase.Execute(item.Id);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Cart_Not_Found()
    {
        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(Guid.NewGuid());

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Error_Item_Not_Found()
    {
        var cart = new MarketAPI.Domain.Entities.Cart
        {
            UserId = Guid.NewGuid(),
            Items = []
        };

        var useCase = CreateUseCase(cart);

        var act = async () => await useCase.Execute(Guid.NewGuid());

        await act.Should().ThrowAsync<NotFoundException>();
    }

    private RemoveItemFromCartUseCase CreateUseCase(
        MarketAPI.Domain.Entities.Cart? cart = null)
    {
        var cartRepositoryBuilder = new ICartRepositoryBuilder();

        if (cart is not null)
            cartRepositoryBuilder.GetByUserId(cart);

        return new RemoveItemFromCartUseCase(
            cartRepositoryBuilder.Build(),
            new ILoggedUserBuilder().Build(),
            IUnitOfWorkBuilder.Build());
    }
}
