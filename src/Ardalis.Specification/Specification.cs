using System.ComponentModel;

namespace Ardalis.Specification;

/// <inheritdoc cref="ISpecification{T, TResult}"/>
public class Specification<T, TResult> : Specification<T>, ISpecification<T, TResult>
{
    public new ISpecificationBuilder<T, TResult> Query => new SpecificationBuilder<T, TResult>(this);

    /// <inheritdoc/>
    public Expression<Func<T, TResult>>? Selector { get; internal set; }

    /// <inheritdoc/>
    public Expression<Func<T, IEnumerable<TResult>>>? SelectorMany { get; internal set; }

    /// <inheritdoc/>
    public new Func<IEnumerable<TResult>, IEnumerable<TResult>>? PostProcessingAction { get; internal set; } = null;

    public new virtual IEnumerable<TResult> Evaluate(IEnumerable<T> entities)
    {
        var evaluator = Evaluator;
        return evaluator.Evaluate(entities, this);
    }
}

/// <inheritdoc cref="ISpecification{T}"/>
public class Specification<T> : ISpecification<T>
{
    // It is utilized only during the building stage for the sub-chains. Once the state is built, we don't care about it anymore.
    // The initial value is not important since the value is always initialized by the root of the chain. Therefore, we don't need ThreadLocal (it's more expensive).
    // With this we're saving 8 bytes per include builder, and we don't need an order builder at all (saving 32 bytes per order builder instance).
    [ThreadStatic]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static bool IsChainDiscarded;

    // The state is null initially, but we're spending 8 bytes per reference (on x64).
    // This will be reconsidered for version 10 where we may store the whole state as a single array of structs.
    private List<WhereExpressionInfo<T>>? _whereExpressions;
    private List<SearchExpressionInfo<T>>? _searchExpressions;
    private List<OrderExpressionInfo<T>>? _orderExpressions;
    private List<IncludeExpressionInfo>? _includeExpressions;
    private List<string>? _includeStrings;
    private Dictionary<string, object>? _items;

    public ISpecificationBuilder<T> Query => new SpecificationBuilder<T>(this);
    protected virtual IInMemorySpecificationEvaluator Evaluator => InMemorySpecificationEvaluator.Default;
    protected virtual ISpecificationValidator Validator => SpecificationValidator.Default;

    /// <inheritdoc/>
    public Func<IEnumerable<T>, IEnumerable<T>>? PostProcessingAction { get; internal set; }

    /// <inheritdoc/>
    public string? CacheKey { get; internal set; }

    /// <inheritdoc/>
    public bool CacheEnabled => CacheKey is not null;

    /// <inheritdoc/>
    public int Take { get; internal set; } = -1;

    /// <inheritdoc/>
    public int Skip { get; internal set; } = -1;


    // We may store all the flags in a single byte. But, based on the object alignment of 8 bytes, we won't save any space anyway.
    // And we'll have unnecessary overhead with enum flags for now. This will be reconsidered for version 10.
    // Based on the alignment of 8 bytes (on x64) we can store 8 flags here. So, we have space for 3 more flags for free.

    /// <inheritdoc/>
    public bool IgnoreQueryFilters { get; internal set; } = false;

    /// <inheritdoc/>
    public bool AsSplitQuery { get; internal set; } = false;

    /// <inheritdoc/>
    public bool AsNoTracking { get; internal set; } = false;

    /// <inheritdoc/>
    public bool AsTracking { get; internal set; } = false;

    /// <inheritdoc/>
    public bool AsNoTrackingWithIdentityResolution { get; internal set; } = false;


    // Specs are not intended to be thread-safe, so we don't need to worry about thread-safety here.
    internal void Add(WhereExpressionInfo<T> whereExpression) => (_whereExpressions ??= new(2)).Add(whereExpression);
    internal void Add(OrderExpressionInfo<T> orderExpression) => (_orderExpressions ??= new(2)).Add(orderExpression);
    internal void Add(IncludeExpressionInfo includeExpression) => (_includeExpressions ??= new(2)).Add(includeExpression);
    internal void Add(string includeString) => (_includeStrings ??= new(1)).Add(includeString);
    internal void Add(SearchExpressionInfo<T> searchExpression)
    {
        if (_searchExpressions is null)
        {
            _searchExpressions = new(2) { searchExpression };
            return;
        }

        // We'll keep the search expressions sorted by the search group.
        // We could keep the state as SortedList instead of List, but it has additional 56 bytes overhead and it's not worth it for our use-case.
        // Having multiple search groups is not a common scenario, and usually there may be just few search expressions.
        var index = _searchExpressions.FindIndex(x => x.SearchGroup > searchExpression.SearchGroup);
        if (index == -1)
        {
            _searchExpressions.Add(searchExpression);
        }
        else
        {
            _searchExpressions.Insert(index, searchExpression);
        }
    }

    /// <inheritdoc/>
    public Dictionary<string, object> Items => _items ??= [];

    /// <inheritdoc/>
    public IEnumerable<WhereExpressionInfo<T>> WhereExpressions => _whereExpressions ?? Enumerable.Empty<WhereExpressionInfo<T>>();

    /// <inheritdoc/>
    public IEnumerable<SearchExpressionInfo<T>> SearchCriterias => _searchExpressions ?? Enumerable.Empty<SearchExpressionInfo<T>>();

    /// <inheritdoc/>
    public IEnumerable<OrderExpressionInfo<T>> OrderExpressions => _orderExpressions ?? Enumerable.Empty<OrderExpressionInfo<T>>();

    /// <inheritdoc/>
    public IEnumerable<IncludeExpressionInfo> IncludeExpressions => _includeExpressions ?? Enumerable.Empty<IncludeExpressionInfo>();

    /// <inheritdoc/>
    public IEnumerable<string> IncludeStrings => _includeStrings ?? Enumerable.Empty<string>();

    /// <inheritdoc/>
    public virtual IEnumerable<T> Evaluate(IEnumerable<T> entities)
    {
        var evaluator = Evaluator;
        return evaluator.Evaluate(entities, this);
    }

    /// <inheritdoc/>
    public virtual bool IsSatisfiedBy(T entity)
    {
        var validator = Validator;
        return validator.IsValid(entity, this);
    }
}
