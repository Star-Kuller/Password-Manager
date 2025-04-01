namespace PasswordManager.Application.Interfaces.Database.Repositories;

public interface ICrudRepository<T>
{
    Task<long> AddAsync(T user);
    Task UpdateAsync(T user);
    Task<T?> GetAsync(long id);
    Task DeleteAsync(long id);
}