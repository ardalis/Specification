namespace Ardalis.Specification;

/// <summary>
/// Extension methods for specifications.
/// </summary>
public static class SpecificationExtensions
{
    /// <summary>
    /// Creates a new specification by applying a projection specification to the current specification.
    /// </summary>
    /// <remarks>This method clones the source specification and applies the projection specification's select
    /// statements to create a new specification. The input specifications remain unchanged.</remarks>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="source">The source specification to which the projection will be applied. Cannot be <see langword="null"/>.</param>
    /// <param name="projectionSpec">The projection specification that defines the transformation to apply to the source specification. Cannot be
    /// <see langword="null"/>.</param>
    /// <returns>A new <see cref="Specification{T, TResult}"/> that represents the result of applying the projection to the
    /// source specification.</returns>
    public static Specification<T, TResult> WithProjectionOf<T, TResult>(this ISpecification<T> source, ISpecification<T, TResult> projectionSpec)
    {
        var newSpec = new Specification<T, TResult>();
        source.CopyTo(newSpec);
        newSpec.Selector = projectionSpec.Selector;
        newSpec.SelectorMany = projectionSpec.SelectorMany;
        newSpec.PostProcessingAction = projectionSpec.PostProcessingAction;
        return newSpec;
    }
}
