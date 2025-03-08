using Moq;
using PasswordManager.Application.Interfaces.Database;
using PasswordManager.Application.Interfaces.Database.Repositories;

namespace PasswordManager.Tests.Infrastructure.Spies;

public class DbUnitOfWorkSpy : IDbUnitOfWork
{
    public Mock<IUserRepository> UserRepositoryMock { get; set; } = new();
    public int BeginNewTransactionCounter { get; private set; }
    public int CommitCounter { get; private set; }
    public int RollBackCounter { get; private set; }
    public int DisposeCounter { get; private set; }
    

    public IUserRepository UserRepository => UserRepositoryMock.Object;

    public void BeginNewTransaction()
    {
        BeginNewTransactionCounter++;
    }

    public void Commit()
    {
        CommitCounter++;
    }

    public void RollBack()
    {
        RollBackCounter++;
    }
    
    public void Dispose()
    {
        DisposeCounter++;
    }
}