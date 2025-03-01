using System.Net;
using FluentValidation;
using PasswordManager.API.ExceptionResponses;
using PasswordManager.Application.Exceptions;

namespace PasswordManager.API.Middlewares;

public class ExceptionsHandler(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, ILogger<ExceptionsHandler> logger)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, logger);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger logger)
    {
        context.Response.ContentType = "application/json";
        switch (exception)
        {
            case ValidationException validationEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var propertyErrors = new Dictionary<string, List<string>>();
                foreach (var error in validationEx.Errors)
                {
                    propertyErrors.TryAdd(error.PropertyName, []);
                    propertyErrors[error.PropertyName].Add(error.ErrorMessage);
                }
                var response = propertyErrors
                    .Select(e => new ValidationExceptionResponse(e.Key, e.Value));
                logger.LogWarning(exception, "Ошибка валидации");
                await context.Response.WriteAsJsonAsync(response);
                break;
            case RepositoryException repositoryEx:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                logger.LogError(exception, "Ошибка базы данных");
                break;
            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                logger.LogCritical(exception, "Необработанная ошибка");
                break;
        }
    }
}