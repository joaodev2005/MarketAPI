namespace MarketAPI.Domain.Security;

public interface ILoggedUser
{
    Guid GetUserId();
    string GetUserEmail();
    string GetUserName();
}