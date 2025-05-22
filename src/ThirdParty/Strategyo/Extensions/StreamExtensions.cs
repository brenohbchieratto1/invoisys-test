namespace Strategyo.Extensions;

public static class StreamExtensions
{
    public static async Task<byte[]> ReadFully(this Stream input)
    {
        using var ms = new MemoryStream();
        await input.CopyToAsync(ms);
        return ms.ToArray();
    }
}