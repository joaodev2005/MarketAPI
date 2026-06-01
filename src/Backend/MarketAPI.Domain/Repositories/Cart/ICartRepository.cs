using MarketAPI.Domain.Entities;

namespace MarketAPI.Domain.Repositories.Cart;

public interface ICartRepository
{
    Task<Entities.Cart?> GetByUserIdAsync(Guid userId);
    Task AddAsync(Entities.Cart cart);
    Task AddItemAsync(CartItem item);
    Task RemoveItemAsync(CartItem item);
    Task ClearAsync(Entities.Cart cart);
}