using PasswordManager.Application.Interfaces.Database;

namespace PasswordManager.Infrastructure.Database.Infrastructure;

public class UnitOfWorkFactory(DbConnectionFactory connectionFactory) : IUnitOfWorkFactory
{
    public async Task<IUnitOfWork> CreateAsync(CancellationToken token)
    {
        var connection = await connectionFactory.OpenAsync(token);
        var transaction = await connection.BeginTransactionAsync(token);
        return new UnitOfWork(connection, transaction);
    }
}