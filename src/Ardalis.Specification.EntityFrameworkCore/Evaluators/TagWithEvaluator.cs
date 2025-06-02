namespace Ardalis.Specification.EntityFrameworkCore;

public class TagWithEvaluator : IEvaluator
{
    private TagWithEvaluator() { }
    public static TagWithEvaluator Instance { get; } = new TagWithEvaluator();

    public bool IsCriteriaEvaluator { get; } = true;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        foreach (var tag in specification.QueryTags)
        {
            query = query.TagWith(tag);
        }

        return query;
    }
}
