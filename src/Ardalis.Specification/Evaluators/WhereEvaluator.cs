namespace Ardalis.Specification;

public class WhereEvaluator : IEvaluator, IInMemoryEvaluator
{
    private WhereEvaluator() { }
    public static WhereEvaluator Instance { get; } = new WhereEvaluator();

    public bool IsCriteriaEvaluator { get; } = true;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        if (specification is Specification<T> spec)
        {
            if (spec.OneOrManyWhereExpressions.IsEmpty) return query;
            if (spec.OneOrManyWhereExpressions.SingleOrDefault is { } whereExpression)
            {
                return query.Where(whereExpression.Filter);
            }
        }

        foreach (var whereExpression in specification.WhereExpressions)
        {
            query = query.Where(whereExpression.Filter);
        }

        return query;
    }

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification)
    {
        if (specification is Specification<T> spec)
        {
            if (spec.OneOrManyWhereExpressions.IsEmpty) return query;
            if (spec.OneOrManyWhereExpressions.SingleOrDefault is { } whereExpression)
            {
                return query.Where(whereExpression.FilterFunc);
            }
        }

        foreach (var whereExpression in specification.WhereExpressions)
        {
            query = query.Where(whereExpression.FilterFunc);
        }

        return query;
    }
}
