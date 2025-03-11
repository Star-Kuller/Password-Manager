using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using PasswordManager.API.Endpoints.Authentication;
using PasswordManager.API.Middlewares;
using PasswordManager.API.Middlewares.Exceptions;
using PasswordManager.Application.Handlers.Authentication;
using PasswordManager.Application.Interfaces;
using PasswordManager.Application.Interfaces.Database;
using PasswordManager.Infrastructure.Database;
using PasswordManager.Infrastructure.Database.Infrastructure;

namespace PasswordManager.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services
            .AddAuthenticationCookie(validFor: TimeSpan.FromMinutes(30),
                opt => opt.SlidingExpiration = true)
            .AddAuthorization()
            .AddFastEndpoints();

        builder.Services.SwaggerDocument();
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Registration).Assembly));
        var connectionString = builder.Configuration.GetConnectionString("MainDbConnection");

        builder.Services.AddTransient<IResponseWriter, ResponseWriter>();
        builder.Services.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();
        builder.Services.AddTransient<ISessionManager, SessionManager>();
        builder.Services.AddTransient<ICryptographer, AesCryptographer>();
        builder.Services.AddTransient<DbConnectionFactory>();

        var app = builder.Build();

        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        MigrationRunner.RunMigrations(connectionString, logger);

        // Configure the HTTP request pipeline.
        app.UseAuthentication()
            .UseAuthorization()
            .UseFastEndpoints();

        app.UseSwaggerGen();

        app.UseMiddleware<ExceptionsHandlerMiddleware>();
        app.UseHttpsRedirection();

        app.Run();
    }
}