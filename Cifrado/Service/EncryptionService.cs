using Cifrado.Interface;
using System.Security.Cryptography;
using System.Text;

namespace Cifrado.Service
{
    public class EncryptionService: IEncryptionService
    {
        public (string EncryptedText, string Key) Encrypt(string text)
        {
          
            string key = Guid.NewGuid().ToString("N");
            byte[] keyBytes = GetKeyFromString(key);

            using (var aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (var ms = new MemoryStream())
                    {
                       
                        ms.Write(aes.IV, 0, aes.IV.Length);

                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (var sw = new StreamWriter(cs))
                            {
                                sw.Write(text);
                            }
                        }
                        
                        return (Convert.ToBase64String(ms.ToArray()), key);
                    }
                }
            }

        }
        public string Decrypt(string encryptedText, string key)
        {
            byte[] buffer = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = GetKeyFromString(key);

            using (var aes = Aes.Create())
            {
                aes.Key = keyBytes;

              
                byte[] iv = new byte[16];
                Array.Copy(buffer, 0, iv, 0, iv.Length);
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (var ms = new MemoryStream(buffer, iv.Length, buffer.Length - iv.Length))
                    {
                        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (var sr = new StreamReader(cs))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }

        private byte[] GetKeyFromString(string key)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
            }
        }
    }
}
