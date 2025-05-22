using Strategyo.Extensions.Conversions;

namespace Strategyo.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task<T?> FromJsonAsync<T>(this HttpResponseMessage responseMessageExtensions)
    {
        var json = await responseMessageExtensions.Content.ReadAsStringAsync();
        return json.FromJson<T>();
    }
}