using PasswordManager.Application.Interfaces;
using PasswordManager.Domain.Entities;

namespace PasswordManager.Application.Models.Encrypted;

public class EncryptedAccount() : EncryptedEntity<Account>()
{
    public byte[] Title { get; set; }
    public byte[] Url { get; set; }
    public byte[] Login { get; set; }
    public byte[] Password { get; set; }
    
    public long DirectoryId { get; set; }
    public EncryptedDirectory? Directory { get; set; }
    
    public EncryptedAccount(Account account, ICryptographer cryptographer) : this()
    {
        Id = account.Id;
        Title = cryptographer.Encrypt(account.Title);
        Url = cryptographer.Encrypt(account.Url);
        Login = cryptographer.Encrypt(account.Login);
        Password = cryptographer.Encrypt(account.Password);
        DirectoryId = account.DirectoryId;
        if(account.Directory is not null) 
            Directory = new EncryptedDirectory(account.Directory, cryptographer);
    }
    
    public override Account ToEntity(ICryptographer cryptographer)
    {
        return new Account
        {
            Id = Id,
            Title = cryptographer.Decrypt(Title),
            Url = cryptographer.Decrypt(Url),
            Login = cryptographer.Decrypt(Login),
            Password = cryptographer.Decrypt(Password),
            DirectoryId = DirectoryId,
            Directory = Directory?.ToEntity(cryptographer)
        };
    }
}