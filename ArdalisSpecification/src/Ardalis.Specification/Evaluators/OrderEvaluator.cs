using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ardalis.Specification
{
    public class WhereEvaluator : IEvaluator
    {
        private WhereEvaluator() { }
        public static WhereEvaluator Instance { get; } = new WhereEvaluator();

        public bool IsCriteriaEvaluator { get; } = true;

        public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
        {
            foreach (var criteria in specification.WhereExpressions)
            {
                query = query.Where(criteria);
            }

            return query;
        }
    }
}
