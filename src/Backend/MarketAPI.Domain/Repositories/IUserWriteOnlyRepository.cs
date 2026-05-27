namespace MarketAPI.Domain.Repositories;

public interface IUserWriteOnlyRepository
{
    Task Add(Entities.User user);
}