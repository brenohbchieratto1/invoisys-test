using System.Collections;
using System.Reflection;

namespace Strategyo.Extensions;

public static class ObjectExtensions
{
    public static Dictionary<string, object> GetPropertyPathsAndValues<T>(this T obj)
    {
        var objType = typeof(T);
        var props = objType.GetProperties();
        
        var updateValues = new Dictionary<string, object>();
        
        //Process the current obj props
        foreach (var prop in props)
        {
            ProcessProp("", prop, obj);
        }

        return updateValues;

        void ProcessProp(string parentName, PropertyInfo? prop, object? parent)
        {
            if (prop == null)
            {
                return;
            }
            
            var propProps = prop.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (propProps.Length != 0                                  &&
                prop is { CanWrite: true, CanRead: true }              &&
                prop.PropertyType != typeof(string)                    &&
                prop.PropertyType.IsClass                              &&
                !prop.PropertyType.IsAssignableTo(typeof(IEnumerable)) &&
                !prop.PropertyType.IsAssignableTo(typeof(ICollection)))
            {
                parentName = !string.IsNullOrEmpty(parentName) ? $"{parentName}.{prop.Name}" : prop.Name;
                
                var propValue = prop.GetValue(parent);
                
                if (propValue == null)
                {
                    return;
                }
                
                foreach (var propPropsProp in propProps)
                {
                    ProcessProp(parentName, propPropsProp, propValue);
                }
            }
            else
            {
                var value = prop.GetValue(parent);
                
                if (value == null)
                {
                    return;
                }
                
                if (DateTime.TryParse(value.ToString(), out var tryDateTime))
                {
                    if (tryDateTime == DateTime.MinValue || tryDateTime == default)
                    {
                        return;
                    }
                }
                
                switch (value)
                {
                    //Ignore DateTime
                    case DateTime dateTime when dateTime == DateTime.MinValue || dateTime == default:
                    case IEnumerable enumerable when !enumerable.Cast<object>().Any():
                        return;
                }
                
                var propFullName = prop.Name;
                if (!string.IsNullOrEmpty(parentName))
                {
                    propFullName = $"{parentName}.{prop.Name}";
                }
                
                updateValues.Add(propFullName, value);
            }
        }
    }
    
    
    /// <summary>
    ///     Creates a new object from a dictionary of property paths and values
    /// </summary>
    /// <param name="propertyValues"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static T? CreateAndPopulateInstance<T>(this Dictionary<string, object> propertyValues)
    {
        // Create an instance of type T
        var instance = Activator.CreateInstance<T>();
        
        if (instance == null)
        {
            return default;
        }
        
        var sortedProperties = new SortedDictionary<string, object>(propertyValues);
        
        foreach (var (key, value) in sortedProperties)
        {
            var propertyNameParts = key.Split('.');
            object? currentObject = instance;
            var currentType = instance.GetType();
            
            for (var i = 0; i < propertyNameParts.Length; i++)
            {
                var propertyName = propertyNameParts[i].ReplaceDiacritics();
                
                // Get the property info for the current property name
                var propertyInfo = currentType?.GetProperty(propertyName,
                                                            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance |
                                                            BindingFlags.FlattenHierarchy);
                
                // If the property does not exist, skip to the next key-value pair
                if (propertyInfo == null)
                {
                    continue;
                }
                
                // If this is the last part of the property name, set the value of the property
                if (i == propertyNameParts.Length - 1)
                {
                    // If the value is null, skip to the next key-value pair
                    if (value == null)
                    {
                        continue;
                    }
                    
                    // Convert the value to the correct type
                    var convertedValue = Convert.ChangeType(value, propertyInfo.PropertyType);
                    
                    if (convertedValue is DateTime dateTime)
                    {
                        convertedValue = dateTime.SetKindToUtc();
                    }
                    
                    propertyInfo.SetValue(currentObject, convertedValue);
                }
                // Otherwise, get the value of the inner object and update the current object reference
                else
                {
                    var innerObject = propertyInfo.GetValue(currentObject);
                    
                    if (innerObject == null)
                    {
                        innerObject = Activator.CreateInstance(propertyInfo.PropertyType);
                        propertyInfo.SetValue(currentObject, innerObject);
                    }
                    
                    currentObject = innerObject;
                    currentType = currentObject?.GetType();
                    
                    // If the value of the inner object is null, skip to the next key-value pair
                    if (currentObject == null)
                    {
                        break;
                    }
                }
            }
        }
        
        return instance;
    }
    
    public static void CopyValuesFromParent<TParent, TChild>(this TChild child, TParent parent)
        where TParent : class
        where TChild : class, TParent
    {
        var parentType = typeof(TParent);
        var childType = typeof(TChild);
        var parentProperties = parentType.GetProperties();
        
        foreach (var parentProperty in parentProperties)
        {
            var childProperty = childType.GetProperty(parentProperty.Name);
            if (childProperty == null || !childProperty.CanWrite)
            {
                continue;
            }
            
            var parentValue = parentProperty.GetValue(parent);
            childProperty.SetValue(child, parentValue);
        }
    }
}