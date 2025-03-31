using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using PasswordManager.Application.Handlers.Authentication;
using PasswordManager.Application.Interfaces;
using PasswordManager.Application.Models.Encrypted;
using PasswordManager.Tests.Infrastructure.Spies;
using PasswordManager.Tests.Infrastructure.Spies.Factories;

namespace PasswordManager.Tests.UnitTest.RequestHandlers.Authentication;

public class LoginTests
{
    private readonly Mock<ISessionManager> _sessionManagerMock = new();
    private readonly UnitOfWorkSpyFactory _uowSpyFactory = new();
    private readonly Mock<ICryptographer> _cryptographerMock = new();
    private readonly Mock<ILogger<Login.Handler>> _loggerMock = new();
    private const long UserId = 1;

    private void SetUp()
    {
        _sessionManagerMock.Setup(sm => sm.CreateSession(It.IsAny<long>()));
        var uowSpy = new UnitOfWorkSpy();
        uowSpy.UserRepositoryMock.Setup(ur => ur.GetAsync(It.IsAny<byte[]>()))
            .ReturnsAsync(new EncryptedUser
            {
                Id = UserId,
                Login = [01, 02, 03, 04, 05],
                Password_hash = [01, 02, 03, 04, 05],
                Secret_key = [01, 02, 03, 04, 05],
            });
        _uowSpyFactory.Instants.Add(uowSpy);
    }
    
    [Fact]
    public async Task Login_is_successful()
    {
        //Arrange
        SetUp();
        
        var request = new Login.Request(
            "test@email.com",
            "password_hash");
        
        var handler = new Login.Handler(
            _uowSpyFactory,
            _sessionManagerMock.Object,
            _cryptographerMock.Object,
            _loggerMock.Object
        );

        var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("password_hash");
        _cryptographerMock.Setup(c => c.Decrypt(It.IsAny<byte[]>()))
            .Returns(passwordHash);
        
        //Act
        await handler.Handle(request, CancellationToken.None);
        
        //Assert
        _sessionManagerMock.Verify(sm => sm.CreateSession(UserId));
    }
    
    [Fact]
    public async Task Login_is_wrong_password()
    {
        //Arrange
        SetUp();
        
        var request = new Login.Request(
            "test@email.com",
            "wrong_password_hash");
        
        var handler = new Login.Handler(
            _uowSpyFactory,
            _sessionManagerMock.Object,
            _cryptographerMock.Object,
            _loggerMock.Object
        );

        var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("password_hash");
        _cryptographerMock.Setup(c => c.Decrypt(It.IsAny<byte[]>()))
            .Returns(passwordHash);
        
        //Act
        var act = async () => await handler.Handle(request, CancellationToken.None);
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("Неверный логин или пароль");
        
        //Assert
        _sessionManagerMock.Verify(sm => sm.CreateSession(UserId), Times.Never);
    }
    
    [Fact]
    public async Task Login_is_wrong_login()
    {
        //Arrange
        SetUp();
        
        var request = new Login.Request(
            "test11@email.com",
            "password_hash");
        
        var handler = new Login.Handler(
            _uowSpyFactory,
            _sessionManagerMock.Object,
            _cryptographerMock.Object,
            _loggerMock.Object
        );
        
        _uowSpyFactory.Instants[0].UserRepositoryMock.Setup(ur => ur.GetAsync(It.IsAny<byte[]>()))
            .ReturnsAsync((EncryptedUser?)null);
        
        //Act
        var act = async () => await handler.Handle(request, CancellationToken.None);
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("Неверный логин или пароль");
        
        //Assert
        _sessionManagerMock.Verify(sm => sm.CreateSession(UserId), Times.Never);
    }
}