using PasswordManager.Domain.Entities;

namespace PasswordManager.Application.Interfaces;

public interface IUserRepository
{
    Task<long> AddAsync(User user);
    Task UpdateAsync(User user);
    Task<User> GetAsync(long id);
    Task<User> GetAsync(string email);
}