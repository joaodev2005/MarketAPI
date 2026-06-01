namespace MarketAPI.Domain.Security;

public interface ILoggedUser
{
    Guid GetUserId();
}