namespace PasswordManager.Domain.Entities;

public class User
{
    public long? UserId { get; set; }
    public byte[] Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] SecretKey { get; set; }
}