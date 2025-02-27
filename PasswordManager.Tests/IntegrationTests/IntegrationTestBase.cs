using PasswordManager.Tests.Infrastructure;

namespace PasswordManager.Tests.IntegrationTests;

public class IntegrationTestBase(WebAppFactory factory) : IClassFixture<WebAppFactory>
{
    protected readonly WebAppFactory Factory = factory;
}