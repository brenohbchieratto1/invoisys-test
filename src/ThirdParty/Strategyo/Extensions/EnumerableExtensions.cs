namespace Strategyo.Extensions;

public static class EnumerableExtensions
{
    public static IReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> list)
        => list.ToList().AsReadOnly();
    
    public static IEnumerable<Type> FindInterfacesThatClose(this Type? pluggedType, Type templateType) 
        => FindInterfacesThatClosesCore(pluggedType, templateType).Distinct();
    
    public static void Fill<T>(this List<T> list, T value)
    {
        if (list.Contains(value))
        {
            return;
        }

        list.Add(value);
    }
    
    private static IEnumerable<Type> FindInterfacesThatClosesCore(Type? pluggedType, Type templateType)
    {
        if (pluggedType == null) yield break;

        if (!pluggedType.IsConcrete()) yield break;

        if (templateType.IsInterface)
        {
            foreach (
                var interfaceType in
                pluggedType.GetInterfaces()
                           .Where(type => type.IsGenericType && (type.GetGenericTypeDefinition() == templateType)))
            {
                yield return interfaceType;
            }
        }
        else if (pluggedType.BaseType!.IsGenericType &&
                 (pluggedType.BaseType!.GetGenericTypeDefinition() == templateType))
        {
            yield return pluggedType.BaseType!;
        }

        if (pluggedType.BaseType == typeof(object)) yield break;

        foreach (var interfaceType in FindInterfacesThatClosesCore(pluggedType.BaseType!, templateType))
        {
            yield return interfaceType;
        }
    }
}