namespace PasswordManager.API.ExceptionResponses;

public record ValidationExceptionResponse(string Property, List<string> Messages);