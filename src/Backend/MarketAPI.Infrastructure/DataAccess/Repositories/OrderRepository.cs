using MarketAPI.Domain.Entities;
using MarketAPI.Domain.Repositories.Order;
using Microsoft.EntityFrameworkCore;

namespace MarketAPI.Infrastructure.DataAccess.Repositories;

internal sealed class OrderRepository : IOrderRepository
{
    private readonly MarketApiDbContext _dbContext;

    public OrderRepository(MarketApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Order order)
        => await _dbContext.Orders.AddAsync(order);

    public Task<List<Order>> GetByUserIdAsync(Guid userId)
        => _dbContext.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

    public Task<Order?> GetByIdAsync(Guid id)
        => _dbContext.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

    public Task<List<Order>> GetAllAsync()
        => _dbContext.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Include(o => o.User)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
}