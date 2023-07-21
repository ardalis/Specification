using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Ardalis.Specification.EntityFrameworkCore;

/// <summary>
/// A thread-safe dictionary for read-heavy workloads.
/// </summary>
/// <typeparam name="TKey">The key type.</typeparam>
/// <typeparam name="TValue">The value type.</typeparam>
internal class CachedReadConcurrentDictionary<TKey, TValue> : IDictionary<TKey, TValue> where TKey : notnull
{
    /// <summary>
    /// The number of cache misses which are tolerated before the cache is regenerated.
    /// </summary>
    private const int _cacheMissesBeforeCaching = 10;
    private readonly ConcurrentDictionary<TKey, TValue> _dictionary;
    private readonly IEqualityComparer<TKey>? _comparer;

    /// <summary>
    /// Approximate number of reads which did not hit the cache since it was last invalidated.
    /// This is used as a heuristic that the dictionary is not being modified frequently with respect to the read volume.
    /// </summary>
    private int _cacheMissReads;

    /// <summary>
    /// Cached version of <see cref="_dictionary"/>.
    /// </summary>
    private Dictionary<TKey, TValue>? _readCache;

    /// <summary>
    /// Initializes a new instance of the <see cref="CachedReadConcurrentDictionary{TKey,TValue}"/> class.
    /// </summary>
    public CachedReadConcurrentDictionary()
    {
        _dictionary = new ConcurrentDictionary<TKey, TValue>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CachedReadConcurrentDictionary{TKey,TValue}"/> class
    /// that contains elements copied from the specified collection.
    /// </summary>
    /// <param name="collection">
    /// The <see cref="T:IEnumerable{KeyValuePair{TKey,TValue}}"/> whose elements are copied to the new instance.
    /// </param>
    public CachedReadConcurrentDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
    {
        _dictionary = new ConcurrentDictionary<TKey, TValue>(collection);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CachedReadConcurrentDictionary{TKey,TValue}"/> class
    /// that contains elements copied from the specified collection and uses the specified
    /// <see cref="T:System.Collections.Generic.IEqualityComparer{TKey}"/>.
    /// </summary>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{TKey}"/> implementation to use when comparing keys.
    /// </param>
    public CachedReadConcurrentDictionary(IEqualityComparer<TKey> comparer)
    {
        _comparer = comparer;
        _dictionary = new ConcurrentDictionary<TKey, TValue>(comparer);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CachedReadConcurrentDictionary{TKey,TValue}"/>
    /// class that contains elements copied from the specified collection and uses the specified
    /// <see cref="T:System.Collections.Generic.IEqualityComparer{TKey}"/>.
    /// </summary>
    /// <param name="collection">
    /// The <see cref="T:IEnumerable{KeyValuePair{TKey,TValue}}"/> whose elements are copied to the new instance.
    /// </param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{TKey}"/> implementation to use when comparing keys.
    /// </param>
    public CachedReadConcurrentDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer)
    {
        _comparer = comparer;
        _dictionary = new ConcurrentDictionary<TKey, TValue>(collection, comparer);
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => GetReadDictionary().GetEnumerator();

    /// <inheritdoc />
    public void Add(KeyValuePair<TKey, TValue> item)
    {
        ((IDictionary<TKey, TValue>)_dictionary).Add(item);
        InvalidateCache();
    }

    /// <inheritdoc />
    public void Clear()
    {
        _dictionary.Clear();
        InvalidateCache();
    }

    /// <inheritdoc />
    public bool Contains(KeyValuePair<TKey, TValue> item) => GetReadDictionary().Contains(item);

    /// <inheritdoc />
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        GetReadDictionary().CopyTo(array, arrayIndex);
    }

    /// <inheritdoc />
    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        var result = ((IDictionary<TKey, TValue>)_dictionary).Remove(item);
        if (result) InvalidateCache();
        return result;
    }

    /// <inheritdoc />
    public int Count => GetReadDictionary().Count;

    /// <inheritdoc />
    public bool IsReadOnly => false;

    /// <inheritdoc />
    public void Add(TKey key, TValue value)
    {
        ((IDictionary<TKey, TValue>)_dictionary).Add(key, value);
        InvalidateCache();
    }

    /// <summary>
    /// Adds a key/value pair to the <see cref="CachedReadConcurrentDictionary{TKey,TValue}"/> if the key does not exist.
    /// </summary>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="valueFactory">The function used to generate a value for the key</param>
    /// <returns>The value for the key. This will be either the existing value for the key if the key is already in the dictionary, or the new value if the key was not in the dictionary.</returns>
    public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
    {
        if (GetReadDictionary().TryGetValue(key, out var value))
        {
            return value;
        }

        value = _dictionary.GetOrAdd(key, valueFactory);
        InvalidateCache();

        return value;
    }

    /// <summary>
    /// Attempts to add the specified key and value.
    /// </summary>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="value">The value of the element to add. The value can be a null reference (Nothing
    /// in Visual Basic) for reference types.</param>
    /// <returns>true if the key/value pair was added successfully; otherwise, false.</returns>
    public bool TryAdd(TKey key, TValue value)
    {
        if (_dictionary.TryAdd(key, value))
        {
            InvalidateCache();
            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public bool ContainsKey(TKey key) => GetReadDictionary().ContainsKey(key);

    /// <inheritdoc />
    public bool Remove(TKey key)
    {
        var result = ((IDictionary<TKey, TValue>)_dictionary).Remove(key);
        if (result) InvalidateCache();
        return result;
    }

    /// <inheritdoc />
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => GetReadDictionary().TryGetValue(key, out value);

    /// <inheritdoc />
    public TValue this[TKey key]
    {
        get => GetReadDictionary()[key];
        set
        {
            _dictionary[key] = value;
            InvalidateCache();
        }
    }

    /// <inheritdoc />
    public ICollection<TKey> Keys => GetReadDictionary().Keys;

    /// <inheritdoc />
    public ICollection<TValue> Values => GetReadDictionary().Values;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IDictionary<TKey, TValue> GetReadDictionary() => _readCache ?? GetWithoutCache();

    private IDictionary<TKey, TValue> GetWithoutCache()
    {
        // If the dictionary was recently modified or the cache is being recomputed, return the dictionary directly.
        if (Interlocked.Increment(ref _cacheMissReads) < _cacheMissesBeforeCaching)
        {
            return _dictionary;
        }

        // Recompute the cache if too many cache misses have occurred.
        _cacheMissReads = 0;
        return _readCache = new Dictionary<TKey, TValue>(_dictionary, _comparer);
    }

    private void InvalidateCache()
    {
        _cacheMissReads = 0;
        _readCache = null;
    }
}
