using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Ardalis.Specification
{
    public class EfSpecificationEvaluator<T, TId> where T : class, IEntity<TId>
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = inputQuery;

            query = SpecificationEvaluator<T, TId>.GetQuery(query, specification);

            // Includes all expression-based includes
            query = specification.Includes.Aggregate(query,
                                    (current, include) => current.Include(include));

            // Include any string-based include statements
            query = specification.IncludeStrings.Aggregate(query,
                                    (current, include) => current.Include(include));

            // Apply selector
            // query = query.Select(specification.Selector);

            return query;
        }
    }
}
