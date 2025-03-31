using PasswordManager.Application.Interfaces.Database;

namespace PasswordManager.Tests.Infrastructure.Spies.Factories;

public class UnitOfWorkSpyFactory : IUnitOfWorkFactory
{
    public List<UnitOfWorkSpy> Instants { get; set; } = new();

    public int CallCounter = 0;
    public Task<IUnitOfWork> CreateAsync(CancellationToken token)
    {
        var instant = Instants[CallCounter];
        CallCounter++;
        return Task.FromResult<IUnitOfWork>(instant);
    }
}