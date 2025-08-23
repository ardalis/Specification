namespace Ardalis.Specification;

public class WhereValidator : IValidator
{
    private WhereValidator() { }
    public static WhereValidator Instance { get; } = new WhereValidator();

    public bool IsValid<T>(T entity, ISpecification<T> specification)
    {
        if (specification is Specification<T> spec)
        {
            if (spec.OneOrManyWhereExpressions.IsEmpty) return true;
            if (spec.OneOrManyWhereExpressions.SingleOrDefault is { } whereExpression)
            {
                return whereExpression.FilterFunc(entity);
            }

            foreach (var whereExpr in spec.OneOrManyWhereExpressions.List)
            {
                if (whereExpr.FilterFunc(entity) == false) return false;
            }
            return true;
        }

        foreach (var whereExpr in specification.WhereExpressions)
        {
            if (whereExpr.FilterFunc(entity) == false) return false;
        }
        return true;
    }
}
