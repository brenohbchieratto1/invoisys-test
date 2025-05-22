namespace Strategyo.Extensions;

public static class UrlExtensions
{
    public static string EnsureHttps(this string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return url;
        }
        
        var uri = new UriBuilder(url);
        
        if (uri.Scheme != Uri.UriSchemeHttps)
        {
            uri.Scheme = Uri.UriSchemeHttps;
        }
        
        return uri.Uri.ToString();
    }
    
    public static Uri EnsureHttps(this Uri uri)
    {
        try
        {
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                return uri;
            }
            
            var builder = new UriBuilder(uri)
            {
                Scheme = Uri.UriSchemeHttps
            };
            
            uri = builder.Uri;
            
            return uri;
        }
        catch (Exception)
        {
            return uri;
        }
    }
    
    public static string CleanUrl(this string url)
    {
        url = url.Replace("http://", "").Replace("https://", "").Replace("www.", "");
        
        if (url.EndsWith('/'))
        {
            url = url.Remove(url.Length - 1, 1);
        }
        
        if (url.Contains(':'))
        {
            url = url.Remove(url.IndexOf(':'), url.Length - url.IndexOf(':'));
        }
        
        return url;
    }
}