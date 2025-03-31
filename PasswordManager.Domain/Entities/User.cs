namespace PasswordManager.Domain.Entities;

public class User : Entity
{
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public string SecretKey { get; set; }
}