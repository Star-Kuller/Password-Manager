namespace PasswordManager.Application.Models;

public class SessionData
{
    public long UserId { get; set; }
    public DateTime ExpirationTime { get; set; }
}