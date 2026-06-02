using MarketAPI.Domain.Repositories;
using MarketAPI.Domain.Repositories.Cart;
using MarketAPI.Domain.Security;
using MarketAPI.Exception.ExceptionsBase;

namespace MarketAPI.Application.UseCases.Cart.ClearCart;

public class ClearCartUseCase : IClearCartUseCase
{
    private readonly ICartRepository _cartRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;

    public ClearCartUseCase(
        ICartRepository cartRepository, 
        ILoggedUser loggedUser, 
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute()
    {
        var userId = _loggedUser.GetUserId();
        var cart = await _cartRepository.GetByUserIdAsync(userId);

        if (cart is null)
            throw new NotFoundException("Cart not found");

        await _cartRepository.ClearAsync(cart);
        await _unitOfWork.Commit();
    }
}