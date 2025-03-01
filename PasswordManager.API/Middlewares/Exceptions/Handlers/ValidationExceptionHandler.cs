using System.Net;
using FluentValidation;
using PasswordManager.API.Middlewares.Exceptions.Models;

namespace PasswordManager.API.Middlewares.Exceptions.Handlers;

public class ValidationExceptionHandler : ExceptionHandlerBase
{
    public override async Task HandleAsync(HttpContext context, Exception exception, ILogger logger)
    {
        var validationEx = exception as ValidationException;
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        var propertyErrors = new Dictionary<string, List<string>>();
        foreach (var error in validationEx!.Errors)
        {
            propertyErrors.TryAdd(error.PropertyName, []);
            propertyErrors[error.PropertyName].Add(error.ErrorMessage);
        }
        var response = propertyErrors
            .Select(e => new ValidationExceptionResponse(e.Key, e.Value));
        logger.LogWarning(exception, "Ошибка валидации");
        await context.Response.WriteAsJsonAsync(response);
    }
}