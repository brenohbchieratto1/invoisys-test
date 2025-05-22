using System.Net;

namespace Strategyo.Extensions;

public static partial class StringExtensions
{
    /// <summary>
    ///     Adds http:// to the url if it doesnt start with http:// or https://
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string NormalizeUrl(this string url)
    {
        if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return url;
        }
        
        return $"http://{url}";
    }
    
    public static string ToKebabCase(this string input, bool onlyDots = false)
    {
        if (onlyDots)
        {
            input = input.Replace(".", "-").ToLower();
            return input;
        }
        
        input = input.Replace(".", "");
        var span = input.AsSpan();
        var output = new char[span.Length * 2];
        var j = 0;
        // ReSharper disable once ForCanBeConvertedToForeach
        for (var i = 0; i < span.Length; i++)
        {
            var c = span[i];
            //If first char is uppercase, make it lowercase
            if (i == 0)
            {
                output[j++] = char.ToLower(c);
                continue;
            }
            
            
            if (char.IsUpper(c))
            {
                output[j++] = '-';
                output[j++] = char.ToLower(c);
            }
            else
            {
                output[j++] = c;
            }
        }
        
        return new string(output, 0, j);
    }
    
    public static string ToStringIgnoreDefault<T>(this T? obj)
        => obj switch
        {
            DateTime time when time == default || time == DateTime.MinValue => string.Empty,
            DateTime time                                                   => time.DateTimeToString(),
            _                                                               => obj?.ToString() ?? string.Empty
        };
    
    public static string ToFriendlyCase(this string pascalString)
    {
        if (string.IsNullOrEmpty(pascalString))
        {
            return pascalString;
        }
        
        var input = pascalString.AsSpan();
        var result = new char[input.Length * 2];
        var resultIndex = 0;
        result[resultIndex++] = input[0];
        for (var i = 1; i < input.Length; i++)
        {
            if (char.IsUpper(input[i]))
            {
                result[resultIndex++] = ' ';
            }
            
            result[resultIndex++] = input[i];
        }
        
        return new string(result, 0, resultIndex);
    }
    
    public static string UrlEncode(this string value)
        => WebUtility.UrlEncode(value);
}