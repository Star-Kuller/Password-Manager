namespace PasswordManager.Application.Exceptions;

public class NotFoundException(string message) : Exception(message)
{
    public NotFoundException(string? name, string? propertyName, object? key, string message)
        : this(message)
    {
        PropertyName = propertyName;
        Name = name;
        Key = key;
    }
    public NotFoundException(string name, string propertyName, object key)
        : this(name, propertyName, key, $"{name} с {propertyName} = {key} не был найден")
    { }
        
    public string? PropertyName { get; private set; }
    public string? Name { get; private set; }
    public object? Key { get; private set; }
};