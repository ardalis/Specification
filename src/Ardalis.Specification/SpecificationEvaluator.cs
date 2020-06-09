using System.Linq;
using System.Threading.Tasks;

namespace Ardalis.Specification
{
    public class SpecificationEvaluator<T> where T : class
    {
        public static Task<IQueryable<T>> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
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

            //TODO: Breaking changes in EF Core 3.0 have made this difficult. Need to re-think how this is done.
            //if (specification.GroupBy != null)
            //{
               
            //}

            // Apply paging if enabled
            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip)
                             .Take(specification.Take);
            }
            return Task.FromResult(query);
        }
    }
}
