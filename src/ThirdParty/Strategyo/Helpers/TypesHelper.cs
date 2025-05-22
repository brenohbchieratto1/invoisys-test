using System.Reflection;
using Strategyo.Extensions;

namespace Strategyo.Helpers;

public static class TypesHelper
{
    static TypesHelper()
    {
        Initialize();
    }

    public static HashSet<Assembly> Assemblies { get; } = [];

    public static HashSet<Type> AllTypes { get; } = [];

    public static Dictionary<string, Type> TypesDictionary { get; set; } = [];

    private static void Initialize()
    {
        //Current Running Assembly
        var currentAssembly = Assembly.GetExecutingAssembly();

        Assemblies.Add(currentAssembly);
        AllTypes.UnionWith(currentAssembly.GetTypes());

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            if (!((assembly.GetName().Name?.StartsWith("Strategyo") ?? false)
               || (assembly.GetName().Name?.StartsWith("App")       ?? false)))
            {
                continue;
            }

            Assemblies.Add(assembly);
            AllTypes.UnionWith(assembly.GetTypes());
        }

        //Create dictionary of types
        foreach (var type in AllTypes)
        {
            TypesDictionary.GetOrAdd(type.Name, type);
        }
    }

    public static Assembly GetAssemblyFromName(this string assemblyName)
    {
        var assembly = Assemblies.FirstOrDefault(x => x.GetName().Name == assemblyName);

        if (assembly == null)
        {
            throw new Exception($"Assembly {assemblyName} not found");
        }

        return assembly;
    }

    public static Type GetTypeFromName(this string typeName)
    {
        var type = AllTypes.FirstOrDefault(x => x.Name == typeName);

        if (type == null)
        {
            throw new Exception($"Type {typeName} not found");
        }

        return type;
    }
}