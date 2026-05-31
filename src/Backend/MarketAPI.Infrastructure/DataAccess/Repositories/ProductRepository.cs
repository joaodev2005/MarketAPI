using MarketAPI.Domain.Entities;
using MarketAPI.Domain.Repositories.Product;
using Microsoft.EntityFrameworkCore;

namespace MarketAPI.Infrastructure.DataAccess.Repositories;

internal sealed class ProductRepository : IProductRepository
{
    private readonly MarketApiDbContext _dbContext;

    public ProductRepository(MarketApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Product product)
        => await _dbContext.Products.AddAsync(product);

    public Task<List<Product>> GetAllAsync()
        => _dbContext.Products.Include(p => p.Category).ToListAsync();

    public Task<Product?> GetByIdAsync(Guid id)
        => _dbContext.Products.Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

    public Task DeleteAsync(Product product)
    {
        _dbContext.Products.Remove(product);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByCategoryIdAsync(Guid categoryId)
        => _dbContext.Products.AnyAsync(p => p.CategoryId == categoryId);
}