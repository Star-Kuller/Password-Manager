using PasswordManager.Application.Models.Encrypted;

namespace PasswordManager.Tests.Infrastructure.TestDataBuilders;

public class EncryptedDirectoryBuilder : ITestDataBuilder<EncryptedDirectory>
{
    public long Id { get; set; } = 1;

    public byte[] Title { get; set; } = { 01, 02, 03, 04, 05, 06, 07 };

    public long? Parent_id { get; set; } = null;
    public EncryptedDirectory? Parent { get; set; }

    public long Owner_id { get; set; } = 1;
    public EncryptedUser? Owner { get; set; }

    public List<EncryptedAccount> Accounts { get; set; } = new();

    public EncryptedDirectoryBuilder SetId(long id)
    {
        Id = id;
        return this;
    }
    
    public EncryptedDirectoryBuilder SetTitle(byte[] title)
    {
        Title = title;
        return this;
    }

    public EncryptedDirectoryBuilder SetParentId(long parentId)
    {
        Parent_id = parentId;
        return this;
    }

    public EncryptedDirectoryBuilder SetParent(EncryptedDirectory parent)
    {
        Parent_id = parent.Id;
        Parent = parent;
        return this;
    }
    
    public EncryptedDirectoryBuilder SetOwnerId(long ownerId)
    {
        Parent_id = ownerId;
        return this;
    }

    public EncryptedDirectoryBuilder SetOwner(EncryptedUser owner)
    {
        Owner_id = owner.Id!.Value;
        Owner = owner;
        return this;
    }
    
    public EncryptedDirectory Build()
    {
        return new EncryptedDirectory
        {
            Id = Id,
            Title = Title,
            Parent_id = Parent_id,
            Parent = Parent,
            Owner_id = Owner_id,
            Owner = Owner,
            Accounts = Accounts
        };
    }
}