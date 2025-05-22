using System.Reflection;
using Strategyo.Extensions;

namespace Strategyo.Enumerations.SmartEnum.Base;

public abstract class SmartEnum<TEnum>(int value, string name) : IEquatable<SmartEnum<TEnum>>
    where TEnum : SmartEnum<TEnum>
{
    protected static readonly Dictionary<int, TEnum> Enumerations = CreateEnumerations();

    public int Value { get; } = value;

    public string Name { get; } = name;

    public static TEnum? FromValue(int value) 
        => Enumerations.GetValue(
            value);

    public static TEnum? FromName(string name)
        => Enumerations
          .Values
          .SingleOrDefault(e => e.Name == name);

    public bool Equals(SmartEnum<TEnum>? other)
    {
        if (other is null)
        {
            return false;
        }

        return GetType() == other.GetType() &&
               Value     == other.Value;
    }

    public override bool Equals(object? obj) 
        => obj is SmartEnum<TEnum> other &&
           Equals(other);

    public override int GetHashCode()
        => Value.GetHashCode();

    public override string ToString()
        => Name;

    private static Dictionary<int, TEnum> CreateEnumerations()
    {
        var enumerationType = typeof(TEnum);

        var fieldsForType = enumerationType
                           .GetFields(
                                BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.FlattenHierarchy)
                           .Where(fieldInfo =>
                                      enumerationType.IsAssignableFrom(fieldInfo.FieldType))
                           .Select(fieldInfo =>
                                       (TEnum)fieldInfo.GetValue(null)!);

        return fieldsForType.ToDictionary(x => x.Value);
    }
}