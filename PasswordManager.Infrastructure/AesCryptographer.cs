using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using PasswordManager.Application.Interfaces;

namespace PasswordManager.Infrastructure;

public class AesCryptographer : ICryptographer
{
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public AesCryptographer(IConfiguration configuration)
    {
        var serverSecret = configuration.GetSection("ServerSecret");
        _key = Convert.FromBase64String(serverSecret["Key"]!);
        _iv = Convert.FromBase64String(serverSecret["IV"]!);
        
        if (_key.Length != 32 || _iv.Length != 16)
        {
            throw new InvalidOperationException("Invalid key or IV length. Key must be 32 bytes, IV must be 16 bytes.");
        }
    }

    public byte[] Encrypt(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            throw new ArgumentException("Content cannot be null or empty.", nameof(content));
        }

        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;
        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        // Преобразование строки в байты
        var plainTextBytes = Encoding.UTF8.GetBytes(content);

        // Шифрование
        using var memoryStream = new MemoryStream();
        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
        {
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
        }
        return memoryStream.ToArray();
    }

    public string Decrypt(byte[] encryptedContent)
    {
        if (encryptedContent == null || encryptedContent.Length == 0)
        {
            throw new ArgumentException("Encrypted content cannot be null or empty.", nameof(encryptedContent));
        }

        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;
        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        // Дешифрование
        using var memoryStream = new MemoryStream(encryptedContent);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream, Encoding.UTF8);
        return streamReader.ReadToEnd();
    }
}