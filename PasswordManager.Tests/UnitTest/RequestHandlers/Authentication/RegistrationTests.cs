using FluentAssertions;
using Moq;
using PasswordManager.Application.Handlers.Authentication;
using PasswordManager.Application.Interfaces;
using PasswordManager.Domain.Entities;
using PasswordManager.Tests.Infrastructure.Spies;

namespace PasswordManager.Tests.UnitTest.RequestHandlers.Authentication;

public class RegistrationTests
{
    private readonly Mock<ISessionManager> _mockSessionManager = new();
    private readonly DbUnitOfWorkSpy _dbUnitOfWorkSpy = new();
    private const long UserId = 1;

    private void SetUp()
    {
        _mockSessionManager.Setup(sm => sm.CreateSession(It.IsAny<long>()));
        var mockUserRepository = _dbUnitOfWorkSpy.UserRepositoryMock;
        mockUserRepository.Setup(ur => ur.AddAsync(It.IsAny<User>()))
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
            _dbUnitOfWorkSpy,
            _mockSessionManager.Object);
        
        //Act
        await handler.Handle(request, CancellationToken.None);
        
        //Assert
        _dbUnitOfWorkSpy.UserRepositoryMock.Verify(ur =>
            ur.AddAsync(It.Is<User>(
                u =>
                    u.EmailConfirmed == false &&
                    u.Email == request.Email &&
                    u.SecretKey == request.Secret
            )), Times.Once);
        _dbUnitOfWorkSpy.CommitCounter.Should().Be(1);
        
        _mockSessionManager.Verify(sm => sm.CreateSession(UserId));
    }
    
    [Fact]
    public async Task Registration_when_user_already_exist()
    {
        //Arrange
        SetUp();
        
        var request = new Registration.Request(
            "encrypted_secret_key",
            "test@email.com",
            "password_hash");
        
        var handler = new Registration.Handler(
            _dbUnitOfWorkSpy,
            _mockSessionManager.Object);
        
        //Act
        await handler.Handle(request, CancellationToken.None);
        
        //Assert
        _dbUnitOfWorkSpy.UserRepositoryMock.Verify(ur =>
            ur.AddAsync(It.Is<User>(
                u =>
                    u.EmailConfirmed == false &&
                    u.Email == request.Email &&
                    u.SecretKey == request.Secret
            )), Times.Once);
        _dbUnitOfWorkSpy.CommitCounter.Should().Be(1);
        
        _mockSessionManager.Verify(sm => sm.CreateSession(UserId));
    }
}