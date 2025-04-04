using PasswordManager.Application.Models.Encrypted;

namespace PasswordManager.Application.Interfaces.Database.Repositories;

public interface IUserRepository : ICrudRepository<EncryptedUser>
{
    Task<EncryptedUser?> GetAsync(byte[] email);
}