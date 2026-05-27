using MarketAPI.Domain.Entities;
using MarketAPI.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MarketAPI.Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
    private readonly MarketApiDbContext _dbContext;

    public UserRepository(MarketApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);

    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        return await _dbContext.Users.AnyAsync(user => user.Active && user.Email.Equals(email));
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(user => user.Active && user.Email.Equals(email));
    }
}