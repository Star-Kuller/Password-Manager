using PasswordManager.API.Middlewares.Exceptions.Handlers;

namespace PasswordManager.API.Middlewares.Exceptions;

public class ExceptionsHandlerMiddleware
{
    private readonly IExceptionHandler _chainOfHandlers;
    private readonly RequestDelegate _next;
    public ExceptionsHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
        
        var repositoryHandler = new RepositoryExceptionHandler(null);
        var validationHandler = new ValidationExceptionHandler(repositoryHandler);
        _chainOfHandlers = validationHandler;
    }
    
    public async Task Invoke(HttpContext context, ILogger<ExceptionsHandlerMiddleware> logger, IResponseWriter responseWriter)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            responseWriter.HttpContext = context;
            await _chainOfHandlers.HandleAsync(responseWriter, ex, logger);
        }
    }
}