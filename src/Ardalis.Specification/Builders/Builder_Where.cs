namespace Ardalis.Specification;

public static partial class SpecificationBuilderExtensions
{
    /// <summary>
    /// Adds a Where clause to the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="predicate">The predicate expression.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> Where<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, bool>> predicate)
        => Where(builder, predicate, true);

    /// <summary>
    /// Adds a Where clause to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="predicate">The predicate expression.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> Where<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, bool>> predicate,
        bool condition)
    {
        ((ISpecificationBuilder<T>)builder).Where(predicate, condition);
        return builder;
    }

    /// <summary>
    /// Adds a Where clause to the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="predicate">The predicate expression.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> Where<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, bool>> predicate)
        => Where(builder, predicate, true);

    /// <summary>
    /// Adds a Where clause to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="predicate">The predicate expression.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> Where<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, bool>> predicate,
        bool condition)
    {
        if (condition)
        {
            var expr = new WhereExpressionInfo<T>(predicate);
            builder.Specification.Add(expr);
        }

        return builder;
    }
}
