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
}