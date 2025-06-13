namespace Ardalis.Specification;

internal struct OneOrMany<T> where T : class
{
    private const int DEFAULT_CAPACITY = 2;
    private object? _value;

    public readonly bool IsEmpty => _value is null;
    public readonly bool HasSingleItem => _value is T;

    public void Add(T item)
    {
        if (_value is null)
        {
            _value = item;
            return;
        }

        if (_value is List<T> list)
        {
            list.Add(item);
            return;
        }

        if (_value is T singleValue)
        {
            _value = new List<T>(2) { singleValue, item };
        }
    }

    public void AddSorted(T item, IComparer<T> comparer)
    {
        if (_value is null)
        {
            _value = item;
            return;
        }

        if (comparer is null)
        {
            throw new ArgumentNullException(nameof(comparer), "Comparer cannot be null.");
        }

        if (_value is List<T> list)
        {
            var index = list.FindIndex(x => comparer.Compare(item, x) < 0);
            if (index == -1)
            {
                list.Add(item);
            }
            else
            {
                list.Insert(index, item);
            }
            return;
        }

        if (_value is T singleValue)
        {
            if (comparer.Compare(item, singleValue) < 0)
            {
                _value = new List<T>(DEFAULT_CAPACITY) { item, singleValue };
            }
            else
            {
                _value = new List<T>(DEFAULT_CAPACITY) { singleValue, item };
            }
        }
    }

    public readonly T Single
    {
        get
        {
            if (_value is T singleValue)
            {
                return singleValue;
            }

            throw new InvalidOperationException("The value is not a single item.");
        }
    }

    public readonly T? SingleOrDefault
    {
        get
        {
            if (_value is T singleValue)
            {
                return singleValue;
            }

            return null;
        }
    }

    public readonly IEnumerable<T> Values
    {
        get
        {
            if (_value is null)
            {
                return Enumerable.Empty<T>();
            }

            if (_value is List<T> list)
            {
                return list;
            }

            if (_value is T singleValue)
            {
                return new[] { singleValue };
            }

            throw new InvalidOperationException("The value is neither a single item nor a list of items.");
        }
    }

    public readonly OneOrMany<T> Clone()
    {
        var clone = new OneOrMany<T>();

        if (_value is T singleValue)
        {
            clone._value = singleValue;
        }
        else if (_value is List<T> list)
        {
            clone._value = list.ToList();
        }

        return clone;
    }
}
