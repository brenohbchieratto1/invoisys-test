using System.Reflection;

namespace Strategyo.Extensions;

public static class AttributeExtensions
{
    public static List<(PropertyInfo Property, TAttribute Attribute)> GetPropertiesWithAttribute<TAttribute>(this Type type) where TAttribute : Attribute
    {
        var sortedProperties = from prop in type.GetProperties()
                               let att = prop.GetCustomAttribute<TAttribute>()
                               where att != null
                               select (prop, att);

        return sortedProperties.ToList();
    }
}