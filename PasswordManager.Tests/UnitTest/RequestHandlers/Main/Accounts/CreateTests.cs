using FluentAssertions;
using Moq;
using PasswordManager.Application.Handlers.Main.Accounts;
using PasswordManager.Application.Interfaces;
using PasswordManager.Application.Models.Encrypted;
using PasswordManager.Tests.Infrastructure.Spies;
using PasswordManager.Tests.Infrastructure.Spies.Factories;

namespace PasswordManager.Tests.UnitTest.RequestHandlers.Main.Accounts;

public class CreateTests
{
    private readonly UnitOfWorkSpyFactory _uowSpyFactory = new();
    private readonly Mock<ICryptographer> _cryptographerMock = new();
    private const long NewId = 1;

    private void SetUp()
    {
        var uow = new UnitOfWorkSpy();
        uow.AccountRepositoryMock
            .Setup(ar => ar.AddAsync(It.IsAny<EncryptedAccount>()))
            .ReturnsAsync(NewId);
        _uowSpyFactory.Instants.Add(uow);
    }
    
    [Fact]
    public async Task Create_account_is_successful()
    {
        //Arrange
        SetUp();
        
        var request = new Create.Request(
            "TestTitle",
            "test.com",
            "TestLogin",
            "TestPassword",
            1);
        
        var handler = new Create.Handler(
            _uowSpyFactory,
            _cryptographerMock.Object
        );
        
        //Act
        var id = await handler.Handle(request, CancellationToken.None);
        
        //Assert
        id.Should().Be(1);
        _uowSpyFactory.Instants[0].AccountRepositoryMock
            .Verify(ar => ar.AddAsync(It.IsAny<EncryptedAccount>()), Times.Once);
    }
}