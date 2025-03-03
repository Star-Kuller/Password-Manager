using FluentAssertions;
using Moq;
using PasswordManager.Application.Handlers.Authentication;
using PasswordManager.Application.Interfaces;
using PasswordManager.Domain.Entities;

namespace PasswordManager.Tests.UnitTest.RequestHandlers.Authentication;

public class RegistrationTests
{
    private Mock<ISessionManager> _mockSessionManager = new();
    private Mock<IUserRepository> _mockUserRepository = new();
    private Mock<ICryptographer> _mockCryptographer = new();

    private void SetUp()
    {
        _mockSessionManager.Setup(sm => sm.CreateSession(1))
            .Returns("TestSessionId");
        _mockCryptographer.Setup(ct => ct.Encrypt(It.IsAny<string>()))
            .Returns([01,02,03,04,05]);
        _mockUserRepository.Setup(ur => ur.AddAsync(It.IsAny<User>()))
            .ReturnsAsync(1);
    }
        
    [Fact]
    public async Task Registration_successful()
    {
        //Arrange
        var request = new Registration.Request(
            "encrypted_secret_key",
            "test@email.com",
            "password_hash");
        
        var handler = new Registration.Handler(
            _mockUserRepository.Object,
            _mockCryptographer.Object,
            _mockSessionManager.Object);
        
        //Act
        var response = await handler.Handle(request, CancellationToken.None);
        
        //Assert
        response.Should().NotBeNull();
    }
}