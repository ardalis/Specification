using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ardalis.Specification
{
    public interface IEvaluator
    {
        bool IsCriteriaEvaluator { get; }

        IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class;
    }
}
