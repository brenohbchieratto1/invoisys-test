using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Cysharp.Serialization.Json;

namespace Strategyo.Extensions.Conversions;

public static class JsonExtensions
{
    public static readonly JsonSerializerOptions DefaultJsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        AllowTrailingCommas = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        Converters =
        {
            new JsonStringEnumConverter(),
            new UlidJsonConverter()
        },
        #if DEBUG
        WriteIndented = true,
        #endif
    };

    public static string? ToJson(this object? str) 
        => str == null ? null : JsonSerializer.Serialize(str, DefaultJsonSerializerOptions);

    public static JsonObject? ToJsonObject(this string? str) 
        => str == null ? null : FromJson<JsonObject>(str);

    public static T? FromJson<T>(this string? str, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        if (str == null)
        {
            return default;
        }

        var result = JsonSerializer.Deserialize<T>(str, jsonSerializerOptions ?? DefaultJsonSerializerOptions);
        return result;
    }

    public static async Task<T?> FromJson<T>(this Stream stream)
    {
        stream.Position = 0;
        return await JsonSerializer.DeserializeAsync<T>(stream, DefaultJsonSerializerOptions);
    }

    public static bool TryParseJson<T>(this string? str, out T? result, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        result = default;
        try
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            result = str.FromJson<T>(jsonSerializerOptions);
            return result != null;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}