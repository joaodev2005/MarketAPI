using MarketAPI.Communication.Responses;

namespace MarketAPI.Application.UseCases.Cart.GetCart;

public interface IGetCartUseCase
{
    Task<ResponseCartJson> Execute();
}