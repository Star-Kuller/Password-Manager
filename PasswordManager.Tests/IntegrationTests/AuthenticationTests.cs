using FluentAssertions;
using PasswordManager.Tests.Infrastructure;

namespace PasswordManager.Tests.IntegrationTests;

public class AuthenticationTests(WebAppFactory factory) : IntegrationTestBase(factory)
{
    public async Task Register_and_login_is_successful()
    {
        // Arrange:/register
        var client = AppFactory.CreateClient();
        const string registerReqJson = """
                                       {
                                         "Secret":"encrypted_secret_key",
                                         "Login":"test@email.com",
                                         "Password":"password_hash"
                                       }
                                       """;
        var registerReq = new StringContent(registerReqJson);
        
        // Act:/register
        var registerRes = await client.PostAsync("/register",registerReq);
        
        // Assert:/register
        registerRes.IsSuccessStatusCode.Should().BeTrue();
        
        // Arrange:/login
        // Act:/login
        // Assert:/login
    }
}