namespace PasswordManager.Domain.Entities;

public class Directory : Entity
{
    public string Title { get; set; }
    
    public long? ParentId { get; set; }
    public Directory? Parent { get; set; }
    
    public long OwnerId { get; set; }
    public User? Owner { get; set; }
    
    public List<Account> Accounts { get; set; }
}