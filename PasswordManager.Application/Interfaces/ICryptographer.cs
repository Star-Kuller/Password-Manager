namespace PasswordManager.Application.Interfaces;

public interface ICryptographer
{
    byte[] Encrypt(string content);
    string Decrypt(byte[] encryptedContent);
}