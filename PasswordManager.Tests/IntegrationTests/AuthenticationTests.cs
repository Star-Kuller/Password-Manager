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

        await TestRegister("encrypted_secret_key", "test@email.com", "password_hash");
        await TestLogin("test@email.com", "password_hash");
    }

    private async Task TestRegister(string secret, string login, string password)
    {
        // Arrange
        var client = AppFactory.CreateClient();
        var request = JsonContent.Create(
            new Registration.Request(secret, login, password));
        
        // Act
        var response = await client.PostAsync("/register",request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var setCookieHeader = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
        setCookieHeader.Should().NotBeNull("Куки сессии должны быть установлены");
        var cookieParts = setCookieHeader.Split(';');
        cookieParts.Should().Contain(part => part.ToLower().Contains("httponly"), "Куки должны быть HttpOnly");
        cookieParts.Should().Contain(part => part.Trim().Contains("path=/"), "Куки должны быть доступны на всех путях");
    }
    
    private async Task TestLogin(string login, string password)
    {
        // Arrange
        var client = AppFactory.CreateClient();
        var request = JsonContent.Create(
            new Login.Request(login, password));
        
        // Act
        var response = await client.PostAsync("/register",request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var setCookieHeader = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
        setCookieHeader.Should().NotBeNull("Куки сессии должны быть установлены");
        var cookieParts = setCookieHeader.Split(';');
        cookieParts.Should().Contain(part => part.ToLower().Contains("httponly"), "Куки должны быть HttpOnly");
        cookieParts.Should().Contain(part => part.Trim().Contains("path=/"), "Куки должны быть доступны на всех путях");
    }
}