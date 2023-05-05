using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;

public class EncryptDecrypt
{
    public static string Encrypt(string plainText, string secrateKey)
    {
        byte[] _iv = new byte[16];
        byte[] _memory;

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(secrateKey);
            aes.IV = _iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    _memory = memoryStream.ToArray();
                }
            }
        }

        return Convert.ToBase64String(_memory);
    }

    public static string Decrypt(string encryptedText, string secrateKey)
    {
        byte[] _iv = new byte[16];
        byte[] _memory = Convert.FromBase64String(encryptedText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(secrateKey);
            aes.IV = _iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream(_memory))
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }
}

