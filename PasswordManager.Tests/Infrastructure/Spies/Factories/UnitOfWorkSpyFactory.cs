using PasswordManager.Application.Interfaces.Database;

namespace PasswordManager.Tests.Infrastructure.Spies.Factories;

public class UnitOfWorkSpyFactory : IUnitOfWorkFactory
{
    public UnitOfWorkSpy Instant { get; set; } = new();
    public Task<IUnitOfWork> CreateAsync(CancellationToken token)
    {
        return Task.FromResult<IUnitOfWork>(Instant);
    }
}