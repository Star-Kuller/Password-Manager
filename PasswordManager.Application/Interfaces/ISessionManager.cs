

namespace PasswordManager.Application.Interfaces;

public interface ISessionManager
{
    Task CreateSession(long userId);
    Task EndSession();
    long? GetCurrentUserId();
}