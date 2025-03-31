using PasswordManager.Application.Models.Encrypted;

namespace PasswordManager.Application.Interfaces.Database.Repositories;

public interface IAccountRepository
{
    Task<long> AddAsync(EncryptedAccount account);
    Task UpdateAsync(EncryptedAccount account);
    Task<EncryptedAccount?> GetAsync(long id);
}