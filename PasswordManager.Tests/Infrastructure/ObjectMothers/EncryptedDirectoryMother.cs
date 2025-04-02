using PasswordManager.Application.Models.Encrypted;
using PasswordManager.Tests.Infrastructure.TestDataBuilders;

namespace PasswordManager.Tests.Infrastructure.ObjectMothers;

public static class EncryptedDirectoryMother
{
    public static EncryptedDirectory Default() => new EncryptedDirectoryBuilder().Build();
    
    public static EncryptedDirectoryBuilder Builder() => new EncryptedDirectoryBuilder();
}