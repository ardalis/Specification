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
        }

        foreach (var whereExpression in specification.WhereExpressions)
        {
            if (whereExpression.FilterFunc(entity) == false) return false;
        }

        return true;
    }
}
