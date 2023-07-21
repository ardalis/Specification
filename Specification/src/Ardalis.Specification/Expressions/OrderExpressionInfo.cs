using System;
using System.Linq.Expressions;

namespace Ardalis.Specification;

/// <summary>
/// Encapsulates data needed to perform sorting.
/// </summary>
/// <typeparam name="T">Type of the entity to apply sort on.</typeparam>
public class OrderExpressionInfo<T>
{
    private readonly Lazy<Func<T, object?>> _keySelectorFunc;

    /// <summary>
    /// Creates instance of <see cref="OrderExpressionInfo{T}" />.
    /// </summary>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="orderType">Whether to (subsequently) sort ascending or descending.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="keySelector"/> is null.</exception>
    public OrderExpressionInfo(Expression<Func<T, object?>> keySelector, OrderTypeEnum orderType)
    {
        _ = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

        KeySelector = keySelector;
        OrderType = orderType;

        _keySelectorFunc = new Lazy<Func<T, object?>>(KeySelector.Compile);
    }

    /// <summary>
    /// A function to extract a key from an element.
    /// </summary>
    public Expression<Func<T, object?>> KeySelector { get; }

    /// <summary>
    /// Whether to (subsequently) sort ascending or descending.
    /// </summary>
    public OrderTypeEnum OrderType { get; }

    /// <summary>
    /// Compiled <see cref="KeySelector" />.
    /// </summary>
    public Func<T, object?> KeySelectorFunc => _keySelectorFunc.Value;
}
