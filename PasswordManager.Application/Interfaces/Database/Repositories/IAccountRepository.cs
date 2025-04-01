using PasswordManager.Application.Models.Encrypted;

namespace PasswordManager.Application.Interfaces.Database.Repositories;

public interface IAccountRepository : ICrudRepository<EncryptedAccount>
{
}