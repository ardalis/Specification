namespace Ardalis.Specification;

public class SpecificationBuilder<T, TResult>(Specification<T, TResult> specification) : SpecificationBuilder<T>(specification), ISpecificationBuilder<T, TResult>
{
    public new Specification<T, TResult> Specification { get; } = specification;
}

public class SpecificationBuilder<T>(Specification<T> specification) : ISpecificationBuilder<T>
{
    public Specification<T> Specification { get; } = specification;
}
