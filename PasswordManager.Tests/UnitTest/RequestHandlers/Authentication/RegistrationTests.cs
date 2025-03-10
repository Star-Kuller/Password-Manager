using FluentAssertions;
using Moq;
using PasswordManager.Application.Handlers.Authentication;
using PasswordManager.Application.Interfaces;
using PasswordManager.Application.Models.Encrypted;
using PasswordManager.Tests.Infrastructure.Spies.Factories;

namespace PasswordManager.Tests.UnitTest.RequestHandlers.Authentication;

public class RegistrationTests
{
    private readonly Mock<ISessionManager> _sessionManagerMock = new();
    private readonly UnitOfWorkSpyFactory _uowSpyFactory = new();
    private readonly Mock<ICryptographer> _cryptographerMock = new();
    private const long UserId = 1;

    private void SetUp()
    {
        _sessionManagerMock.Setup(sm => sm.CreateSession(It.IsAny<long>()));
        _uowSpyFactory.Instant.UserRepositoryMock.Setup(ur => ur.AddAsync(It.IsAny<EncryptedUser>()))
            .ReturnsAsync(UserId);
        _cryptographerMock.Setup(c => c.Encrypt(It.IsAny<string>()))
            .Returns([01, 02, 03, 04, 05]);
    }
        
    [Fact]
    public async Task Registration_is_successful()
    {
        //Arrange
        SetUp();
        
        var request = new Registration.Request(
            "encrypted_secret_key",
            "test@email.com",
            "password_hash");
        
        var handler = new Registration.Handler(
            _uowSpyFactory,
            _sessionManagerMock.Object,
            _cryptographerMock.Object
            );
        
        //Act
        await handler.Handle(request, CancellationToken.None);
        
        //Assert
        var uow = _uowSpyFactory.Instant;
        uow.UserRepositoryMock.Verify(ur =>
            ur.AddAsync(It.IsAny<EncryptedUser>()), Times.Once);
        uow.CommitCounter.Should().Be(1);
        
        _sessionManagerMock.Verify(sm => sm.CreateSession(UserId));
    }
    
    [Fact]
    public async Task Registration_failed_when_user_already_exist()
    {
        //Arrange
        SetUp();
        
        var request = new Registration.Request(
            "encrypted_secret_key",
            "test@email.com",
            "password_hash");
        
        var handler = new Registration.Handler(
            _uowSpyFactory,
            _sessionManagerMock.Object,
            _cryptographerMock.Object);
        
        //Act
        await handler.Handle(request, CancellationToken.None);
        
        //Assert
        var uow = _uowSpyFactory.Instant;
        uow.UserRepositoryMock.Verify(ur =>
            ur.AddAsync(It.IsAny<EncryptedUser>()), Times.Once);
        uow.CommitCounter.Should().Be(1);
        
        _sessionManagerMock.Verify(sm => sm.CreateSession(UserId));
    }
}