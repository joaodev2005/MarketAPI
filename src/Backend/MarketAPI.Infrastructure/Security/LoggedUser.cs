using System.Security.Claims;
using MarketAPI.Domain.Security;
using Microsoft.AspNetCore.Http;

namespace MarketAPI.Infrastructure.Security;

public class LoggedUser : ILoggedUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public LoggedUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public Guid GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext!.User
            .FindFirst(ClaimTypes.Sid)?.Value;

        return Guid.Parse(userId!);
    }

    public string GetUserEmail()
    {
        return _httpContextAccessor.HttpContext!.User
            .FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
    }

    public string GetUserName()
    {
        return _httpContextAccessor.HttpContext!.User
            .FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
    }
}