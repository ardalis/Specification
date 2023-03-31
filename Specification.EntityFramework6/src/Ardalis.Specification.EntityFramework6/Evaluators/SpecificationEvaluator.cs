using System;
using System.Collections.Generic;
using System.Linq;

namespace Ardalis.Specification.EntityFramework6
{
  /// <inheritdoc/>
  public class SpecificationEvaluator : ISpecificationEvaluator
  {
    // Will use singleton for default configuration. Yet, it can be instantiated if necessary, with default or provided evaluators.
    public static SpecificationEvaluator Default { get; } = new SpecificationEvaluator();

    private readonly List<IEvaluator> evaluators = new List<IEvaluator>();

    public SpecificationEvaluator()
    {
      this.evaluators.AddRange(new IEvaluator[]
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
      this.evaluators.AddRange(evaluators);
    }

    /// <inheritdoc/>
    public virtual IQueryable<TResult> GetQuery<T, TResult>(IQueryable<T> query, ISpecification<T, TResult> specification) where T : class
    {
      if (specification is null) throw new ArgumentNullException("Specification is required");
      if (specification.Selector is null && specification.SelectManyExpression is null) throw new SelectorNotFoundException();
      if (specification.Selector != null && specification.SelectManyExpression != null) throw new ConcurrentSelectorsException();

      query = GetQuery(query, (ISpecification<T>)specification);

      return specification.Selector != null
        ? query.Select(specification.Selector)
        : query.SelectMany(specification.SelectManyExpression);
    }

    /// <inheritdoc/>
    public virtual IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification, bool evaluateCriteriaOnly = false) where T : class
    {
      if (specification is null) throw new ArgumentNullException("Specification is required");

      var evaluators = evaluateCriteriaOnly ? this.evaluators.Where(x => x.IsCriteriaEvaluator) : this.evaluators;

      foreach (var evaluator in evaluators)
      {
        query = evaluator.GetQuery(query, specification);
      }

      return query;
    }
  }
}
