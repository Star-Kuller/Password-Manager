using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PasswordManager.Infrastructure.Database;

public static class MigrationRunner
{
    public static void RunMigrations(string connectionString, ILogger logger)
    {
        if(string.IsNullOrWhiteSpace(connectionString))
        {
            logger.LogWarning("connectionString пуста, миграции не применены");
            return;
        };
        try
        {
            var serviceProvider = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(MigrationRunner).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
        catch
        {
            logger.LogError("Ошибка применения миграций");
            throw;
        }
        
    }
}