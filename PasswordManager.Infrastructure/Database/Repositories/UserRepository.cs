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
            VALUES (@Login, @PasswordHash, @SecretKey)
            ON CONFLICT (login) DO NOTHING
            RETURNING Id
            """;
        var result = await connection.QuerySingleAsync<long?>(
            sql, user, transaction);
        
        if (result == null)
            throw new AlreadyExistException($"Пользователь с такой почтой уже существует");

        return result.Value;
    }

    public async Task UpdateAsync(EncryptedUser user)
    {
        const string sql = """
            UPDATE Users
            SET PasswordHash = @PasswordHash, SecretKey = @SecretKey
            WHERE Id = @Id
            """;
        
        await connection.ExecuteAsync(sql, user, transaction);
    }

    public async Task<EncryptedUser?> GetAsync(long id)
    {
        const string sql = """
            SELECT Id, Login, PasswordHash, SecretKey
            FROM Users
            WHERE Id = @Id
            LIMIT 1
            """;
        
        var result = await connection.QuerySingleOrDefaultAsync<EncryptedUser>(
            sql, new { Id = id }, transaction);
        
        return result;
    }

    public async Task<EncryptedUser?> GetAsync(byte[] login)
    {
        const string sql = """
                           SELECT Id, Login, PasswordHash, SecretKey
                           FROM Users
                           WHERE Email = @Email
                           LIMIT 1
                           """;
        
        var result = await connection.QuerySingleOrDefaultAsync<EncryptedUser>(
            sql, new { Login = login }, transaction);

        return result;
    }
}