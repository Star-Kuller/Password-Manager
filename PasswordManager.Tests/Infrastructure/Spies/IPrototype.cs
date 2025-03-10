namespace PasswordManager.Tests.Infrastructure.Spies;

public interface IPrototype<out T>
{
    public T Clone();
}