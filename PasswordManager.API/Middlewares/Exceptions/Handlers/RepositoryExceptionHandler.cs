using System.Net;
using PasswordManager.Application.Exceptions;

namespace PasswordManager.API.Middlewares.Exceptions.Handlers;

public class RepositoryExceptionHandler(IExceptionHandler? next) : ExceptionHandlerBase<RepositoryException>(next)
{
    protected override async Task HandleAsync(IResponseWriter responseWriter, RepositoryException exception, ILogger logger)
    {
        logger.LogError(exception, "Ошибка базы данных");
        await responseWriter.WriteAsync(HttpStatusCode.InternalServerError);
    }
}