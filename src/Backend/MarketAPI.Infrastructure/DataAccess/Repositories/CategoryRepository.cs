using MarketAPI.Domain.Entities;
using MarketAPI.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MarketAPI.Infrastructure.DataAccess.Repositories;

internal class CategoryRepository : ICategoryRepository
{
    private readonly MarketApiDbContext _dbContext;

    public CategoryRepository(MarketApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Category category)
        => await _dbContext.Categories.AddAsync(category);
    
    public async Task<bool> ExistsByNameAsync(string name)
        => await _dbContext.Categories.AnyAsync(c => c.Name == name);

    public async Task<List<Category>> GetAllAsync()
        => await _dbContext.Categories.ToListAsync();

    public async Task<Category?> GetByIdAsync(Guid id)
        => await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);

    public async Task DeleteAsync(Category category)
        => _dbContext.Categories.Remove(category);
}