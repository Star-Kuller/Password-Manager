namespace PasswordManager.API.Middlewares.Exceptions.Handlers;

public interface IExceptionHandler
{
    Task HandleAsync(IResponseWriter responseWriter, Exception exception, ILogger logger);
}