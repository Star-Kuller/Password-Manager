namespace PasswordManager.API.Middlewares.Exceptions.Handlers;

public abstract class ExceptionHandlerBase
{
    public abstract Task HandleAsync(HttpContext context, Exception exception, ILogger logger);
}