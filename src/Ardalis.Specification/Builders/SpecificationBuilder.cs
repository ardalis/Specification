namespace Ardalis.Specification;

public interface ICacheSpecificationBuilder<T, TResult> : ISpecificationBuilder<T, TResult>
{
}

public interface ICacheSpecificationBuilder<T> : ISpecificationBuilder<T>
{
}

public interface IOrderedSpecificationBuilder<T, TResult> : ISpecificationBuilder<T, TResult>
{
}

public interface IOrderedSpecificationBuilder<T> : ISpecificationBuilder<T>
{
}

public interface ISpecificationBuilder<T, TResult>
{
    Specification<T, TResult> Specification { get; }
}

public interface ISpecificationBuilder<T>
{
    Specification<T> Specification { get; }
}

internal class SpecificationBuilder<T, TResult>
    : ICacheSpecificationBuilder<T, TResult>, IOrderedSpecificationBuilder<T, TResult>, ISpecificationBuilder<T, TResult>
{
    public Specification<T, TResult> Specification { get; }

    public SpecificationBuilder(Specification<T, TResult> specification)
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
