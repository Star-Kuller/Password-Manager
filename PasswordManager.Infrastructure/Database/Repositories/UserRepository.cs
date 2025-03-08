using System.Data;
using Dapper;
using PasswordManager.Application.Exceptions;
using PasswordManager.Application.Interfaces;
using PasswordManager.Application.Interfaces.Database.Repositories;
using PasswordManager.Domain.Entities;

namespace PasswordManager.Infrastructure.Database.Repositories;

public class UserRepository(IDbConnection dbConnection,
    ICryptographer cryptographer, IDbTransaction transaction) : IUserRepository
{
    public async Task<long> AddAsync(User user)
    {
        const string sql = """
            INSERT INTO Users (Email, PasswordHash, SecretKey, EmailConfirmed)
            VALUES (@Email, @PasswordHash, @SecretKey, @EmailConfirmed)
            ON CONFLICT (Email) DO NOTHING
            RETURNING Id
            """;

        var result = await dbConnection.QuerySingleAsync<long?>(sql,
            new EncryptedUser(user, cryptographer), transaction);
        
        if (result == null)
        {
            throw new AlreadyExistException($"Пользователь с почтой {user.Email} уже зарегистрирован");
        }

        return result.Value;
    }

    public async Task UpdateAsync(User user)
    {
        const string sql = """
            UPDATE Users
            SET PasswordHash = @PasswordHash, SecretKey = @SecretKey, EmailConfirmed = @EmailConfirmed
            WHERE Id = @Id
            """;
        
        await dbConnection.ExecuteAsync(sql, 
            new EncryptedUser(user, cryptographer), transaction);
    }

    public async Task<User?> GetAsync(long id)
    {
        const string sql = """
            SELECT Id, Email, PasswordHash, SecretKey, EmailConfirmed
            FROM Users
            WHERE Id = @Id
            """;
        
        var result = await dbConnection.QuerySingleOrDefaultAsync<EncryptedUser>(sql,
            new { Id = id }, transaction);

        if (result is null)
            throw new NotFoundException("Пользователь", "ID", id);
        
        return result.ToUser(cryptographer);
    }

    public async Task<User?> GetAsync(string email)
    {
        const string sql = """
                           SELECT Id, Email, PasswordHash, SecretKey, EmailConfirmed
                           FROM Users
                           WHERE Email = @Email
                           """;
        
        var result = await dbConnection.QuerySingleOrDefaultAsync<EncryptedUser>(sql,
            new { Email = cryptographer.Encrypt(email) }, transaction);
        
        return result?.ToUser(cryptographer);
    }
    
    private class EncryptedUser()
    {
        public long? Id { get; init; }
        public byte[] Email { get; init; }
        public bool EmailConfirmed { get; init; }
        public byte[] PasswordHash { get; init; }
        public byte[] SecretKey { get; init; }

        public EncryptedUser(User user, ICryptographer cryptographer) : this()
        {
            Id = user.Id;
            Email = cryptographer.Encrypt(user.Email);
            EmailConfirmed = user.EmailConfirmed;
            PasswordHash = cryptographer.Encrypt(user.PasswordHash);
            SecretKey = cryptographer.Encrypt(user.SecretKey);
        }

        public User ToUser(ICryptographer cryptographer)
        {
            return new User
            {
                Id = Id,
                Email = cryptographer.Decrypt(Email),
                EmailConfirmed = EmailConfirmed,
                PasswordHash = cryptographer.Decrypt(PasswordHash),
                SecretKey = cryptographer.Decrypt(SecretKey)
            };
        }
    }
}