namespace Ardalis.Specification.EntityFrameworkCore;

/// <inheritdoc/>
public class SpecificationEvaluator : ISpecificationEvaluator
{
    // Will use singleton for default configuration. Yet, it can be instantiated if necessary, with default or provided evaluators.

    /// <summary>
    /// <see cref="SpecificationEvaluator" /> instance with default evaluators..
    /// </summary>
    public static SpecificationEvaluator Default { get; } = new SpecificationEvaluator();

    protected List<IEvaluator> Evaluators { get; }

    public SpecificationEvaluator()
    {
        Evaluators =
        [
            WhereEvaluator.Instance,
            SearchEvaluator.Instance,
            IncludeStringEvaluator.Instance,
            IncludeEvaluator.Instance,
            OrderEvaluator.Instance,
            PaginationEvaluator.Instance,
            AsNoTrackingEvaluator.Instance,
            AsNoTrackingWithIdentityResolutionEvaluator.Instance,
            AsTrackingEvaluator.Instance,
            IgnoreQueryFiltersEvaluator.Instance,
            IgnoreAutoIncludesEvaluator.Instance,
            AsSplitQueryEvaluator.Instance,
            TagWithEvaluator.Instance,
        ];
    }

    public SpecificationEvaluator(IEnumerable<IEvaluator> evaluators)
    {
        Evaluators = evaluators.ToList();
    }

    /// <inheritdoc/>
    public virtual IQueryable<TResult> GetQuery<T, TResult>(IQueryable<T> query, ISpecification<T, TResult> specification) where T : class
    {
        if (specification is null) throw new ArgumentNullException(nameof(specification));
        if (specification.Selector is null && specification.SelectorMany is null && specification.SelectorFunc is null) throw new SelectorNotFoundException();
        if ((specification.Selector is not null || specification.SelectorFunc is not null) && specification.SelectorMany is not null) throw new ConcurrentSelectorsException();
        if (specification.Selector is not null && specification.SelectorFunc is not null) throw new DuplicateSelectorsException();

        query = GetQuery(query, (ISpecification<T>)specification);

        return specification.Selector is not null
            ? query.Select(specification.Selector)
            : specification.SelectorMany != null
                ? query.SelectMany(specification.SelectorMany)
                : specification.SelectorFunc!(query);
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
