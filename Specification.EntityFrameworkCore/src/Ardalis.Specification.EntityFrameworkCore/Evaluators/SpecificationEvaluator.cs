namespace Ardalis.Specification.EntityFrameworkCore;

/// <inheritdoc/>
public class SpecificationEvaluator : ISpecificationEvaluator
{
    // Will use singleton for default configuration. Yet, it can be instantiated if necessary, with default or provided evaluators.
    /// <summary>
    /// <see cref="SpecificationEvaluator" /> instance with default evaluators and without any additional features enabled.
    /// </summary>
    public static SpecificationEvaluator Default { get; } = new SpecificationEvaluator();

    /// <summary>
    /// <see cref="SpecificationEvaluator" /> instance with default evaluators and enabled caching.
    /// </summary>
    public static SpecificationEvaluator Cached { get; } = new SpecificationEvaluator(true);

    protected List<IEvaluator> Evaluators { get; } = new List<IEvaluator>();

    public SpecificationEvaluator(bool cacheEnabled = false)
    {
        Evaluators.AddRange(new IEvaluator[]
        {
            WhereEvaluator.Instance,
            SearchEvaluator.Instance,
            cacheEnabled ? IncludeEvaluator.Cached : IncludeEvaluator.Default,
            OrderEvaluator.Instance,
            PaginationEvaluator.Instance,
            AsNoTrackingEvaluator.Instance,
            AsNoTrackingWithIdentityResolutionEvaluator.Instance,
            AsTrackingEvaluator.Instance,
            IgnoreQueryFiltersEvaluator.Instance,
            AsSplitQueryEvaluator.Instance
        });
    }

    public SpecificationEvaluator(IEnumerable<IEvaluator> evaluators)
    {
        Evaluators.AddRange(evaluators);
    }

    /// <inheritdoc/>
    public virtual IQueryable<TResult> GetQuery<T, TResult>(IQueryable<T> query, ISpecification<T, TResult> specification) where T : class
    {
        if (specification is null) throw new ArgumentNullException(nameof(specification));
        if (specification.Selector is null && specification.SelectorMany is null) throw new SelectorNotFoundException();
        if (specification.Selector is not null && specification.SelectorMany is not null) throw new ConcurrentSelectorsException();

        query = GetQuery(query, (ISpecification<T>)specification);

        return specification.Selector is not null
          ? query.Select(specification.Selector)
          : query.SelectMany(specification.SelectorMany!);
    }

    /// <inheritdoc/>
    public virtual IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification, bool evaluateCriteriaOnly = false) where T : class
    {
        if (specification is null) throw new ArgumentNullException(nameof(specification));

        var evaluators = evaluateCriteriaOnly ? Evaluators.Where(x => x.IsCriteriaEvaluator) : Evaluators;

        foreach (var evaluator in evaluators)
        {
            query = evaluator.GetQuery(query, specification);
        }

        return query;
    }
}
