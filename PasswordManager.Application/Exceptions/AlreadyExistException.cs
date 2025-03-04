namespace PasswordManager.Application.Exceptions;

public class AlreadyExistException(string message) : Exception(message);