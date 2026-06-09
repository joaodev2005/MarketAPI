namespace MarketAPI.Domain.Repositories;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistActiveUserWithEmail(string email);
    Task<Entities.User?> GetByEmail(string email);
    Task<bool> ExistsAnyAdmin();
}