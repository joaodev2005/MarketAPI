namespace MarketAPI.Domain.Repositories;

public interface IUnitOfWork
{
    Task Commit();
}