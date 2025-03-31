using FastEndpoints.Security;
using PasswordManager.Application.Interfaces;

namespace PasswordManager.API;

public class SessionManager : ISessionManager
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
}