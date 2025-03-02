using System.Net;

namespace PasswordManager.API.Middlewares.Exceptions.Handlers;

public abstract class ExceptionHandlerBase<T>(IExceptionHandler? nextHandler) : IExceptionHandler
    where T : Exception
{
    protected abstract Task HandleAsync(IResponseWriter responseWriter, T exception, ILogger logger);

    public async Task HandleAsync(IResponseWriter responseWriter, Exception exception, ILogger logger)
    {
        if (exception is T tException)
            await HandleAsync(responseWriter, tException, logger);
        else if (nextHandler is not null)
            await nextHandler.HandleAsync(responseWriter, exception, logger);
        else
        {
            logger.LogCritical(exception, "Необработанная ошибка");
            await responseWriter.WriteAsync(HttpStatusCode.InternalServerError);
        }
    }
}