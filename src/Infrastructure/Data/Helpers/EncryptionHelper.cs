using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace App.InvoisysTest.Infrastructure.Data.Helpers;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class EncryptionHelper
{
    private static readonly byte[] Key = "xW9vAqPzJfT3uGc7MdE4rHyNqLzV8bXe"u8.ToArray();
    private static readonly byte[] IV = "A7dPmE9sUqWzL0Bt"u8.ToArray();

    public static string Encrypt(this string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        var plainBytes = Encoding.UTF8.GetBytes(plainText);

        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        cs.Write(plainBytes, 0, plainBytes.Length);
        cs.FlushFinalBlock();

        return Convert.ToBase64String(ms.ToArray());
    }

    public static string Decrypt(this string cipherText)
    {
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        var cipherBytes = Convert.FromBase64String(cipherText);
        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using var ms = new MemoryStream(cipherBytes);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }
}