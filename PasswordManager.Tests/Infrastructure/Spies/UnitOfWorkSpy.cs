using Moq;
using PasswordManager.Application.Interfaces.Database;
using PasswordManager.Application.Interfaces.Database.Repositories;

namespace PasswordManager.Tests.Infrastructure.Spies;

public class UnitOfWorkSpy : IUnitOfWork
{
    public Mock<IUserRepository> UserRepositoryMock { get; set; } = new();
    public Mock<IAccountRepository> AccountRepositoryMock { get; set; } = new();
    public int CommitCounter { get; private set; }
    public int RollBackCounter { get; private set; }
    
    public IUserRepository Users => UserRepositoryMock.Object;
    public IAccountRepository Accounts => AccountRepositoryMock.Object;

    public Task CommitAsync(CancellationToken cancellationToken)
    { 
        CommitCounter++; 
        return Task.CompletedTask;
    }

    public Task RollBackAsync(CancellationToken cancellationToken)
    {
        RollBackCounter++;
        return Task.CompletedTask;
    }
    
    public void Dispose() { }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}