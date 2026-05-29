using MarketAPI.Domain.Entities;

namespace MarketAPI.Domain.Repositories;

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    Task<bool> ExistsByNameAsync(string name);
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(Guid id);
    Task DeleteAsync(Category category);
}