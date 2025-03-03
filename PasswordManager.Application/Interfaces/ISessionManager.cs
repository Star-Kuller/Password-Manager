using PasswordManager.Application.Models;

namespace PasswordManager.Application.Interfaces;

public interface ISessionManager
{
    string CreateSession(long userId);
    bool ValidateSession(string sessionId, out SessionData sessionData);
    void EndSession(string sessionId);
}