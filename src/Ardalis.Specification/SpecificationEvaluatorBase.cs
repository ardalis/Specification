using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ardalis.Specification
{
    public abstract class SpecificationEvaluatorBase<T> : ISpecificationEvaluator<T> where T : class
    {
        public virtual IQueryable<TResult> GetQuery<TResult>(IQueryable<T> inputQuery, ISpecification<T, TResult> specification)
        {
            var query = GetQuery(inputQuery, (ISpecification<T>)specification);

            // Apply selector
            var selectQuery = query.Select(specification.Selector);

            return selectQuery;
        }

        public virtual IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = inputQuery;

            if (specification.WhereExpressions.Count() > 0)
            {
                query = specification.WhereExpressions.Aggregate(query,
                                    (current, criteria) => current.Where(criteria));
            }

            // Need to check for null if <Nullable> is enabled.
            if (specification.OrderExpressions != null)
            {
                IOrderedQueryable<T> orderedQuery = null;
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

            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip)
                             .Take(specification.Take);
            }


            //Include expressions will be evaluated within the EF implementation package.

            return query;
        }
    }
}
