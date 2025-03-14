namespace Ardalis.Specification;

public static partial class SpecificationBuilderExtensions
{
    /// <summary>
    /// Adds a query tag to the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="tag">The query tag.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> TagWith<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        string tag)
        => TagWith(builder, tag, true);

    /// <summary>
    /// Adds a query tag to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="tag">The query tag.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> TagWith<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        string tag,
        bool condition)
    {
        ((ISpecificationBuilder<T>)builder).TagWith(tag, condition);
        return builder;
    }

    /// <summary>
    /// Adds a query tag to the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="tag">The query tag.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> TagWith<T>(
        this ISpecificationBuilder<T> builder,
        string tag)
        => TagWith(builder, tag, true);

    /// <summary>
    /// Adds a query tag to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="tag">The query tag.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> TagWith<T>(
        this ISpecificationBuilder<T> builder,
        string tag,
        bool condition)
    {
        if (condition)
        {
            builder.Specification.QueryTag = tag;
        }

        return builder;
    }
}
