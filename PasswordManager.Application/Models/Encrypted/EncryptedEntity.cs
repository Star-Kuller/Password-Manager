using PasswordManager.Application.Interfaces;
using PasswordManager.Domain.Entities;
using PasswordManager.Domain.Interfaces;


namespace PasswordManager.Application.Models.Encrypted;

public abstract class EncryptedEntity<T>() : IId
    where T : Entity
{
    public long? Id { get; set; }
    public abstract T ToEntity(ICryptographer cryptographer);
}