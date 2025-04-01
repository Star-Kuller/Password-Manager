using PasswordManager.Application.Interfaces;
using Directory = PasswordManager.Domain.Entities.Directory;

// ReSharper disable InconsistentNaming

namespace PasswordManager.Application.Models.Encrypted;

public class EncryptedDirectory() : EncryptedEntity<Directory>()
{
    public byte[] Title { get; set; }
    
    public long? Parent_id { get; set; }
    public EncryptedDirectory? Parent { get; set; }
    
    public long Owner_id { get; set; }
    public EncryptedUser? Owner { get; set; }

    public List<EncryptedAccount> Accounts { get; set; } = new();
    
    public EncryptedDirectory(Directory directory, ICryptographer cryptographer) : this()
    {
        Id = directory.Id;
        Title = cryptographer.Encrypt(directory.Title);
        Parent_id = directory.ParentId;
        Owner_id = directory.OwnerId;
    }
    
    public override Directory ToEntity(ICryptographer cryptographer)
    {
        return new Directory
        {
            Id = Id,
            Title = cryptographer.Decrypt(Title),
            ParentId = Parent_id,
            Parent = Parent?.ToEntity(cryptographer),
            OwnerId = Owner_id,
            Owner = Owner?.ToEntity(cryptographer),
            Accounts = Accounts.Select(a => a.ToEntity(cryptographer)).ToList()
        };
    }
}