namespace Ardalis.Specification;

/// <summary>
/// Represents a specification builder that supports include operations.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
/// <typeparam name="TProperty">The type of the property.</typeparam>
public interface IIncludableSpecificationBuilder<T, TResult, out TProperty>
    : ISpecificationBuilder<T, TResult>, IIncludableSpecificationBuilder<T, TProperty> where T : class
{
}

/// <summary>
/// Represents a specification builder that supports include operations.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
/// <typeparam name="TProperty">The type of the property.</typeparam>
public interface IIncludableSpecificationBuilder<T, out TProperty>
    : ISpecificationBuilder<T> where T : class
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
