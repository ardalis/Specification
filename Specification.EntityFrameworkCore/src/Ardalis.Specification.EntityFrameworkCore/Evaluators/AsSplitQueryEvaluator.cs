using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ardalis.Specification.EntityFrameworkCore
{
#if NETSTANDARD2_1
    public class AsSplitQueryEvaluator : IEvaluator
    {
        private AsSplitQueryEvaluator() { }
        public static AsSplitQueryEvaluator Instance { get; } = new AsSplitQueryEvaluator();

        public bool IsCriteriaEvaluator { get; } = true;

        public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
        {
            if (specification.AsSplitQuery)
            {
                query = query.AsSplitQuery();
            }

            return query;
        }
    }
#endif
}
