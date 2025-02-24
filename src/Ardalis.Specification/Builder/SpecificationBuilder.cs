namespace Ardalis.Specification;

public class SpecificationBuilder<T, TResult> : SpecificationBuilder<T>, ISpecificationBuilder<T, TResult>
{
    public new Specification<T, TResult> Specification { get; }

    public SpecificationBuilder(Specification<T, TResult> specification)
        : base(specification)
    {
        Specification = specification;
    }
}

public class SpecificationBuilder<T> : ISpecificationBuilder<T>
{
    public Specification<T> Specification { get; }

    public SpecificationBuilder(Specification<T> specification)
    {
        Specification = specification;
    }
}
