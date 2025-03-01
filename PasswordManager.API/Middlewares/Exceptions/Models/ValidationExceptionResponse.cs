namespace PasswordManager.API.Middlewares.Exceptions.Models;

public record ValidationExceptionResponse(string Property, List<string> Messages);