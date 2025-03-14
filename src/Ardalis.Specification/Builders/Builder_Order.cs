namespace Ardalis.Specification;

public static partial class SpecificationBuilderExtensions
{
    /// <summary>
    /// Adds an OrderBy clause to the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <returns>The updated ordered specification builder.</returns>
    public static IOrderedSpecificationBuilder<T, TResult> OrderBy<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, object?>> keySelector)
        => OrderBy(builder, keySelector, true);

    /// <summary>
    /// Adds an OrderBy clause to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated ordered specification builder.</returns>
    public static IOrderedSpecificationBuilder<T, TResult> OrderBy<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, object?>> keySelector,
        bool condition)
    {
        ((ISpecificationBuilder<T>)builder).OrderBy(keySelector, condition);
        return (SpecificationBuilder<T, TResult>)builder;
    }

    /// <summary>
    /// Adds an OrderBy clause to the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <returns>The updated ordered specification builder.</returns>
    public static IOrderedSpecificationBuilder<T> OrderBy<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, object?>> keySelector)
        => OrderBy(builder, keySelector, true);

    /// <summary>
    /// Adds an OrderBy clause to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated ordered specification builder.</returns>
    public static IOrderedSpecificationBuilder<T> OrderBy<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, object?>> keySelector,
        bool condition)
    {
        if (condition)
        {
            var expr = new OrderExpressionInfo<T>(keySelector, OrderTypeEnum.OrderBy);
            builder.Specification.Add(expr);
        }

        Specification<T>.IsChainDiscarded = !condition;
        return (SpecificationBuilder<T>)builder;
    }

    /// <summary>
    /// Adds an OrderBy descending clause to the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <returns>The updated ordered specification builder.</returns>
    public static IOrderedSpecificationBuilder<T, TResult> OrderByDescending<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, object?>> keySelector)
        => OrderByDescending(builder, keySelector, true);

    /// <summary>
    /// Adds an OrderByDescending clause to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated ordered specification builder.</returns>
    public static IOrderedSpecificationBuilder<T, TResult> OrderByDescending<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, object?>> keySelector,
        bool condition)
    {
        ((ISpecificationBuilder<T>)builder).OrderByDescending(keySelector, condition);
        return (SpecificationBuilder<T, TResult>)builder;
    }

    /// <summary>
    /// Adds an OrderByDescending clause to the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <returns>The updated ordered specification builder.</returns>
    public static IOrderedSpecificationBuilder<T> OrderByDescending<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, object?>> keySelector)
        => OrderByDescending(builder, keySelector, true);

    /// <summary>
    /// Adds an OrderByDescending clause to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated ordered specification builder.</returns>
    public static IOrderedSpecificationBuilder<T> OrderByDescending<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, object?>> keySelector,
        bool condition)
    {
        if (condition)
        {
            var expr = new OrderExpressionInfo<T>(keySelector, OrderTypeEnum.OrderByDescending);
            builder.Specification.Add(expr);
        }

        Specification<T>.IsChainDiscarded = !condition;
        return (SpecificationBuilder<T>)builder;
    }

    /// <summary>
    /// Adds a ThenBy clause to the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The ordered specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <returns>The updated ordered specification builder.</returns>
    public static IOrderedSpecificationBuilder<T, TResult> ThenBy<T, TResult>(
        this IOrderedSpecificationBuilder<T, TResult> builder,
        Expression<Func<T, object?>> keySelector)
        => ThenBy(builder, keySelector, true);

    /// <summary>
    /// Adds a ThenBy clause to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The ordered specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated ordered specification builder.</returns>
    public static IOrderedSpecificationBuilder<T, TResult> ThenBy<T, TResult>(
        this IOrderedSpecificationBuilder<T, TResult> builder,
        Expression<Func<T, object?>> keySelector,
        bool condition)
    {
        ((IOrderedSpecificationBuilder<T>)builder).ThenBy(keySelector, condition);
        return builder;
    }

    /// <summary>
    /// Adds a ThenBy clause to the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The ordered specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <returns>The updated ordered specification builder.</returns>
    public static IOrderedSpecificationBuilder<T> ThenBy<T>(
        this IOrderedSpecificationBuilder<T> builder,
        Expression<Func<T, object?>> keySelector)
        => ThenBy(builder, keySelector, true);

    /// <summary>
    /// Adds a ThenBy clause to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The ordered specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated ordered specification builder.</returns>
    public static IOrderedSpecificationBuilder<T> ThenBy<T>(
        this IOrderedSpecificationBuilder<T> builder,
        Expression<Func<T, object?>> keySelector,
        bool condition)
    {
        if (condition && !Specification<T>.IsChainDiscarded)
        {
            var expr = new OrderExpressionInfo<T>(keySelector, OrderTypeEnum.ThenBy);
            builder.Specification.Add(expr);
        }
        else
        {
            Specification<T>.IsChainDiscarded = true;
        }

        return builder;
    }

    /// <summary>
    /// Adds a ThenByDescending clause to the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The ordered specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <returns>The updated ordered specification builder.</returns>
    public static IOrderedSpecificationBuilder<T, TResult> ThenByDescending<T, TResult>(
        this IOrderedSpecificationBuilder<T, TResult> builder,
        Expression<Func<T, object?>> keySelector)
        => ThenByDescending(builder, keySelector, true);

    /// <summary>
    /// Adds a ThenByDescending clause to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The ordered specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated ordered specification builder.</returns>
    public static IOrderedSpecificationBuilder<T, TResult> ThenByDescending<T, TResult>(
        this IOrderedSpecificationBuilder<T, TResult> builder,
        Expression<Func<T, object?>> keySelector,
        bool condition)
    {
        ((IOrderedSpecificationBuilder<T>)builder).ThenByDescending(keySelector, condition);
        return builder;
    }

    /// <summary>
    /// Adds a ThenByDescending clause to the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The ordered specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <returns>The updated ordered specification builder.</returns>
    public static IOrderedSpecificationBuilder<T> ThenByDescending<T>(
        this IOrderedSpecificationBuilder<T> builder,
        Expression<Func<T, object?>> keySelector)
        => ThenByDescending(builder, keySelector, true);

    /// <summary>
    /// Adds a ThenByDescending clause to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The ordered specification builder.</param>
    /// <param name="keySelector">The key selector expression.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated ordered specification builder.</returns>
    public static IOrderedSpecificationBuilder<T> ThenByDescending<T>(
        this IOrderedSpecificationBuilder<T> builder,
        Expression<Func<T, object?>> keySelector,
        bool condition)
    {
        if (condition && !Specification<T>.IsChainDiscarded)
        {
            var expr = new OrderExpressionInfo<T>(keySelector, OrderTypeEnum.ThenByDescending);
            builder.Specification.Add(expr);
        }
        else
        {
            Specification<T>.IsChainDiscarded = true;
        }

        return builder;
    }
}
