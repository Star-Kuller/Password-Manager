using System.Net.Http.Json;
using FluentAssertions;
using PasswordManager.Application.Handlers.Authentication;
using PasswordManager.Tests.Infrastructure;

namespace PasswordManager.Tests.IntegrationTests;

public class AuthenticationTests(WebAppFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task Register_and_login_is_successful()
    {
        await AppFactory.ResetDatabase();
        // Arrange:/register
        var client = AppFactory.CreateClient();
        var registerReq = JsonContent.Create(
            new Registration.Request("encrypted_secret_key", "test@email.com", "password_hash"));
        
        // Act:/register
        var registerRes = await client.PostAsync("/register",registerReq);
        
        // Assert:/register
        registerRes.IsSuccessStatusCode.Should().BeTrue();
        
        // Arrange:/login
        // Act:/login
        // Assert:/login
    }
}