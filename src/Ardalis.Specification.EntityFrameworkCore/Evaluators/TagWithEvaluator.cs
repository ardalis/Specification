namespace Ardalis.Specification.EntityFrameworkCore;

public class TagWithEvaluator : IEvaluator
{
    private TagWithEvaluator() { }
    public static TagWithEvaluator Instance { get; } = new TagWithEvaluator();

    public bool IsCriteriaEvaluator { get; } = true;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        if (specification.OneOrManyQueryTags.IsEmpty) return query;

        if (specification.OneOrManyQueryTags.IsSingle)
        {
            query = query.TagWith(specification.OneOrManyQueryTags.Single);
        }
        else
        {
            foreach (var tag in specification.OneOrManyQueryTags.Values)
            {
                query = query.TagWith(tag);
            }
        }

        return query;
    }
}
