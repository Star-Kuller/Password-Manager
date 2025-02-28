using FastEndpoints;
using FastEndpoints.Swagger;
using PasswordManager.API.Middlewares;
using PasswordManager.Application.Handlers.Authentication;
using PasswordManager.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Register).Assembly));


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseFastEndpoints();
app.UseSwaggerGen();

app.UseMiddleware<ExceptionsHandler>();
app.UseHttpsRedirection();

app.Run();

var logger = app.Services.GetRequiredService<ILogger>();
MigrationRunner.RunMigrations("", logger);