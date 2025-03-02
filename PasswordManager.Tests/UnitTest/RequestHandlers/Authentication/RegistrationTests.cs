using FluentAssertions;
using PasswordManager.Application.Handlers.Authentication;

namespace PasswordManager.Tests.UnitTest.RequestHandlers.Authentication;

public class RegistrationTests
{
    [Fact]
    public async Task Registration_successful()
    {
        //Arrange
        var request = new Registration.Request("", "", "");
        var handler = new Registration.Handler();
        
        //Act
        var response = await handler.Handle(request, CancellationToken.None);
        
        //Assert
        response.Should().NotBeNull();
    }
}