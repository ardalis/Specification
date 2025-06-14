namespace Ardalis.Specification;

/// <summary>
/// Represents a specification builder that supports caching operations.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public interface ICacheSpecificationBuilder<T, TResult>
    : ISpecificationBuilder<T, TResult>, ICacheSpecificationBuilder<T>
{
}

/// <summary>
/// Represents a specification builder that supports caching operations.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public interface ICacheSpecificationBuilder<T>
    : ISpecificationBuilder<T>
{
}

/// <summary>
/// Represents a specification builder that supports order operations.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public interface IOrderedSpecificationBuilder<T, TResult>
    : ISpecificationBuilder<T, TResult>, IOrderedSpecificationBuilder<T>
{
}

/// <summary>
/// Represents a specification builder that supports order operations.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public interface IOrderedSpecificationBuilder<T>
    : ISpecificationBuilder<T>
{
}

/// <summary>
/// Represents a specification builder.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public interface ISpecificationBuilder<T, TResult>
    : ISpecificationBuilder<T>
{
    /// <summary>
    /// Gets the specification associated with this builder.
    /// </summary>
    new Specification<T, TResult> Specification { get; }
}

/// <summary>
/// Represents a specification builder.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public interface ISpecificationBuilder<T>
{
    /// <summary>
    /// Gets the specification associated with this builder.
    /// </summary>
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
