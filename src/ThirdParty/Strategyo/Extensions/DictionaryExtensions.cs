using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Strategyo.Extensions;

public static class DictionaryExtensions
{
    public static TValue? GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue? value = default)
        where TKey : notnull
    {
        ref var val = ref CollectionsMarshal.GetValueRefOrNullRef(dictionary, key);
        return Unsafe.IsNullRef(ref val) ? value : val;
    }
    
    public static TValue? AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue? value)
        where TKey : notnull
    {
        ref var val = ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, key, out _);
        val = value!;
        return val;
    }
    
    public static TValue? GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue? value)
        where TKey : notnull
    {
        ref var val = ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, key, out var exists);

        if (exists)
        {
            return val;
        }

        val = value;
        return value;
    }

    public static bool TryUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        where TKey : notnull
    {
        ref var val = ref CollectionsMarshal.GetValueRefOrNullRef(dictionary, key);

        if (Unsafe.IsNullRef(ref val))
        {
            return false;
        }
        
        val = value;
        return true;
    }
}