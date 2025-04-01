using PasswordManager.Application.Interfaces;
using PasswordManager.Domain.Entities;

// ReSharper disable InconsistentNaming

namespace PasswordManager.Application.Models.Encrypted;

public class EncryptedUser()
{
    public long? Id { get; init; }
    public byte[] Login { get; init; }
    public byte[] Password_hash { get; init; }
    public byte[] Secret_key { get; init; }
    
    public long Root_directory_id { get; init; }
    public EncryptedDirectory RootDirectory { get; set; }

    public EncryptedUser(User user, ICryptographer cryptographer) : this()
    {
        Id = user.Id;
        Login = cryptographer.Encrypt(user.Login);
        Password_hash = cryptographer.Encrypt(user.PasswordHash);
        Secret_key = cryptographer.Encrypt(user.SecretKey);
        Root_directory_id = user.RootDirectoryId;
    }

    public User ToEntity(ICryptographer cryptographer)
    {
        return new User
        {
            Id = Id,
            Login = cryptographer.Decrypt(Login),
            PasswordHash = cryptographer.Decrypt(Password_hash),
            SecretKey = cryptographer.Decrypt(Secret_key),
            RootDirectoryId = Root_directory_id,
            RootDirectory = RootDirectory?.ToEntity(cryptographer)
        };
    }
}