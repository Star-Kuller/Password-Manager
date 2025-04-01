using PasswordManager.Application.Models.Encrypted;

namespace PasswordManager.Application.Interfaces.Database.Repositories;

public interface IDirectoryRepository : ICrudRepository<EncryptedDirectory>
{
}