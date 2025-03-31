using System.Data;
using Dapper;
using PasswordManager.Application.Exceptions;
using PasswordManager.Application.Interfaces.Database.Repositories;
using PasswordManager.Application.Models.Encrypted;

namespace PasswordManager.Infrastructure.Database.Repositories;

public class UserRepository(IDbConnection connection, IDbTransaction transaction) : IUserRepository
{
    public async Task<long> AddAsync(EncryptedUser user)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        const string sql = """
            INSERT INTO users (login, password_hash, secret_key)
            VALUES (@Login, @Password_hash, @Secret_key)
            ON CONFLICT (login) DO NOTHING
            RETURNING Id
            """;
        var result = await connection.QuerySingleOrDefaultAsync<long?>(
            sql, user, transaction);
        
        if (result == null)
            throw new AlreadyExistException("Пользователь с такой почтой уже существует");

        return result.Value;
    }

    public async Task UpdateAsync(EncryptedUser user)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        const string sql = """
            UPDATE Users
            SET password_hash = @Password_hash, secret_key = @Secret_key
            WHERE Id = @Id
            """;
        
        await connection.ExecuteAsync(sql, user, transaction);
    }

    public async Task<EncryptedUser?> GetAsync(long id)
    {
        const string sql = """
            SELECT id, login, password_hash, secret_key
            FROM Users
            WHERE id = @Id
            LIMIT 1
            """;
        
        var result = await connection.QuerySingleOrDefaultAsync<EncryptedUser>(
            sql, new { Id = id }, transaction);
        
        return result;
    }

    public async Task<EncryptedUser?> GetAsync(byte[] login)
    {
        ArgumentNullException.ThrowIfNull(login);
        
        const string sql = """
                           SELECT id, login, password_hash, secret_key
                           FROM Users
                           WHERE login = @Login
                           LIMIT 1
                           """;
        
        var result = await connection.QuerySingleOrDefaultAsync<EncryptedUser>(
            sql, new { Login = login }, transaction);

        return result;
    }
}