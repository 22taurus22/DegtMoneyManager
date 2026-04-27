using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using DegtMoney.Interfaces;


namespace DegtMoney.Services
{
    internal class AesEncryptionService : IEncryptionService
    {
        private readonly byte[] _key = Convert.FromBase64String("YOUR_BASE64_256BIT_KEY"); // замените на свой ключ
        private readonly byte[] _iv = Convert.FromBase64String("YOUR_BASE64_128BIT_IV");   // замените на свой вектор

        public string Encrypt(string plainText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;
                var encryptor = aes.CreateEncryptor();
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                return Convert.ToBase64String(cipherBytes);
            }
        }

        public string Decrypt(string cipherText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;
                var decryptor = aes.CreateDecryptor();
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                return Encoding.UTF8.GetString(plainBytes);
            }
        }
    }
}
