using PasswordManager.Tests.Infrastructure;

namespace PasswordManager.Tests.IntegrationTests;

[Collection("IntegrationTests")]
public class IntegrationTestBase()
{
    protected readonly WebAppFactory AppFactory = new();
}