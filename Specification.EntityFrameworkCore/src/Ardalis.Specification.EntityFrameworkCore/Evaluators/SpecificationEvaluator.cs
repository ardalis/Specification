using System.Collections.Generic;
using System.Linq;

namespace Ardalis.Specification.EntityFrameworkCore
{
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

        private readonly List<IEvaluator> evaluators = new List<IEvaluator>();

        public SpecificationEvaluator(bool cacheEnabled = false)
        {
            this.evaluators.AddRange(new IEvaluator[]
            {
                WhereEvaluator.Instance,
                SearchEvaluator.Instance,
                cacheEnabled ? IncludeEvaluator.Cached : IncludeEvaluator.Default,
                OrderEvaluator.Instance,
                PaginationEvaluator.Instance,
                AsNoTrackingEvaluator.Instance,
                IgnoreQueryFiltersEvaluator.Instance,
#if NETSTANDARD2_1
                AsSplitQueryEvaluator.Instance,
                AsNoTrackingWithIdentityResolutionEvaluator.Instance
#endif
            });
        }

        public SpecificationEvaluator(IEnumerable<IEvaluator> evaluators)
        {
            this.evaluators.AddRange(evaluators);
        }

        /// <inheritdoc/>
        public virtual IQueryable<TResult> GetQuery<T, TResult>(IQueryable<T> query, ISpecification<T, TResult> specification) where T : class
        {
            query = GetQuery(query, (ISpecification<T>)specification);

            return query.Select(specification.Selector);
        }

        /// <inheritdoc/>
        public virtual IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification, bool evaluateCriteriaOnly = false) where T : class
        {
            var evaluators = evaluateCriteriaOnly ? this.evaluators.Where(x => x.IsCriteriaEvaluator) : this.evaluators;

            foreach (var evaluator in evaluators)
            {
                query = evaluator.GetQuery(query, specification);
            }

            return query;
        }
    }
}
