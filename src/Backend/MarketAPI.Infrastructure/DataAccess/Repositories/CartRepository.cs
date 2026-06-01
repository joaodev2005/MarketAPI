using MarketAPI.Domain.Entities;
using MarketAPI.Domain.Repositories.Cart;
using Microsoft.EntityFrameworkCore;

namespace MarketAPI.Infrastructure.DataAccess.Repositories;

internal sealed class CartRepository : ICartRepository
{
    private readonly MarketApiDbContext _dbContext;
    
    public CartRepository(MarketApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<Cart?> GetByUserIdAsync(Guid userId)
        => _dbContext.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

    public async Task AddAsync(Cart cart)
        => await _dbContext.Carts.AddAsync(cart);

    public async Task AddItemAsync(CartItem item)
        => await _dbContext.CartItems.AddAsync(item);

    public Task RemoveItemAsync(CartItem item)
    {
        _dbContext.CartItems.Remove(item);
        return Task.CompletedTask;
    }

    public Task ClearAsync(Cart cart)
    {
        _dbContext.CartItems.RemoveRange(cart.Items);
        return Task.CompletedTask;
    }
}