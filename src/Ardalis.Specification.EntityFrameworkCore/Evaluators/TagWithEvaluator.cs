namespace Ardalis.Specification.EntityFrameworkCore;

public class TagWithEvaluator : IEvaluator
{
    private TagWithEvaluator() { }
    public static TagWithEvaluator Instance { get; } = new TagWithEvaluator();

    public bool IsCriteriaEvaluator { get; } = true;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        if (specification is Specification<T> spec)
        {
            if (spec.OneOrManyQueryTags.IsEmpty) return query;

            if (spec.OneOrManyQueryTags.HasSingleItem)
            {
                query = query.TagWith(spec.OneOrManyQueryTags.Single);
                return query;
            }
        }

        foreach (var tag in specification.QueryTags)
        {
            query = query.TagWith(tag);
        }

        return query;
    }
}
