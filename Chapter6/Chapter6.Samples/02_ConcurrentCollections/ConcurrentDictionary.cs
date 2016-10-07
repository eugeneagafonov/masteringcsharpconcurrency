using System;

namespace Chapter6.Samples._02_ConcurrentCollections
{
    // Class is used only for syntax highlighting purposes!
    public class ConcurrentDictionary1<TKey, TValue>
    {
// Внутренности ConcurrentDictionary<TKey, TValue>
public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
{
    TValue resultingValue;
    if (TryGetValue(key, out resultingValue))
    {
        return resultingValue;
    }
            
    TryAddInternal(key, valueFactory(key), false, true, out resultingValue);
    return resultingValue;
}

/// <summary>
/// Shared internal implementation for inserts and updates.
/// If key exists, we always return false; and if updateIfExists == true we force update with value;
/// If key doesn't exist, we always add value and return true;
/// </summary>
private bool TryAddInternal(TKey key, TValue value, bool updateIfExists, bool acquireLock,
    out TValue resultingValue)
{
    // ... Подробности реализации
    resultingValue = default(TValue);
    return false;
}

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);
            return false;
        }
    }


}