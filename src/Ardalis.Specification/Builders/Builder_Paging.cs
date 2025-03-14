namespace Ardalis.Specification;

public static partial class SpecificationBuilderExtensions
{
    /// <summary>
    /// Sets the number of items to take in the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="take">The number of items to take.</param>
    /// <returns>The updated specification builder.</returns>
    /// <exception cref="DuplicateTakeException">If take value is already set.</exception>
    public static ISpecificationBuilder<T, TResult> Take<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        int take)
        => Take(builder, take, true);

    /// <summary>
    /// Sets the number of items to take in the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="take">The number of items to take.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    /// <exception cref="DuplicateTakeException">If take value is already set.</exception>
    public static ISpecificationBuilder<T, TResult> Take<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        int take,
        bool condition)
    {
        ((ISpecificationBuilder<T>)builder).Take(take, condition);
        return builder;
    }

    /// <summary>
    /// Sets the number of items to take in the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="take">The number of items to take.</param>
    /// <returns>The updated specification builder.</returns>
    /// <exception cref="DuplicateTakeException">If take value is already set.</exception>
    public static ISpecificationBuilder<T> Take<T>(
        this ISpecificationBuilder<T> builder,
        int take)
        => Take(builder, take, true);

    /// <summary>
    /// Sets the number of items to take in the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="take">The number of items to take.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    /// <exception cref="DuplicateTakeException">If take value is already set.</exception>
    public static ISpecificationBuilder<T> Take<T>(
        this ISpecificationBuilder<T> builder,
        int take,
        bool condition)
    {
        if (condition)
        {
            if (builder.Specification.Take != -1) throw new DuplicateTakeException();
            builder.Specification.Take = take;
        }

        return builder;
    }

    /// <summary>
    /// Sets the number of items to skip in the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="skip">The number of items to skip.</param>
    /// <returns>The updated specification builder.</returns>
    /// <exception cref="DuplicateSkipException">If the skip value is already set.</exception>
    public static ISpecificationBuilder<T, TResult> Skip<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        int skip)
        => Skip(builder, skip, true);

    /// <summary>
    /// Sets the number of items to skip in the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="skip">The number of items to skip.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    /// <exception cref="DuplicateSkipException">If the skip value is already set.</exception>
    public static ISpecificationBuilder<T, TResult> Skip<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        int skip,
        bool condition)
    {
        ((ISpecificationBuilder<T>)builder).Skip(skip, condition);
        return builder;
    }

    /// <summary>
    /// Sets the number of items to skip in the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="skip">The number of items to skip.</param>
    /// <returns>The updated specification builder.</returns>
    /// <exception cref="DuplicateSkipException">If the skip value is already set.</exception>
    public static ISpecificationBuilder<T> Skip<T>(
        this ISpecificationBuilder<T> builder,
        int skip)
        => Skip(builder, skip, true);

    /// <summary>
    /// Sets the number of items to skip in the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="skip">The number of items to skip.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    /// <exception cref="DuplicateSkipException">If the skip value is already set.</exception>
    public static ISpecificationBuilder<T> Skip<T>(
        this ISpecificationBuilder<T> builder,
        int skip,
        bool condition)
    {
        if (condition)
        {
            if (builder.Specification.Skip != -1) throw new DuplicateSkipException();
            builder.Specification.Skip = skip;
        }

        return builder;
    }
}
