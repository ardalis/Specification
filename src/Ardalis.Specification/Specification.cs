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
    // The initial value is not important since the value is always initialized by the root of the chain.
    // Therefore, we don't need ThreadLocal (it's more expensive).
    // With this we're saving 8 bytes per include builder, and we don't need an order builder at all (saving 32 bytes per order builder instance).
    [ThreadStatic]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static bool IsChainDiscarded;

    // The state is null initially, but we're spending 8 bytes per reference (on x64).
    // This will be reconsidered for version 10 where we may store the whole state as a single array of structs.
    private OneOrMany<WhereExpressionInfo<T>> _whereExpressions = new();
    private OneOrMany<SearchExpressionInfo<T>> _searchExpressions = new();
    private OneOrMany<OrderExpressionInfo<T>> _orderExpressions = new();
    private OneOrMany<IncludeExpressionInfo> _includeExpressions = new();
    private OneOrMany<string> _includeStrings = new();
    private OneOrMany<string> _queryTags = new();
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
    // Based on the alignment of 8 bytes (on x64) we can store 8 flags here. So, we have space for 2 more flags for free.

    /// <inheritdoc/>
    public bool IgnoreQueryFilters { get; internal set; } = false;

    /// <inheritdoc/>
    public bool IgnoreAutoIncludes { get; internal set; } = false;

    /// <inheritdoc/>
    public bool AsSplitQuery { get; internal set; } = false;

    /// <inheritdoc/>
    public bool AsNoTracking { get; internal set; } = false;

    /// <inheritdoc/>
    public bool AsTracking { get; internal set; } = false;

    /// <inheritdoc/>
    public bool AsNoTrackingWithIdentityResolution { get; internal set; } = false;

    /// <inheritdoc/>
    public Dictionary<string, object> Items => _items ??= [];

    /// <inheritdoc/>
    public IEnumerable<WhereExpressionInfo<T>> WhereExpressions => _whereExpressions.Values;

    /// <inheritdoc/>
    public IEnumerable<SearchExpressionInfo<T>> SearchCriterias => _searchExpressions.Values;

    /// <inheritdoc/>
    public IEnumerable<OrderExpressionInfo<T>> OrderExpressions => _orderExpressions.Values;

    /// <inheritdoc/>
    public IEnumerable<IncludeExpressionInfo> IncludeExpressions => _includeExpressions.Values;

    /// <inheritdoc/>
    public IEnumerable<string> IncludeStrings => _includeStrings.Values;

    /// <inheritdoc/>
    public IEnumerable<string> QueryTags => _queryTags.Values;

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

    internal OneOrMany<WhereExpressionInfo<T>> OneOrManyWhereExpressions => _whereExpressions;
    internal OneOrMany<SearchExpressionInfo<T>> OneOrManySearchExpressions => _searchExpressions;
    internal OneOrMany<OrderExpressionInfo<T>> OneOrManyOrderExpressions => _orderExpressions;
    internal OneOrMany<IncludeExpressionInfo> OneOrManyIncludeExpressions => _includeExpressions;
    internal OneOrMany<string> OneOrManyIncludeStrings => _includeStrings;
    internal OneOrMany<string> OneOrManyQueryTags => _queryTags;

    internal void Add(WhereExpressionInfo<T> whereExpression) => _whereExpressions.Add(whereExpression);
    internal void Add(SearchExpressionInfo<T> searchExpression) => _searchExpressions.AddSorted(searchExpression, SearchExpressionComparer<T>.Default);
    internal void Add(OrderExpressionInfo<T> orderExpression) => _orderExpressions.Add(orderExpression);
    internal void Add(IncludeExpressionInfo includeExpression) => _includeExpressions.Add(includeExpression);
    internal void Add(string includeString) => _includeStrings.Add(includeString);
    internal void AddQueryTag(string queryTag) => _queryTags.Add(queryTag);

    internal Specification<T> Clone()
    {
        var newSpec = new Specification<T>();
        CopyState(this, newSpec);
        return newSpec;
    }

    internal Specification<T, TResult> Clone<TResult>()
    {
        var newSpec = new Specification<T, TResult>();
        CopyState(this, newSpec);
        return newSpec;
    }

    private static void CopyState(Specification<T> source, Specification<T> target)
    {
        target.PostProcessingAction = source.PostProcessingAction;
        target.CacheKey = source.CacheKey;
        target.Take = source.Take;
        target.Skip = source.Skip;
        target.IgnoreAutoIncludes = source.IgnoreAutoIncludes;
        target.IgnoreQueryFilters = source.IgnoreQueryFilters;
        target.AsSplitQuery = source.AsSplitQuery;
        target.AsNoTracking = source.AsNoTracking;
        target.AsTracking = source.AsTracking;
        target.AsNoTrackingWithIdentityResolution = source.AsNoTrackingWithIdentityResolution;
        target._whereExpressions = source._whereExpressions.Clone();
        target._searchExpressions = source._searchExpressions.Clone();
        target._orderExpressions = source._orderExpressions.Clone();
        target._includeExpressions = source._includeExpressions.Clone();
        target._includeStrings = source._includeStrings.Clone();
        target._queryTags = source._queryTags.Clone();
        if (source._items is not null)
        {
            target._items = new Dictionary<string, object>(source._items);
        }
    }
}
