using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PasswordManager.API;

namespace PasswordManager.Tests.Infrastructure;

public class WebAppFactory : WebApplicationFactory<Program>
{
    private string _connectionString;
    public WebAppFactory()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.IntegrationTests.json")
            .Build();
        _connectionString = configuration.GetConnectionString("MainDbConnection")!;
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTests");
        builder.ConfigureTestServices(services =>
        {
            //Тут место для моков внешних сервисов
        });
    }
    
    public async Task ResetDatabase()
    {
        if (string.IsNullOrWhiteSpace(_connectionString))
        {
            throw new InvalidOperationException("Connection string is not configured.");
        }

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        
        var disableConstraintsCommand = new NpgsqlCommand("SET CONSTRAINTS ALL DEFERRED;", connection);
        await disableConstraintsCommand.ExecuteNonQueryAsync();
        
        var getTablesCommand = new NpgsqlCommand(
            """
            SELECT table_name 
            FROM information_schema.tables
            WHERE table_schema = 'public' AND table_type = 'BASE TABLE'
            """, connection);

        await using var reader = await getTablesCommand.ExecuteReaderAsync();
        var tables = new List<string>();
        while (reader.Read())
        {
            tables.Add(reader.GetString(0));
        }

        await reader.CloseAsync();

        foreach (var table in tables)
        {
            var dropTableCommand = new NpgsqlCommand($"DROP TABLE IF EXISTS \"{table}\" CASCADE", connection);
            await dropTableCommand.ExecuteNonQueryAsync();
        }
    }
}