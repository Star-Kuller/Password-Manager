namespace PasswordManager.Domain.Entities;

public class Account : Entity
{
    public string Title { get; set; }
    public string Url { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    
    public long DirectoryId { get; set; }
    public Directory? Directory { get; set; }
}