namespace Ardalis.Specification;

public class WhereValidator : IValidator
{
    private WhereValidator() { }
    public static WhereValidator Instance { get; } = new WhereValidator();

    public bool IsValid<T>(T entity, ISpecification<T> specification)
    {
        foreach (var info in specification.WhereExpressions)
        {
            if (info.FilterFunc(entity) == false) return false;
        }

        return true;
    }
}
