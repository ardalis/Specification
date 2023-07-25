using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;

namespace Ardalis.Sample.App3;

public class QueryTagEvaluator : IEvaluator
{
    private QueryTagEvaluator() { }
    public static QueryTagEvaluator Instance { get; } = new QueryTagEvaluator();

    public bool IsCriteriaEvaluator { get; } = true;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        if (specification.Items.TryGetValue("TagWith", out var value) && value is string tag)
        {
            query = query.TagWith(tag);
        }

        return query;
    }
}
