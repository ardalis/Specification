using System.Collections.Generic;
using System.Linq;

namespace Ardalis.Specification;

public class WhereEvaluator : IEvaluator, IInMemoryEvaluator
{
    private WhereEvaluator() { }
    public static WhereEvaluator Instance { get; } = new WhereEvaluator();

    public bool IsCriteriaEvaluator { get; } = true;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        foreach (var info in specification.WhereExpressions)
        {
            query = query.Where(info.Filter);
        }

        return query;
    }

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification)
    {
        foreach (var info in specification.WhereExpressions)
        {
            query = query.Where(info.FilterFunc);
        }

        return query;
    }
}
