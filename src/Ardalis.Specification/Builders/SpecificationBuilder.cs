namespace Ardalis.Specification;

public interface ICacheSpecificationBuilder<T, TResult>
    : ISpecificationBuilder<T, TResult>, ICacheSpecificationBuilder<T>
{
}

public interface ICacheSpecificationBuilder<T>
    : ISpecificationBuilder<T>
{
}

public interface IOrderedSpecificationBuilder<T, TResult>
    : ISpecificationBuilder<T, TResult>, IOrderedSpecificationBuilder<T>
{
}

public interface IOrderedSpecificationBuilder<T>
    : ISpecificationBuilder<T>
{
}

public interface ISpecificationBuilder<T, TResult>
    : ISpecificationBuilder<T>
{
    new Specification<T, TResult> Specification { get; }
}

public interface ISpecificationBuilder<T>
{
    Specification<T> Specification { get; }
}

internal class SpecificationBuilder<T, TResult> : SpecificationBuilder<T>,
    ICacheSpecificationBuilder<T, TResult>, IOrderedSpecificationBuilder<T, TResult>, ISpecificationBuilder<T, TResult>
{
    public new Specification<T, TResult> Specification { get; }

    public SpecificationBuilder(Specification<T, TResult> specification)
        : base(specification)
    {
        Specification = specification;
    }
}

internal class SpecificationBuilder<T>
    : ICacheSpecificationBuilder<T>, IOrderedSpecificationBuilder<T>, ISpecificationBuilder<T>
{
    public Specification<T> Specification { get; }

    public SpecificationBuilder(Specification<T> specification)
    {
        Specification = specification;
    }
}
