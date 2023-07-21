using System;
using System.Collections.Generic;
using System.Linq;

namespace Ardalis.Specification.EntityFramework6;

/// <inheritdoc/>
public class SpecificationEvaluator : ISpecificationEvaluator
{
    // Will use singleton for default configuration. Yet, it can be instantiated if necessary, with default or provided evaluators.
    public static SpecificationEvaluator Default { get; } = new SpecificationEvaluator();

    protected List<IEvaluator> Evaluators { get; } = new List<IEvaluator>();

    public SpecificationEvaluator()
    {
        Evaluators.AddRange(new IEvaluator[]
        {
            WhereEvaluator.Instance,
            SearchEvaluator.Instance,
            IncludeEvaluator.Instance,
            OrderEvaluator.Instance,
            PaginationEvaluator.Instance,
            AsNoTrackingEvaluator.Instance
        });
    }
    public SpecificationEvaluator(IEnumerable<IEvaluator> evaluators)
    {
        Evaluators.AddRange(evaluators);
    }

    /// <inheritdoc/>
    public virtual IQueryable<TResult> GetQuery<T, TResult>(IQueryable<T> query, ISpecification<T, TResult> specification) where T : class
    {
        if (specification is null) throw new ArgumentNullException("Specification is required");
        if (specification.Selector is null && specification.SelectorMany is null) throw new SelectorNotFoundException();
        if (specification.Selector != null && specification.SelectorMany != null) throw new ConcurrentSelectorsException();

        query = GetQuery(query, (ISpecification<T>)specification);

        return specification.Selector != null
          ? query.Select(specification.Selector)
          : query.SelectMany(specification.SelectorMany);
    }

    /// <inheritdoc/>
    public virtual IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification, bool evaluateCriteriaOnly = false) where T : class
    {
        if (specification is null) throw new ArgumentNullException("Specification is required");

        var evaluators = evaluateCriteriaOnly ? Evaluators.Where(x => x.IsCriteriaEvaluator) : Evaluators;

        foreach (var evaluator in evaluators)
        {
            query = evaluator.GetQuery(query, specification);
        }

        return query;
    }
}
