namespace Ardalis.Specification;

public static partial class SpecificationBuilderExtensions
{

    /// <summary>
    /// Specify a function to apply to the result of the query. It's an in-memory operation.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="filter">The filter function.</param>
    /// <returns>Filtered results</returns>
    public static ISpecificationBuilder<T, TResult> PostProcessingAction<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Func<IEnumerable<TResult>, IEnumerable<TResult>> filter)
    {
        builder.Specification.PostProcessingAction = filter;

        return builder;
    }


    /// <summary>
    /// Specify a function to apply to the result of the query. It's an in-memory operation.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="filter">The filter function.</param>
    /// <returns>Filtered results</returns>
    public static ISpecificationBuilder<T> PostProcessingAction<T>(
        this ISpecificationBuilder<T> builder,
        Func<IEnumerable<T>, IEnumerable<T>> filter)
    {
        builder.Specification.PostProcessingAction = filter;

        return builder;
    }
}
