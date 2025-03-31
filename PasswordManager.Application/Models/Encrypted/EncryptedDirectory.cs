using PasswordManager.Application.Interfaces;
using Directory = PasswordManager.Domain.Entities.Directory;

namespace PasswordManager.Application.Models.Encrypted;

public class EncryptedDirectory() : EncryptedEntity<Directory>()
{
    public byte[] Title { get; set; }
    
    public long? ParentId { get; set; }
    public EncryptedDirectory? Parent { get; set; }
    
    public long OwnerId { get; set; }
    public EncryptedUser? Owner { get; set; }

    public List<EncryptedAccount> Accounts { get; set; } = new();
    
    public EncryptedDirectory(Directory directory, ICryptographer cryptographer) : this()
    {
        Id = directory.Id;
        Title = cryptographer.Encrypt(directory.Title);
        ParentId = directory.ParentId;
        OwnerId = directory.OwnerId;
    }
    
    public override Directory ToEntity(ICryptographer cryptographer)
    {
        return new Directory
        {
            Id = Id,
            Title = cryptographer.Decrypt(Title),
            ParentId = ParentId,
            Parent = Parent?.ToEntity(cryptographer),
            OwnerId = OwnerId,
            Owner = Owner?.ToEntity(cryptographer),
            Accounts = Accounts.Select(a => a.ToEntity(cryptographer)).ToList()
        };
    }
}