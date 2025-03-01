using System.Net;
using FluentValidation;
using PasswordManager.API.Middlewares.Exceptions.Handlers;
using PasswordManager.Application.Exceptions;

namespace PasswordManager.API.Middlewares.Exceptions;

public class ExceptionsHandler(RequestDelegate next)
{
    private readonly Dictionary<Type, ExceptionHandlerBase> _handlers = new()
    {
        {typeof(ValidationException), new ValidationExceptionHandler()},
        {typeof(RepositoryException), new RepositoryExceptionHandler()}
    };
    public async Task Invoke(HttpContext context, ILogger<ExceptionsHandler> logger)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            if (_handlers.TryGetValue(typeof(Exception), out var handler))
            {
                await handler.HandleAsync(context, ex, logger);
            }
            else
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                logger.LogError(ex, "Ошибка базы данных");
            }
        }
    }
}