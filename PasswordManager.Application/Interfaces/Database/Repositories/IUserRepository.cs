using PasswordManager.Application.Models.Encrypted;

namespace PasswordManager.Application.Interfaces.Database.Repositories;

public interface IUserRepository
{
    Task<long> AddAsync(EncryptedUser user);
    Task UpdateAsync(EncryptedUser user);
    Task<EncryptedUser?> GetAsync(long id);
    Task<EncryptedUser?> GetAsync(byte[] email);
}