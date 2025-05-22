using System.Text;

namespace Strategyo.Extensions.Conversions;

public static class StreamExtensions
{
    private static async Task<MemoryStream> ToMemoryStreamAsync(byte[] content)
    {
        var stream = new MemoryStream();
        await stream.WriteAsync(content);
        stream.Position = 0;
        return stream;
    }
    
    public static async Task<Stream> ToStream(this string str)
    {
        var content = Encoding.UTF8.GetBytes(str);
        return await ToMemoryStreamAsync(content);
    }
    
    public static async Task<Stream> ToStream(this byte[] byteArray) 
        => await ToMemoryStreamAsync(byteArray);

    public static async Task<MemoryStream> ReadAsStream(this Stream input)
    {
        if (input is MemoryStream stream)
        {
            return stream;
        }
        
        var memoryStream = new MemoryStream();
        await input.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        return memoryStream;
    }
    
    public static async Task<byte[]> ReadAsByteArray(this Stream input)
    {
        if (input is MemoryStream stream)
        {
            return stream.ToArray();
        }
        
        using var memoryStream = new MemoryStream();
        await input.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        return memoryStream.ToArray();
    }
    
    public static async Task<string> ReadAsString(this Stream input)
    {
        var bytesRead = await input.ReadAsByteArray();
        return Encoding.UTF8.GetString(bytesRead).RemoveNoBreakSpace();
    }
    
    public static async Task<string> ReadAsBase64(this Stream input)
    {
        var bytes = await input.ReadAsByteArray();
        return await bytes.ToBase64();
    }
}