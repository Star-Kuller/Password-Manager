using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PasswordManager.Application.Interfaces;
using PasswordManager.Application.Interfaces.Database;
using PasswordManager.Application.Interfaces.Database.Repositories;
using PasswordManager.Infrastructure.Database.Repositories;

namespace PasswordManager.Infrastructure.Database;

public class DbUnitOfWork : IDbUnitOfWork
{
    private readonly ICryptographer _cryptographer;
    private readonly IDbConnection _connection;
    private IDbTransaction _transaction;

    public DbUnitOfWork(IConfiguration configuration, ICryptographer cryptographer)
    {
        _cryptographer = cryptographer;
        
        var connectionString = configuration.GetConnectionString("MainDbConnection");
        _connection = new NpgsqlConnection(connectionString);
        _connection.Open();
        _transaction = _connection.BeginTransaction();
    }
    
    public IUserRepository UserRepository =>  new UserRepository(_connection, _cryptographer, _transaction);

    public void BeginNewTransaction()
    {
        _transaction = _connection.BeginTransaction();
    }
    
    /// <summary>
    /// После завершения транзакции репозитории нужно получать повторно
    /// </summary>
    public void Commit()
    {
        try
        {
            _transaction.Commit();
        }
        catch
        {
            RollBack();
            throw;
        }
    }

    public void RollBack()
    {
        _transaction.Rollback();
    }

    public void Dispose()
    {
        _connection.Dispose();
        _transaction.Dispose();
    }
}