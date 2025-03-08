using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using PasswordManager.API.Middlewares;
using PasswordManager.API.Middlewares.Exceptions;
using PasswordManager.Application.Handlers.Authentication;
using PasswordManager.Application.Interfaces;
using PasswordManager.Application.Interfaces.Database;
using PasswordManager.Application.Interfaces.Database.Repositories;
using PasswordManager.Infrastructure.Database;
using PasswordManager.Infrastructure.Database.Repositories;

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
builder.Services.AddTransient<IDbUnitOfWork, DbUnitOfWork>();

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