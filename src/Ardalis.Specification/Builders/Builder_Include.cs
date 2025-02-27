namespace Ardalis.Specification;

public static partial class SpecificationBuilderExtensions
{
    /// <summary>
    /// Adds an Include clause to the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="includeString">The include string.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> Include<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        string includeString) where T : class
        => Include(builder, includeString, true);

    /// <summary>
    /// Adds an Include clause to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="includeString">The include string.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> Include<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        string includeString,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.Add(includeString);
        }

        return builder;
    }

    /// <summary>
    /// Adds an Include clause to the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="includeString">The include string.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> Include<T>(
        this ISpecificationBuilder<T> builder,
        string includeString) where T : class
        => Include(builder, includeString, true);

    /// <summary>
    /// Adds an Include clause to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="includeString">The include string.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> Include<T>(
        this ISpecificationBuilder<T> builder,
        string includeString,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.Add(includeString);
        }

        return builder;
    }

    /// <summary>
    /// Adds an Include clause to the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="navigationSelector">The include expression.</param>
    /// <returns>The updated includable specification builder.</returns>
    public static IIncludableSpecificationBuilder<T, TResult, TProperty> Include<T, TResult, TProperty>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, TProperty>> navigationSelector) where T : class
        => Include(builder, navigationSelector, true);

    /// <summary>
    /// Adds an Include clause to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="navigationSelector">The include expression.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated includable specification builder.</returns>
    public static IIncludableSpecificationBuilder<T, TResult, TProperty> Include<T, TResult, TProperty>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, TProperty>> navigationSelector,
        bool condition) where T : class
    {
        if (condition)
        {
            var expr = new IncludeExpressionInfo(navigationSelector, typeof(T), typeof(TProperty));
            builder.Specification.Add(expr);
        }

        Specification<T, TResult>.IsChainDiscarded = !condition;
        var includeBuilder = new IncludableSpecificationBuilder<T, TResult, TProperty>(builder.Specification);
        return includeBuilder;
    }

    /// <summary>
    /// Adds an Include clause to the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="navigationSelector">The include expression.</param>
    /// <returns>The updated includable specification builder.</returns>
    public static IIncludableSpecificationBuilder<T, TProperty> Include<T, TProperty>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, TProperty>> navigationSelector) where T : class
        => Include(builder, navigationSelector, true);

    /// <summary>
    /// Adds an Include clause to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="navigationSelector">The include expression.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated includable specification builder.</returns>
    public static IIncludableSpecificationBuilder<T, TProperty> Include<T, TProperty>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, TProperty>> navigationSelector,
        bool condition) where T : class
    {
        if (condition)
        {
            var expr = new IncludeExpressionInfo(navigationSelector, typeof(T), typeof(TProperty));
            builder.Specification.Add(expr);
        }

        Specification<T>.IsChainDiscarded = !condition;
        var includeBuilder = new IncludableSpecificationBuilder<T, TProperty>(builder.Specification);
        return includeBuilder;
    }

    /// <summary>
    /// Adds a ThenInclude clause to the specification.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TPreviousProperty">The type of the previous property.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="builder">The previous includable specification builder.</param>
    /// <param name="navigationSelector">The include expression.</param>
    /// <returns>The updated includable specification builder.</returns>
    public static IIncludableSpecificationBuilder<TEntity, TResult, TProperty> ThenInclude<TEntity, TResult, TPreviousProperty, TProperty>(
        this IIncludableSpecificationBuilder<TEntity, TResult, TPreviousProperty> builder,
        Expression<Func<TPreviousProperty, TProperty>> navigationSelector)
        where TEntity : class
        => ThenInclude(builder, navigationSelector, true);

    /// <summary>
    /// Adds a ThenInclude clause to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TPreviousProperty">The type of the previous property.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="builder">The previous includable specification builder.</param>
    /// <param name="navigationSelector">The include expression.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated includable specification builder.</returns>
    public static IIncludableSpecificationBuilder<TEntity, TResult, TProperty> ThenInclude<TEntity, TResult, TPreviousProperty, TProperty>(
        this IIncludableSpecificationBuilder<TEntity, TResult, TPreviousProperty> builder,
        Expression<Func<TPreviousProperty, TProperty>> navigationSelector,
        bool condition)
        where TEntity : class
    {
        if (condition && !Specification<TEntity, TResult>.IsChainDiscarded)
        {
            var expr = new IncludeExpressionInfo(navigationSelector, typeof(TEntity), typeof(TProperty), typeof(TPreviousProperty));
            builder.Specification.Add(expr);
        }
        else
        {
            Specification<TEntity, TResult>.IsChainDiscarded = true;
        }

        var includeBuilder = new IncludableSpecificationBuilder<TEntity, TResult, TProperty>(builder.Specification);
        return includeBuilder;
    }

    /// <summary>
    /// Adds a ThenInclude clause to the specification.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TPreviousProperty">The type of the previous property.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="builder">The previous includable specification builder.</param>
    /// <param name="navigationSelector">The include expression.</param>
    /// <returns>The updated includable specification builder.</returns>
    public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        this IIncludableSpecificationBuilder<TEntity, TPreviousProperty> builder,
        Expression<Func<TPreviousProperty, TProperty>> navigationSelector)
        where TEntity : class
        => ThenInclude(builder, navigationSelector, true);

    /// <summary>
    /// Adds a ThenInclude clause to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TPreviousProperty">The type of the previous property.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="builder">The previous includable specification builder.</param>
    /// <param name="navigationSelector">The include expression.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated includable specification builder.</returns>
    public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        this IIncludableSpecificationBuilder<TEntity, TPreviousProperty> builder,
        Expression<Func<TPreviousProperty, TProperty>> navigationSelector,
        bool condition)
        where TEntity : class
    {
        if (condition && !Specification<TEntity>.IsChainDiscarded)
        {
            var expr = new IncludeExpressionInfo(navigationSelector, typeof(TEntity), typeof(TProperty), typeof(TPreviousProperty));
            builder.Specification.Add(expr);
        }
        else
        {
            Specification<TEntity>.IsChainDiscarded = true;
        }

        var includeBuilder = new IncludableSpecificationBuilder<TEntity, TProperty>(builder.Specification);
        return includeBuilder;
    }

    /// <summary>
    /// Adds a ThenInclude clause to the specification.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TPreviousProperty">The type of the previous property.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="builder">The previous includable specification builder.</param>
    /// <param name="navigationSelector">The include expression.</param>
    /// <returns>The updated includable specification builder.</returns>
    public static IIncludableSpecificationBuilder<TEntity, TResult, TProperty> ThenInclude<TEntity, TResult, TPreviousProperty, TProperty>(
        this IIncludableSpecificationBuilder<TEntity, TResult, IEnumerable<TPreviousProperty>> builder,
        Expression<Func<TPreviousProperty, TProperty>> navigationSelector)
        where TEntity : class
        => ThenInclude(builder, navigationSelector, true);

    /// <summary>
    /// Adds a ThenInclude clause to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TPreviousProperty">The type of the previous property.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="builder">The previous includable specification builder.</param>
    /// <param name="navigationSelector">The include expression.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated includable specification builder.</returns>
    public static IIncludableSpecificationBuilder<TEntity, TResult, TProperty> ThenInclude<TEntity, TResult, TPreviousProperty, TProperty>(
        this IIncludableSpecificationBuilder<TEntity, TResult, IEnumerable<TPreviousProperty>> builder,
        Expression<Func<TPreviousProperty, TProperty>> navigationSelector,
        bool condition)
        where TEntity : class
    {
        if (condition && !Specification<TEntity, TResult>.IsChainDiscarded)
        {
            var expr = new IncludeExpressionInfo(navigationSelector, typeof(TEntity), typeof(TProperty), typeof(IEnumerable<TPreviousProperty>));
            builder.Specification.Add(expr);
        }
        else
        {
            Specification<TEntity, TResult>.IsChainDiscarded = true;
        }

        var includeBuilder = new IncludableSpecificationBuilder<TEntity, TResult, TProperty>(builder.Specification);
        return includeBuilder;
    }

    /// <summary>
    /// Adds a ThenInclude clause to the specification.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TPreviousProperty">The type of the previous property.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="builder">The previous includable specification builder.</param>
    /// <param name="navigationSelector">The include expression.</param>
    /// <returns>The updated includable specification builder.</returns>
    public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        this IIncludableSpecificationBuilder<TEntity, IEnumerable<TPreviousProperty>> builder,
        Expression<Func<TPreviousProperty, TProperty>> navigationSelector)
        where TEntity : class
        => ThenInclude(builder, navigationSelector, true);

    /// <summary>
    /// Adds a ThenInclude clause to the specification if the condition is true.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TPreviousProperty">The type of the previous property.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="builder">The previous includable specification builder.</param>
    /// <param name="navigationSelector">The include expression.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated includable specification builder.</returns>
    public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        this IIncludableSpecificationBuilder<TEntity, IEnumerable<TPreviousProperty>> builder,
        Expression<Func<TPreviousProperty, TProperty>> navigationSelector,
        bool condition)
        where TEntity : class
    {
        if (condition && !Specification<TEntity>.IsChainDiscarded)
        {
            var expr = new IncludeExpressionInfo(navigationSelector, typeof(TEntity), typeof(TProperty), typeof(IEnumerable<TPreviousProperty>));
            builder.Specification.Add(expr);
        }
        else
        {
            Specification<TEntity>.IsChainDiscarded = true;
        }

        var includeBuilder = new IncludableSpecificationBuilder<TEntity, TProperty>(builder.Specification);
        return includeBuilder;
    }
}
