using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ardalis.Specification.EntityFrameworkCore
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
                AsSplitQueryEvaluator.Instance,
                AsNoTrackingEvaluator.Instance,
                AsNoTrackingWithIdentityResolutionEvaluator.Instance
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
