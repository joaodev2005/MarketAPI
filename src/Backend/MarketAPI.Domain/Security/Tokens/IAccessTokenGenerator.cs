using MarketAPI.Domain.Entities;

namespace MarketAPI.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    string Generate(User user);
}