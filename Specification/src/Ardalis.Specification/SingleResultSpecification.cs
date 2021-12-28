namespace Ardalis.Specification
{
    /// <summary>
    /// Encapsulates query logic that returns a single result for <typeparamref name="T"/>,
    /// and projects the result into <typeparamref name="TResult"/>.
    /// </summary>
    public abstract class SingleResultSpecification<T, TResult> : Specification<T, TResult>, ISingleResultSpecification<T>
    {
    }

    /// <summary>
    /// Encapsulates query logic that returns a single result for <typeparamref name="T"/>.
    /// </summary>
    public abstract class SingleResultSpecification<T> : Specification<T>, ISingleResultSpecification<T>
    {
    }
}