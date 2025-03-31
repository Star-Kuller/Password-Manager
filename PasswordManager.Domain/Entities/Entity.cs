using PasswordManager.Domain.Interfaces;

namespace PasswordManager.Domain.Entities;

public class Entity : IId
{
    public long? Id { get; init; }
}