using System.Net;

namespace PasswordManager.API.Middlewares;

public class ResponseWriter : IResponseWriter
{
    public HttpContext HttpContext { get; set; }

    public async Task WriteAsync<T>(HttpStatusCode statusCode, T body) 
        where T : class
    {
        HttpContext.Response.ContentType = "application/json";
        HttpContext.Response.StatusCode = (int)statusCode;
        await HttpContext.Response.WriteAsJsonAsync(body);
    }

    public Task WriteAsync(HttpStatusCode statusCode)
    {
        HttpContext.Response.StatusCode = (int)statusCode;
        return Task.CompletedTask;
    }
}