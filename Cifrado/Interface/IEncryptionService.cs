namespace Cifrado.Interface
{
    public interface IEncryptionService
    {
        (string EncryptedText, string Key) Encrypt(string text);
        string Decrypt(string encryptedText, string key);
    }
}
