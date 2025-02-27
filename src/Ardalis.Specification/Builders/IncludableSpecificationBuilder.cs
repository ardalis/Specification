namespace Ardalis.Specification;

public interface IIncludableSpecificationBuilder<T, TResult, out TProperty> : ISpecificationBuilder<T, TResult> where T : class
{
}

public interface IIncludableSpecificationBuilder<T, out TProperty> : ISpecificationBuilder<T> where T : class
{
}

internal class IncludableSpecificationBuilder<T, TResult, TProperty>
    : SpecificationBuilder<T, TResult>, IIncludableSpecificationBuilder<T, TResult, TProperty> where T : class
{
    public IncludableSpecificationBuilder(Specification<T, TResult> specification) : base(specification)
    {
    }
}

internal class IncludableSpecificationBuilder<T, TProperty>
    : SpecificationBuilder<T>, IIncludableSpecificationBuilder<T, TProperty> where T : class
{
    public IncludableSpecificationBuilder(Specification<T> specification) : base(specification)
    {
    }
}
