namespace MarketAPI.Domain.Repositories.Product;

public interface IProductRepository
{
    Task AddAsync(Entities.Product product);
    Task<List<Entities.Product>> GetAllAsync();
    Task<Entities.Product?> GetByIdAsync(Guid id);
    Task DeleteAsync(Entities.Product product);
    Task<bool> ExistsByCategoryIdAsync(Guid categoryId);
}