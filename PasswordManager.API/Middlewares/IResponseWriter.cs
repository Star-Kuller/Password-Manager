using System.Net;

namespace PasswordManager.API.Middlewares;

public interface IResponseWriter
{
    HttpContext HttpContext { get; set; }
    
    Task WriteAsync<T>(HttpStatusCode statusCode, T body)
        where T : class;
    
    Task WriteAsync(HttpStatusCode statusCode);
}