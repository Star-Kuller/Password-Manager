using PasswordManager.API.Middlewares.Exceptions.Handlers;

namespace PasswordManager.API.Middlewares.Exceptions;

public class ExceptionsHandlerMiddleware
{
    private readonly IExceptionHandler _chainOfHandlers;
    private readonly RequestDelegate _next;
    public ExceptionsHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
        
        var validationHandler = new ValidationExceptionHandler(null);
        var alreadyExistHandler = new AlreadyExistExceptionHandler(validationHandler);
        var notFoundHandler = new NotFoundExceptionHandler(alreadyExistHandler);
        _chainOfHandlers = notFoundHandler;
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