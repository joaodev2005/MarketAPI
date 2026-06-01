using MarketAPI.Communication.Requests;

namespace MarketAPI.Application.UseCases.Cart.AddItem;

public interface IAddItemToCartUseCase
{
    Task Execute(RequestAddItemToCartJson request);
}