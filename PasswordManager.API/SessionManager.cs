using FastEndpoints.Security;
using PasswordManager.Application.Interfaces;

namespace PasswordManager.API;

public class SessionManager(IHttpContextAccessor httpContextAccessor) : ISessionManager
{
    public async Task CreateSession(long userId)
    {
        await CookieAuth.SignInAsync(u =>
        {
            u["Id"] = userId.ToString();
        });
    }

    public async Task EndSession()
    {
        await CookieAuth.SignOutAsync();
    }
    
    public long? GetCurrentUserId()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
            return null;
        
        var idClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
        if (long.TryParse(idClaim, out var userId))
            return userId;

        return null;
    }
}