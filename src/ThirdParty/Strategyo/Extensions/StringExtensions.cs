using System.Text.RegularExpressions;

namespace Strategyo.Extensions;

public static partial class StringExtensions
{
    /// <summary>
    ///     Returns a string between two strings
    /// </summary>
    /// <param name="original"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static string Between(this string original, string start, string end)
    {
        var startPos = original.IndexOf(start, StringComparison.OrdinalIgnoreCase) + start.Length;
        var endPos = original.IndexOf(end, startPos, StringComparison.OrdinalIgnoreCase);
        var finalString = original.Substring(startPos, endPos - startPos);
        return finalString;
    }
    
    public static string Between(this string original, string delimiter)
    {
        var startPos = original.IndexOf(delimiter, StringComparison.OrdinalIgnoreCase) + delimiter.Length;
        var endPos = original.IndexOf(delimiter, startPos, StringComparison.OrdinalIgnoreCase);
        var finalString = original.Substring(startPos, endPos - startPos);
        return finalString;
    }
    
    /// <summary>
    ///     Replace string between two strings
    /// </summary>
    /// <param name="original"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="replace"></param>
    /// <returns></returns>
    public static string ReplaceBetween(this string original, string start, string end, string replace)
    {
        var startPos = original.IndexOf(start, StringComparison.OrdinalIgnoreCase) + start.Length;
        var endPos = original.IndexOf(end, startPos, StringComparison.OrdinalIgnoreCase);
        var finalString = original.Substring(startPos, endPos - startPos);
        return original.Replace(finalString, replace);
    }
    
    
    public static List<string> AllMatches(this string original, Regex regex)
    {
        var matches = regex.Matches(original).Select(x => x.Value);
        return matches.ToList();
    }
    
    public static bool IsNullOrEmpty(this string? original)
        => string.IsNullOrEmpty(original);
    
    public static bool IsNotNullOrEmpty(this string? original)
        => !string.IsNullOrEmpty(original);
    
    public static bool TryParseString(this object? obj, out string value)
    {
        value = obj?.ToString() ?? string.Empty;
        return true;
    }
    
    public static bool StartsWithAny(this string value, IEnumerable<string> strings)
        => strings.Any(value.StartsWith);
    
    public static string JoinStrings(this IEnumerable<string?> strings, string separator)
        => string.Join(separator, strings.Where(x => x.IsNotNullOrEmpty()));
    
    public static string? ToCamelCase(this string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        var firstChar = char.ToLower(value[0]);

        var camelCase = firstChar + value[1..];
        
        return camelCase;
    }
}