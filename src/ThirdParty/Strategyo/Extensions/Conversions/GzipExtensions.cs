using System.IO.Compression;

namespace Strategyo.Extensions.Conversions;

public static class GzipExtensions
{
    public static async Task<MemoryStream> ToGzip(this Stream stream)
    {
        var memoryStream = new MemoryStream();
        await using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
        {
            await stream.CopyToAsync(gzipStream);
        }
        
        memoryStream.Position = 0;
        return memoryStream;
    }
    
    public static async Task<string> ToGzipBase64(this Stream stream)
    {
        using var memoryStream = new MemoryStream();
        await using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
        {
            await stream.CopyToAsync(gzipStream);
        }
        
        return Convert.ToBase64String(memoryStream.ToArray());
    }
    
    public static async Task<MemoryStream> FromGzip(this Stream stream)
    {
        var memoryStream = new MemoryStream();
        await using (var gzipStream = new GZipStream(stream, CompressionMode.Decompress))
        {
            await gzipStream.CopyToAsync(memoryStream);
        }
        
        memoryStream.Position = 0;
        return memoryStream;
    }
}