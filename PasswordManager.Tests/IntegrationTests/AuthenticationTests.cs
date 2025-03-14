using System.Net.Http.Json;
using FastEndpoints;
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

        await Test_register_is_successful("encrypted_secret_key", "test@email.com", "password_hash");
        await Test_logout_is_successful();
        await Test_login_is_successful("test@email.com", "password_hash");
        await Test_logout_is_successful();
    }

    private async Task Test_register_is_successful(string secret, string login, string password)
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
    
    private async Task Test_login_is_successful(string login, string password)
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
    
    private async Task Test_logout_is_successful()
    {
        // Arrange
        var client = AppFactory.CreateClient();
        
        // Act
        var response = await client.PutAsync("/logout", null);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var setCookieHeader = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
        setCookieHeader.Should().NotBeNull("Куки сессии должны быть установлены");
        var cookieParts = setCookieHeader.Split(';');
        cookieParts.Should().Contain(part => part.ToLower().Contains("httponly"), "Куки должны быть HttpOnly");
        cookieParts.Should().Contain(part => part.Trim().Contains("path=/"), "Куки должны быть доступны на всех путях");
    }
}