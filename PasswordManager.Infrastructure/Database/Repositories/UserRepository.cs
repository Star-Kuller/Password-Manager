using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PasswordManager.Application.Interfaces;
using PasswordManager.Domain.Entities;

namespace PasswordManager.Infrastructure.Database.Repositories;

public class UserRepository(ICryptographer cryptographer, IConfiguration configuration) : IUserRepository
{
    private readonly string? _connectionString = configuration.GetConnectionString("MainDbConnection");
    public async Task<long> AddAsync(User user)
    {
        const string sql = """
            INSERT INTO Users (Email, PasswordHash, SecretKey, EmailConfirmed)
            VALUES (@Email, @PasswordHash, @SecretKey, @EmailConfirmed)
            RETURNING Id
            """;

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        return await connection.QuerySingleAsync<long>(sql, new
        {
            Email = cryptographer.Encrypt(user.Email),
            PasswordHash = cryptographer.Encrypt(user.PasswordHash),
            SecretKey = cryptographer.Encrypt(user.SecretKey),
            EmailConfirmed = user.EmailConfirmed
        });
    }

    public async Task UpdateAsync(User user)
    {
        const string sql = """
            UPDATE Users
            SET PasswordHash = @PasswordHash, SecretKey = @SecretKey, EmailConfirmed = @EmailConfirmed
            WHERE Id = @Id
            """;

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, new
        {
            Id = user.Id,
            PasswordHash = cryptographer.Encrypt(user.PasswordHash),
            SecretKey = cryptographer.Encrypt(user.SecretKey),
            EmailConfirmed = user.EmailConfirmed
        });
    }

    public async Task<User?> GetAsync(long id)
    {
        const string sql = """
            SELECT Id, Email, PasswordHash, SecretKey, EmailConfirmed
            FROM Users
            WHERE Id = @Id
            """;

        await using var connection = new NpgsqlConnection(_connectionString);
        var result = await connection.QuerySingleOrDefaultAsync<EncryptedUser>(sql, new { Id = id });
        
        return result?.ToUser(cryptographer);
    }

    public async Task<User?> GetAsync(string email)
    {
        const string sql = """
                           SELECT Id, Email, PasswordHash, SecretKey, EmailConfirmed
                           FROM Users
                           WHERE Email = @Email
                           """;

        await using var connection = new NpgsqlConnection(_connectionString);
        var result = await connection.QuerySingleOrDefaultAsync<EncryptedUser>(sql, new { Email = email });
        
        return result?.ToUser(cryptographer);
    }
    
    private class EncryptedUser
    {
        public long Id { get; init; }
        public byte[] Email { get; init; }
        public bool EmailConfirmed { get; init; }
        public byte[] PasswordHash { get; init; }
        public byte[] SecretKey { get; init; }

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