namespace Ardalis.Specification;

internal struct OneOrMany<T>
{
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
            return;
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

    public readonly IEnumerable<T> Values
    {
        get
        {
            if (_value is null)
            {
                return Enumerable.Empty<T>();
            }

            if (_value is List<T> tags)
            {
                return tags;
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
