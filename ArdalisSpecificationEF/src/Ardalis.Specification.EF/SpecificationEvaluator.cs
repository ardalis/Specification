using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Ardalis.Specification.EF
{
    public class SpecificationEvaluator<T> : SpecificationEvaluatorBase<T> where T : class
    {
        public override IQueryable<TResult> GetQuery<TResult>(IQueryable<T> inputQuery, ISpecification<T, TResult> specification)
        {
            var query = GetQuery(inputQuery, (ISpecification<T>)specification);

            var selectQuery = query.Select(specification.Selector);

            return selectQuery;
        }

        public override IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = base.GetQuery(inputQuery, specification);

            query = specification.IncludeStrings.Aggregate(query,
                        (current, includeString) => current.Include(includeString));

            foreach (var includeAggregator in specification.IncludeAggregators)
            {
                var includeString = includeAggregator.IncludeString;
                if (!string.IsNullOrEmpty(includeString))
                {
                    query = query.Include(includeString);
                }
            }

            return query;
        }
    }
}
