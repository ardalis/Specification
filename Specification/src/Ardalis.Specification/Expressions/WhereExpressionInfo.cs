using System;
using System.Linq.Expressions;

namespace Ardalis.Specification;

/// <summary>
/// Encapsulates data needed to perform filtering.
/// </summary>
/// <typeparam name="T">Type of the entity to apply filter on.</typeparam>
public class WhereExpressionInfo<T>
{
    private readonly Lazy<Func<T, bool>> _filterFunc;

    /// <summary>
    /// Creates instance of <see cref="WhereExpressionInfo{T}" />.
    /// </summary>
    /// <param name="filter">Condition which should be satisfied by instances of <typeparamref name="T"/>.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="filter"/> is null.</exception>
    public WhereExpressionInfo(Expression<Func<T, bool>> filter)
    {
        _ = filter ?? throw new ArgumentNullException(nameof(filter));

        Filter = filter;

        _filterFunc = new Lazy<Func<T, bool>>(Filter.Compile);
    }

    /// <summary>
    /// Condition which should be satisfied by instances of <typeparamref name="T"/>.
    /// </summary>
    public Expression<Func<T, bool>> Filter { get; }

    /// <summary>
    /// Compiled <see cref="Filter" />.
    /// </summary>
    public Func<T, bool> FilterFunc => _filterFunc.Value;
}
