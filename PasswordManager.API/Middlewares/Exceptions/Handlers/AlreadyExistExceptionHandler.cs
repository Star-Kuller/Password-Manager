using System.Net;
using PasswordManager.Application.Exceptions;

namespace PasswordManager.API.Middlewares.Exceptions.Handlers;

public class AlreadyExistExceptionHandler(IExceptionHandler? next) : ExceptionHandlerBase<AlreadyExistException>(next)
{
    protected override async Task HandleAsync(IResponseWriter responseWriter, AlreadyExistException exception, ILogger logger)
    {
        var response = new Response(exception.Message);
        await responseWriter.WriteAsync(HttpStatusCode.BadRequest, response);
    }

    public record Response(string message);
}