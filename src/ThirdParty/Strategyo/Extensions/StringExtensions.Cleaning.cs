using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Strategyo.Extensions;

public static partial class StringExtensions
{
    /// <summary>
    ///     Removes all non ASCII characters from a string
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    public static string CleanStringASCII(this string original)
        => ASCIIRegex().Replace(original, string.Empty);
    
    /// <summary>
    ///     Clean a string to only contain alpha numeric characters
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    public static string CleanStringAlphaNumeric(this string original)
        => AlphaNumericRegex().Replace(original, string.Empty);
    
    /// <summary>
    ///     Remove all diacritics from a string like á, à, ã, ç, etc.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string RemoveDiacritics(this string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormC);
        var stringBuilder = new StringBuilder();
        
        foreach (var c in from c in normalizedString let unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c) where unicodeCategory != UnicodeCategory.NonSpacingMark select c)
        {
            stringBuilder.Append(c);
        }
        
        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
    
    /// <summary>
    ///     Replace all diacritics from a string like á, à, ã, ç, etc.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string ReplaceDiacritics(this string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();
        
        foreach (var c in normalizedString.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
        {
            stringBuilder.Append(c);
        }
        
        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
    
    public static string RemoveNoBreakSpace(this string value)
        => value.Replace("\u00A0", string.Empty).Replace("﻿", string.Empty);
    
    /// <summary>
    ///     Truncate a string to a maximum length
    /// </summary>
    /// <param name="value"></param>
    /// <param name="maxLength"></param>
    /// <returns></returns>
    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }
        
        return value.Length <= maxLength ? value : value[..maxLength];
    }
    
    [GeneratedRegex(@"[^0-9a-zA-Z]+")]
    private static partial Regex AlphaNumericRegex();
    
    [GeneratedRegex(@"[^\u0000-\u007F]+")]
    private static partial Regex ASCIIRegex();
}