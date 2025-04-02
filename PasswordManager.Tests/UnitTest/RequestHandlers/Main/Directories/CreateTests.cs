using FluentAssertions;
using Moq;
using PasswordManager.Application.Handlers.Main.Directories;
using PasswordManager.Application.Interfaces;
using PasswordManager.Application.Models.Encrypted;
using PasswordManager.Tests.Infrastructure.ObjectMothers;
using PasswordManager.Tests.Infrastructure.Spies;
using PasswordManager.Tests.Infrastructure.Spies.Factories;

namespace PasswordManager.Tests.UnitTest.RequestHandlers.Main.Directories;

public class CreateTests
{
    private readonly Mock<ISessionManager> _sessionManagerMock = new();
    private readonly UnitOfWorkSpyFactory _uowSpyFactory = new();
    private readonly Mock<ICryptographer> _cryptographerMock = new();
    private const long CurrentUserId = 1;
    private const long NewId = 2;

    private void SetUp()
    {
        var uow = new UnitOfWorkSpy();
        var defaultParent =
            EncryptedDirectoryMother
            .Builder()
            .SetOwnerId(CurrentUserId)
            .Build();
        
        uow.DirectoryRepositoryMock
            .Setup(r => r.GetAsync(It.IsAny<long>()))
            .ReturnsAsync(defaultParent);
        uow.DirectoryRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<EncryptedDirectory>()))
            .ReturnsAsync(NewId);
        _uowSpyFactory.Instants.Add(uow);
        _sessionManagerMock.Setup(sm => sm.GetCurrentUserId())
            .Returns(CurrentUserId);
    }
    
    [Fact]
    public async Task Create_directory_is_successful()
    {
        //Arrange
        SetUp();
        
        var request = new Create.Request(
            "TestTitle",
            1);
        
        var handler = new Create.Handler(
            _sessionManagerMock.Object,
            _uowSpyFactory,
            _cryptographerMock.Object
        );
        
        //Act
        var id = await handler.Handle(request, CancellationToken.None);
        
        //Assert
        id.Should().Be(NewId);
        _uowSpyFactory.Instants[0].DirectoryRepositoryMock
            .Verify(ar => ar.AddAsync(It.IsAny<EncryptedDirectory>()), Times.Once);
    }
}