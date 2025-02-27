using FastEndpoints;
using FastEndpoints.Swagger;
using PasswordManager.Application.Handlers;
using PasswordManager.Application.Handlers.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Register).Assembly));


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseFastEndpoints();
app.UseSwaggerGen();

app.UseHttpsRedirection();

app.Run();