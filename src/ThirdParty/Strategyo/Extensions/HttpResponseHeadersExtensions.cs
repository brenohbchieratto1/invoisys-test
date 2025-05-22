using System.Net.Http.Headers;

namespace Strategyo.Extensions;

public static class HttpResponseHeadersExtensions
{
    public static T? GetHeaderValue<T>(this HttpResponseHeaders headers, string name)
    {
        if (!headers.TryGetValues(name, out var values))
        {
            return default;
        }
            
        var value = values.FirstOrDefault();
        if (value != null)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
            
        return default;
    }
}