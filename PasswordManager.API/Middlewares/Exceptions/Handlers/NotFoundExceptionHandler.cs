using System.Net;
using PasswordManager.Application.Exceptions;

namespace PasswordManager.API.Middlewares.Exceptions.Handlers;

public class NotFoundExceptionHandler(IExceptionHandler? nextHandler) : ExceptionHandlerBase<NotFoundException>(nextHandler)
{
    protected override async Task HandleAsync(IResponseWriter responseWriter, NotFoundException exception, ILogger logger)
    {
        var response = new Response(exception);
        await responseWriter.WriteAsync(HttpStatusCode.NotFound, response);
    }
    
    public class Response(string message)
    {
        public string Message { get; set; } = message;
        
        public Response(NotFoundException exception) 
            : this(exception.Message)
        {
            PropertyName = exception.PropertyName;
            Name = exception.Name;
            Key = exception.Key?.ToString();
        }
        public string? PropertyName { get; set; }
        public string? Name { get; set; }
        public string? Key { get; set; }
    }
}