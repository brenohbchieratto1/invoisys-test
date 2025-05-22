using System.Text;

namespace Strategyo.Extensions.Conversions;

public static class Base64Extensions
{
    public static Task<string> FromBase64ToString(this string toDecode) =>
        Task.Run(() =>
        {
            try
            {
                var data = Convert.FromBase64String(toDecode);
                return Encoding.UTF8.GetString(data);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        });

    public static Task<byte[]> FromBase64ToBytes(this string toDecode) =>
        Task.Run(() =>
        {
            var data = Convert.FromBase64String(toDecode);
            return data;
        });

    public static Task<MemoryStream> FromBase64ToMemoryStream(this string toDecode, bool leaveOpen = false) =>
        Task.Run(() =>
        {
            var data = Convert.FromBase64String(toDecode);
            return new MemoryStream(data, leaveOpen);
        });

    public static Task<string> ToBase64(this byte[] bytes) 
        => Task.Run(() => Convert.ToBase64String(bytes));

    public static Task<string> ToBase64(this string toEncode) =>
        Task.Run(() =>
        {
            var bytes = Encoding.UTF8.GetBytes(toEncode);
            return Convert.ToBase64String(bytes);
        });

    public static Task<string> ToBase64(this Stream stream) =>
        Task.Run(() =>
        {
            ArgumentNullException.ThrowIfNull(stream);
            
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            var bytes = memoryStream.ToArray();
            return Convert.ToBase64String(bytes);
        });
}