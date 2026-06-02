namespace MarketAPI.Application.UseCases.Cart.RemoveItem;

public interface IRemoveItemFromCartUseCase
{
    Task Execute(Guid itemId);
}