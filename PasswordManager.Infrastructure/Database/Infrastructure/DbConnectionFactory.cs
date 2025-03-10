using Microsoft.Extensions.Configuration;
using Npgsql;

namespace PasswordManager.Infrastructure.Database.Infrastructure;

public class DbConnectionFactory(IConfiguration configuration)
{
    public async Task<NpgsqlConnection> OpenAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var connection = new NpgsqlConnection(configuration.GetConnectionString("MainDbConnection"));
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}