namespace MarketAPI.Domain.Repositories;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistActiveUserWithEmail(string email);
    Task<Domain.Entities.User?> GetByEmail(string email);
    Task<bool> ExistsAnyAdmin();
}