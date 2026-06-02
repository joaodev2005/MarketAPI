using MarketAPI.Domain.Repositories;
using MarketAPI.Domain.Repositories.Cart;
using MarketAPI.Domain.Security;
using MarketAPI.Exception.ExceptionsBase;

namespace MarketAPI.Application.UseCases.Cart.RemoveItem;

public class RemoveItemFromCartUseCase : IRemoveItemFromCartUseCase
{
    private readonly ICartRepository _cartRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    
    public RemoveItemFromCartUseCase(
        ICartRepository cartRepository,
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Execute(Guid itemId)
    {
        var userId = _loggedUser.GetUserId();
        var cart = await _cartRepository.GetByUserIdAsync(userId);

        if (cart is null)
            throw new NotFoundException("Cart not found");

        var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
        if (item is null)
            throw new NotFoundException("Item not found");

        await _cartRepository.RemoveItemAsync(item);
        await _unitOfWork.Commit();
    }
}