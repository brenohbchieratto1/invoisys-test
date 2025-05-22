using System.Web;

namespace Strategyo.Extensions;

public static class QueryExtensions
{
    private static readonly char[] Separator = ['&'];
    
    public static string ToQueryString(this object? request)
    {
        if (request == null)
        {
            return string.Empty;
        }
        
        var parameters = from p in request.GetType().GetProperties()
                         let value = p.GetValue(request, null)
                         where value != null
                         select $"{p.Name}={HttpUtility.UrlEncode(value.ToString())}";
        
        return $"?{string.Join("&", parameters)}";
    }
    
    public static Dictionary<string, string> ParseQuery(this string url)
    {
        var queryString = url[(url.IndexOf('?') + 1)..];
        var queryParameters = queryString.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
        
        return queryParameters
              .Select(param => param.Split('='))
              .Where(parts => parts.Length == 2)
              .ToDictionary(parts => Uri.UnescapeDataString(parts[0]), parts => Uri.UnescapeDataString(parts[1]));
    }
}