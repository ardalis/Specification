using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Ardalis.Specification.EntityFrameworkCore
{
    /// <inheritdoc/>
    public class SpecificationEvaluator<T> : ISpecificationEvaluator<T> where T : class
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

            foreach (var searchCriteria in specification.SearchCriterias.GroupBy(x => x.SearchGroup))
            {
                var criterias = searchCriteria.Select(x => (x.Selector, x.SearchTerm));
                query = query.Search(criterias);
            }

            foreach (var includeString in specification.IncludeStrings)
            {
                query = query.Include(includeString);
            }

            foreach (var includeInfo in specification.IncludeExpressions)
            {
                if (includeInfo.Type == IncludeTypeEnum.Include)
                {
                    query = query.Include(includeInfo);
                }
                else if (includeInfo.Type == IncludeTypeEnum.ThenInclude)
                {
                    query = query.ThenInclude(includeInfo);
                }
            }

            // Need to check for null if <Nullable> is enabled.
            if (specification.OrderExpressions != null && specification.OrderExpressions.Count() > 0)
            {
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

                    if (orderedQuery != null)
                    {
                        query = orderedQuery;
                    }
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

            if (specification.AsNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }
    }
}
