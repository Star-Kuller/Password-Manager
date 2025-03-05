namespace PasswordManager.Domain.Entities;

public class User
{
    public long? Id { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; } = false;
    public string PasswordHash { get; set; }
    public string SecretKey { get; set; }
}