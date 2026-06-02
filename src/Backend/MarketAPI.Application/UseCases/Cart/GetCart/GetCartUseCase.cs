using MarketAPI.Communication.Responses;
using MarketAPI.Domain.Repositories.Cart;
using MarketAPI.Domain.Security;

namespace MarketAPI.Application.UseCases.Cart.GetCart;

public class GetCartUseCase : IGetCartUseCase
{
    private readonly ICartRepository _cartRepository;
    private readonly ILoggedUser _loggedUser;

    public GetCartUseCase(ICartRepository cartRepository, ILoggedUser loggedUser)
    {
        _cartRepository = cartRepository;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseCartJson> Execute()
    {
        var userId = _loggedUser.GetUserId();
        var cart = await _cartRepository.GetByUserIdAsync(userId);

        if (cart is null)
            return new ResponseCartJson();

        var items = cart.Items.Select(i => new ResponseCartItemJson
        {
            ItemId = i.Id,
            ProductId = i.ProductId,
            ProductName = i.Product.Name,
            Price = i.Product.Price,
            Quantity = i.Quantity,
            Subtotal = i.Product.Price * i.Quantity
        }).ToList();

        return new ResponseCartJson
        {
            Items = items,
            Total = items.Sum(i => i.Subtotal)
        };
    }
}