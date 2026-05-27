using MarketAPI.Domain.Repositories;

namespace MarketAPI.Infrastructure.DataAccess;

internal class UnitOfWork : IUnitOfWork
{
    private readonly MarketApiDbContext _dbContext;

    public UnitOfWork(MarketApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Commit() => await _dbContext.SaveChangesAsync();
}