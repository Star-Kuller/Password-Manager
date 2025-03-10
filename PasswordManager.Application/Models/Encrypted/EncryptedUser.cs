using PasswordManager.Application.Interfaces;
using PasswordManager.Domain.Entities;

namespace PasswordManager.Application.Models.Encrypted;

public class EncryptedUser()
{
    public long? Id { get; init; }
    public byte[] Login { get; init; }
    public byte[] PasswordHash { get; init; }
    public byte[] SecretKey { get; init; }

    public EncryptedUser(User user, ICryptographer cryptographer) : this()
    {
        Id = user.Id;
        Login = cryptographer.Encrypt(user.Login);
        
    }

    public User ToUser(ICryptographer cryptographer)
    {
        return new User
        {
            Id = Id,
            Login = cryptographer.Decrypt(Login),
            PasswordHash = cryptographer.Decrypt(PasswordHash),
            SecretKey = cryptographer.Decrypt(SecretKey)
        };
    }
}