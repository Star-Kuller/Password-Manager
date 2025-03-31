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
        var client = AppFactory.CreateClient();

        await Test_register_is_successful(client, "encrypted_secret_key", "test@email.com", "password_hash");
        await Test_logout_is_successful(client);
        await Test_login_is_successful(client, "test@email.com", "password_hash");
        await Test_logout_is_successful(client);
    }

    private async Task Test_register_is_successful(HttpClient client, string secret, string login, string password)
    {
        // Arrange
        var request = JsonContent.Create(
            new Registration.Request(secret, login, password));
        
        // Act
        var response = await client.PostAsync("/register",request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var setCookieHeader = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
        setCookieHeader.Should().NotBeNull("Куки сессии должны быть установлены");
        var cookieParts = setCookieHeader.Split(';');
        cookieParts.Should().Contain(part => part.Contains(".AspNetCore.Cookies="), "Куки должны содержать идентификатор сессии");
        cookieParts.Should().Contain(part => part.ToLower().Contains(" expires="), "Куки должны содержать дату истечения куки");
        cookieParts.Should().Contain(part => part.ToLower().Contains("httponly"), "Куки должны быть HttpOnly");
        cookieParts.Should().Contain(part => part.Trim().Contains("path=/"), "Куки должны быть доступны на всех путях");
    }
    
    private async Task Test_login_is_successful(HttpClient client, string login, string password)
    {
        // Arrange
        var request = JsonContent.Create(
            new Login.Request(login, password));
        
        // Act
        var response = await client.PostAsync("/login",request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var setCookieHeader = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
        setCookieHeader.Should().NotBeNull("Куки сессии должны быть установлены");
        var cookieParts = setCookieHeader.Split(';');
        cookieParts.Should().Contain(part => part.Contains(".AspNetCore.Cookies="), "Куки должны содержать идентификатор сессии");
        cookieParts.First(part => part.Contains(".AspNetCore.Cookies=")).Split("=").Last().Length.Should()
            .BeGreaterThan(1);
        cookieParts.Should().Contain(part => part.Trim().ToLower().Contains("expires="), "Куки должны содержать дату истечения куки");
        cookieParts.Should().Contain(part => part.ToLower().Contains("httponly"), "Куки должны быть HttpOnly");
        cookieParts.Should().Contain(part => part.Trim().Contains("path=/"), "Куки должны быть доступны на всех путях");
    }
    
    private async Task Test_logout_is_successful(HttpClient client)
    {
        // Act
        var response = await client.PutAsync("/logout", null);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var setCookieHeader = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
        setCookieHeader.Should().NotBeNull("Куки сессии должны быть установлены");
        var cookieParts = setCookieHeader.Split(';');
        var sessionPart = cookieParts.First(part => part.Contains(".AspNetCore.Cookies="));
        sessionPart.Should().Be(".AspNetCore.Cookies=");

    }
}