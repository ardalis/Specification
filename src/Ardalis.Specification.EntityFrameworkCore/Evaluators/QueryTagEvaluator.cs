namespace Ardalis.Specification.EntityFrameworkCore;

public class QueryTagEvaluator : IEvaluator
{
    private QueryTagEvaluator() { }
    public static QueryTagEvaluator Instance { get; } = new QueryTagEvaluator();

    public bool IsCriteriaEvaluator { get; } = true;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        if (specification is Specification<T> spec)
        {
            if (spec.OneOrManyQueryTags.IsEmpty) return query;

            if (spec.OneOrManyQueryTags.HasSingleItem)
            {
                return query.TagWith(spec.OneOrManyQueryTags.Single);
            }

            foreach (var tag in spec.OneOrManyQueryTags.List)
            {
                query = query.TagWith(tag);
            }
            return query;
        }

        foreach (var tag in specification.QueryTags)
        {
            query = query.TagWith(tag);
        }
        return query;
    }
}
