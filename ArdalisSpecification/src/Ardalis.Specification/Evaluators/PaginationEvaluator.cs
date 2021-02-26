using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ardalis.Specification
{
    public class PaginationEvaluator : IEvaluator
    {
        private PaginationEvaluator() { }
        public static PaginationEvaluator Instance { get; } = new PaginationEvaluator();

        public bool IsCriteriaEvaluator { get; } = false;

        public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
        {
            // If skip is 0, avoid adding to the IQueryable. It will generate more optimized SQL that way.
            if (specification.Skip != null && specification.Skip != 0)
            {
                query = query.Skip(specification.Skip.Value);
            }

            if (specification.Take != null)
            {
                query = query.Take(specification.Take.Value);
            }

            return query;
        }
    }
}
