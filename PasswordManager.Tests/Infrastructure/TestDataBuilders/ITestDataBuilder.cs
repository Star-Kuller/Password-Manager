namespace PasswordManager.Tests.Infrastructure.TestDataBuilders;

public interface ITestDataBuilder<T>
{
    public T Build();
}