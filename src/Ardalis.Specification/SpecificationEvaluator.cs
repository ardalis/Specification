using System.Linq;

namespace Ardalis.Specification
{
    public class SpecificationEvaluator<T, TId> where T : class, IEntity<TId>
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = inputQuery;

            // modify the IQueryable using the specification's criteria expression
            if (specification.Criterias.Count() > 0)
            {
                query = specification.Criterias.Aggregate(query,
                                    (current, criteria) => current.Where(criteria));
            }

            // Apply ordering if expressions are set
            if (specification.OrderBy != null && specification.ThenBy != null)
            {
                query = query.OrderBy(specification.OrderBy).ThenBy(specification.ThenBy);
            }
            else if (specification.OrderByDescending != null && specification.ThenBy != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending).ThenBy(specification.ThenBy);
            }
            else if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.GroupBy != null)
            {
                query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
            }

            // Apply paging if enabled
            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip)
                             .Take(specification.Take);
            }
            return query;
        }
    }
}
