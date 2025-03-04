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

    private const string SessionId = "TestSessionId";
    private const long UserId = 1;
    private readonly byte[] _encryptedData = [01, 02, 03, 04, 05];

    private void SetUp()
    {
        _mockSessionManager.Setup(sm => sm.CreateSession(It.IsAny<long>()))
            .Returns(SessionId);
        _mockCryptographer.Setup(ct => ct.Encrypt(It.IsAny<string>()))
            .Returns(_encryptedData);
        _mockUserRepository.Setup(ur => ur.GetAsync(It.IsAny<string>()))!
            .ReturnsAsync((User?)null);
        _mockUserRepository.Setup(ur => ur.AddAsync(It.IsAny<User>()))
            .ReturnsAsync(UserId);
    }
        
    [Fact]
    public async Task Registration_successful()
    {
        //Arrange
        SetUp();
        
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
        _mockUserRepository.Verify(ur =>
            ur.AddAsync(It.Is<User>(
                u =>
                    u.EmailConfirmed == false &&
                    u.Email == _encryptedData &&
                    u.SecretKey == _encryptedData
            )), Times.Once);
        
        response.Should().NotBeNull();
        response.Should().Be(SessionId);
    }
}