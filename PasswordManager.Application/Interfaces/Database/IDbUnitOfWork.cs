using PasswordManager.Application.Interfaces.Database.Repositories;

namespace PasswordManager.Application.Interfaces.Database;

public interface IDbUnitOfWork : IDisposable
{
    IUserRepository UserRepository { get; }

    void BeginNewTransaction();
    void Commit();
    void RollBack();
}