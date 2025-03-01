namespace Ardalis.Specification;

public static partial class SpecificationBuilderExtensions
{
    // TODO: Improve the nullability for the key selector. Update everything to work with Func<T, string?>. [Fati Iseni, 27/02/2025]

    /// <summary>
    /// Adds a Like clause to the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <param name="pattern">The pattern to match.</param>
    /// <param name="group">The group number. Like clauses within the same group are evaluated using OR logic.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> Search<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, string?>> keySelector,
        string pattern,
        int group = 1) where T : class
        => Search(builder, keySelector, pattern, true, group);

    /// <summary>
    /// Adds a Like clause to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <param name="pattern">The pattern to match.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="group">The group number. Like clauses within the same group are evaluated using OR logic.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> Search<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, string?>> keySelector,
        string pattern,
        bool condition,
        int group = 1) where T : class
    {
        if (condition)
        {
            var expr = new SearchExpressionInfo<T>(keySelector, pattern, group);
            builder.Specification.Add(expr);
        }

        return builder;
    }

    /// <summary>
    /// Adds a Like clause to the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <param name="pattern">The pattern to match.</param>
    /// <param name="group">The group number. Like clauses within the same group are evaluated using OR logic.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> Search<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, string?>> keySelector,
        string pattern,
        int group = 1) where T : class
        => Search(builder, keySelector, pattern, true, group);

    /// <summary>
    /// Adds a Like clause to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <param name="pattern">The pattern to match.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="group">The group number. Like clauses within the same group are evaluated using OR logic.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> Search<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, string?>> keySelector,
        string pattern,
        bool condition,
        int group = 1) where T : class
    {
        if (condition)
        {
            var expr = new SearchExpressionInfo<T>(keySelector, pattern, group);
            builder.Specification.Add(expr);
        }

        return builder;
    }
}
