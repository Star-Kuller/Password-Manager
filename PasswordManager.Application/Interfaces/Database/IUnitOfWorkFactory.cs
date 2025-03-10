namespace PasswordManager.Application.Interfaces.Database;

public interface IUnitOfWorkFactory
{
    Task<IUnitOfWork> CreateAsync(CancellationToken token);
}