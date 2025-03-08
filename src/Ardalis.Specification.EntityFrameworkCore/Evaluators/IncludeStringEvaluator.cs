namespace Ardalis.Specification.EntityFrameworkCore;

public sealed class IncludeStringEvaluator : IEvaluator
{
    private IncludeStringEvaluator() { }
    public static IncludeStringEvaluator Instance { get; } = new();

    public bool IsCriteriaEvaluator => false;

    /// <inheritdoc/>
    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        foreach (var includeString in specification.IncludeStrings)
        {
            query = query.Include(includeString);
        }

        return query;
    }
}
