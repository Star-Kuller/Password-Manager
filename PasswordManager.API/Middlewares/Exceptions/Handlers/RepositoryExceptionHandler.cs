using System.Net;

namespace PasswordManager.API.Middlewares.Exceptions.Handlers;

public class RepositoryExceptionHandler : ExceptionHandlerBase
{
    public override async Task HandleAsync(HttpContext context, Exception exception, ILogger logger)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        logger.LogError(exception, "Ошибка базы данных");
    }
}