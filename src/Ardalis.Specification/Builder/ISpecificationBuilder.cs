namespace Ardalis.Specification;

public interface ISpecificationBuilder<T, TResult> : ISpecificationBuilder<T>
{
    new Specification<T, TResult> Specification { get; }
}

public interface ISpecificationBuilder<T>
{
    Specification<T> Specification { get; }
}
