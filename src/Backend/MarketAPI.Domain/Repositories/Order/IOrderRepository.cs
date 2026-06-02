namespace MarketAPI.Domain.Repositories.Order;

public interface IOrderRepository
{
    Task AddAsync(Entities.Order order);
    Task<List<Entities.Order>> GetByUserIdAsync(Guid userId);
    Task<Entities.Order?> GetByIdAsync(Guid id);
    Task<List<Entities.Order>> GetAllAsync();
}