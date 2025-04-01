using Npgsql;
using PasswordManager.Application.Interfaces.Database;
using PasswordManager.Application.Interfaces.Database.Repositories;
using PasswordManager.Infrastructure.Database.Repositories;

namespace PasswordManager.Infrastructure.Database.Infrastructure;

public class UnitOfWork(NpgsqlConnection connection, NpgsqlTransaction transaction) : IUnitOfWork
{
    private bool _commited;
    
    public IUserRepository Users =>  new UserRepository(connection, transaction);
    public IDirectoryRepository Directories { get; }
    public IAccountRepository Accounts { get; }

    public async Task CommitAsync(CancellationToken token)
    {
        if (_commited)
        {
            throw new InvalidOperationException("Already committed");
        }
        _commited = true;
        await transaction.CommitAsync(token);
    }

    public async Task RollBackAsync(CancellationToken token)
    {
        await transaction.RollbackAsync(token);
        transaction = await connection.BeginTransactionAsync(token);
    }
    
    public async ValueTask DisposeAsync()
    {
        await connection.DisposeAsync();
        await transaction.DisposeAsync();
    }

    public void Dispose()
    {
        connection.Dispose();
        transaction.Dispose();
    }
}