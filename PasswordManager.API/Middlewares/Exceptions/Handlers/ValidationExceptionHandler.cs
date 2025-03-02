using System.Net;
using FluentValidation;
using PasswordManager.API.Middlewares.Exceptions.Models;

namespace PasswordManager.API.Middlewares.Exceptions.Handlers;

public class ValidationExceptionHandler(IExceptionHandler? next) : ExceptionHandlerBase<ValidationException>(next)
{
    protected override async Task HandleAsync(IResponseWriter responseWriter, ValidationException exception, ILogger logger)
    {
        var propertyErrors = new Dictionary<string, List<string>>();
        foreach (var error in exception.Errors)
        {
            propertyErrors.TryAdd(error.PropertyName, []);
            propertyErrors[error.PropertyName].Add(error.ErrorMessage);
        }
        var response = propertyErrors
            .Select(e => new ValidationExceptionResponse(e.Key, e.Value));
        await responseWriter.WriteAsync(HttpStatusCode.BadRequest, response);
    }
}