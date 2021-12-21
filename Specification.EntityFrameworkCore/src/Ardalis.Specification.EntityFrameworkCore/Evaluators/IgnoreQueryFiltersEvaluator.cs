using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ardalis.Specification.EntityFrameworkCore
{
    public class IgnoreQueryFiltersEvaluator : IEvaluator
    {
        private IgnoreQueryFiltersEvaluator() { }
        public static IgnoreQueryFiltersEvaluator Instance { get; } = new IgnoreQueryFiltersEvaluator();

        public bool IsCriteriaEvaluator { get; } = true;

        public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
        {
            if (specification.IgnoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            return query;
        }
    }
}
