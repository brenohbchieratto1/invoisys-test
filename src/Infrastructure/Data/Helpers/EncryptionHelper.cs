using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using Strategyo.Extensions.Conversions;

namespace App.InvoiSysTest.Infrastructure.Data.Helpers;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class EncryptionHelper
{
    private static readonly byte[] Key = "xW9vAqPzJfT3uGc7MdE4rHyNqLzV8bXe"u8.ToArray();
    private static readonly byte[] IV = "A7dPmE9sUqWzL0Bt"u8.ToArray();

    public static async Task<string> EncryptAsync(this string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        var plainBytes = Encoding.UTF8.GetBytes(plainText);

        using var ms = new MemoryStream();
        await using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        cs.Write(plainBytes, 0, plainBytes.Length);
        await cs.FlushFinalBlockAsync();

        var convert = await ms.ToArray().ToBase64();
        return convert;
    }

    public static async Task<string> DecryptAsync(this string cipherText)
    {
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        var cipherBytes = await cipherText.FromBase64ToBytes();
        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using var ms = new MemoryStream(cipherBytes);
        await using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        var convert = await sr.ReadToEndAsync();
        return convert;
    }
}