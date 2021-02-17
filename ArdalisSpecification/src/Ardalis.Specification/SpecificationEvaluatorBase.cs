using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ardalis.Specification
{
    // This class is not used as base by the evaluator in plugin package anymore, since we have to ensure proper order of evaluation.
    // For an example Search should be evaluated before pagination.
    // This base class remains just for legacy reasons and for unit tests.

    /// <inheritdoc/>
    public abstract class SpecificationEvaluatorBase<T> : ISpecificationEvaluator<T> where T : class
    {
        /// <inheritdoc/>
        public virtual IQueryable<TResult> GetQuery<TResult>(IQueryable<T> inputQuery, ISpecification<T, TResult> specification)
        {
            var query = GetQuery(inputQuery, (ISpecification<T>)specification);

            // Apply selector
            var selectQuery = query.Select(specification.Selector);

            return selectQuery;
        }

        /// <inheritdoc/>
        public virtual IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = inputQuery;

            foreach (var criteria in specification.WhereExpressions)
            {
                query = query.Where(criteria);
            }

            // Need to check for null if <Nullable> is enabled.
            if (specification.OrderExpressions != null)
            {
                if (specification.OrderExpressions.Where(x=>x.OrderType == OrderTypeEnum.OrderBy || 
                                                            x.OrderType == OrderTypeEnum.OrderByDescending).Count() > 1)
                {
                    throw new DuplicateOrderChainException();
                }

                IOrderedQueryable<T>? orderedQuery = null;
                foreach (var orderExpression in specification.OrderExpressions)
                {
                    if (orderExpression.OrderType == OrderTypeEnum.OrderBy)
                    {
                        orderedQuery = query.OrderBy(orderExpression.KeySelector);
                    }
                    else if (orderExpression.OrderType == OrderTypeEnum.OrderByDescending)
                    {
                        orderedQuery = query.OrderByDescending(orderExpression.KeySelector);
                    }
                    else if (orderExpression.OrderType == OrderTypeEnum.ThenBy)
                    {
                        orderedQuery = orderedQuery.ThenBy(orderExpression.KeySelector);
                    }
                    else if (orderExpression.OrderType == OrderTypeEnum.ThenByDescending)
                    {
                        orderedQuery = orderedQuery.ThenByDescending(orderExpression.KeySelector);
                    }
                }

                if (orderedQuery != null)
                {
                    query = orderedQuery;
                }
            }

            // If skip is 0, avoid adding to the IQueryable. It will generate more optimized SQL that way.
            if (specification.Skip != null && specification.Skip != 0)
            {
                query = query.Skip(specification.Skip.Value);
            }

            if (specification.Take != null)
            {
                query = query.Take(specification.Take.Value);
            }


            //Include expressions will be evaluated within the EF implementation package.

            return query;
        }
    }
}
