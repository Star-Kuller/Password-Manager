using PasswordManager.Application.Interfaces.Database.Repositories;

namespace PasswordManager.Application.Interfaces.Database;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IUserRepository Users { get; }
    
    Task CommitAsync(CancellationToken cancellationToken);
    Task RollBackAsync(CancellationToken cancellationToken);
}