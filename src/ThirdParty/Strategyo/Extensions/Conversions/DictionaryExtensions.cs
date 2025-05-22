using System.Reflection;

namespace Strategyo.Extensions.Conversions;

public static class DictionaryExtensions
{
    public static Dictionary<string, object> ToDictionary<T>(this T obj)
    {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        var dictionary = new Dictionary<string, object>();
        
        foreach (var property in properties)
        {
            var propertyName = property.Name;
            
            var propertyValue = property.GetValue(obj)!;
            
            dictionary.Add(propertyName, propertyValue);
        }
        
        return dictionary;
    }
}