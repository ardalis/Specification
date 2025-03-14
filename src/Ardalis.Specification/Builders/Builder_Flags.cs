namespace Ardalis.Specification;

public static partial class SpecificationBuilderExtensions
{
    /// <summary>
    /// Configures the specification to ignore query filters.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> IgnoreQueryFilters<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder) where T : class
    {
        IgnoreQueryFilters((ISpecificationBuilder<T>)builder, true);
        return builder;
    }

    /// <summary>
    /// Configures the specification to ignore query filters if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> IgnoreQueryFilters<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        bool condition) where T : class
    {
        IgnoreQueryFilters((ISpecificationBuilder<T>)builder, condition);
        return builder;
    }

    /// <summary>
    /// Configures the specification to ignore query filters.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> IgnoreQueryFilters<T>(
        this ISpecificationBuilder<T> builder) where T : class
        => IgnoreQueryFilters(builder, true);

    /// <summary>
    /// Configures the specification to ignore query filters if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> IgnoreQueryFilters<T>(
        this ISpecificationBuilder<T> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.IgnoreQueryFilters = true;
        }

        return builder;
    }

    /// <summary>
    /// Configures the specification to ignore auto includes.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> IgnoreAutoIncludes<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder) where T : class
    {
        IgnoreAutoIncludes((ISpecificationBuilder<T>)builder, true);
        return builder;
    }

    /// <summary>
    /// Configures the specification to ignore auto includes if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> IgnoreAutoIncludes<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        bool condition) where T : class
    {
        IgnoreAutoIncludes((ISpecificationBuilder<T>)builder, condition);
        return builder;
    }

    /// <summary>
    /// Configures the specification to ignore auto includes.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> IgnoreAutoIncludes<T>(
        this ISpecificationBuilder<T> builder) where T : class
        => IgnoreAutoIncludes(builder, true);

    /// <summary>
    /// Configures the specification to ignore auto includes if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> IgnoreAutoIncludes<T>(
        this ISpecificationBuilder<T> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.IgnoreAutoIncludes = true;
        }

        return builder;
    }

    /// <summary>
    /// Configures the specification to use split queries.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> AsSplitQuery<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder) where T : class
    {
        AsSplitQuery((ISpecificationBuilder<T>)builder, true);
        return builder;
    }

    /// <summary>
    /// Configures the specification to use split queries if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> AsSplitQuery<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        bool condition) where T : class
    {
        AsSplitQuery((ISpecificationBuilder<T>)builder, condition);
        return builder;
    }

    /// <summary>
    /// Configures the specification to use split queries.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> AsSplitQuery<T>(
        this ISpecificationBuilder<T> builder) where T : class
        => AsSplitQuery(builder, true);

    /// <summary>
    /// Configures the specification to use split queries if the condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> AsSplitQuery<T>(
        this ISpecificationBuilder<T> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.AsSplitQuery = true;
        }

        return builder;
    }

    /// <summary>
    /// Configures the specification to apply NoTracking behavior.
    /// It will also disable AsNoTrackingWithIdentityResolution and AsTracking flags.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> AsNoTracking<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder) where T : class
    {
        AsNoTracking((ISpecificationBuilder<T>)builder, true);
        return builder;
    }

    /// <summary>
    /// Configures the specification to apply NoTracking behavior if the condition is true.
    /// It will also disable AsNoTrackingWithIdentityResolution and AsTracking flags.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> AsNoTracking<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        bool condition) where T : class
    {
        AsNoTracking((ISpecificationBuilder<T>)builder, condition);
        return builder;
    }

    /// <summary>
    /// Configures the specification to apply NoTracking behavior.
    /// It will also disable AsNoTrackingWithIdentityResolution and AsTracking flags.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> AsNoTracking<T>(
        this ISpecificationBuilder<T> builder) where T : class
        => AsNoTracking(builder, true);

    /// <summary>
    /// Configures the specification to apply NoTracking behavior if the condition is true.
    /// It will also disable AsNoTrackingWithIdentityResolution and AsTracking flags.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> AsNoTracking<T>(
        this ISpecificationBuilder<T> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.AsTracking = false;
            builder.Specification.AsNoTrackingWithIdentityResolution = false;
            builder.Specification.AsNoTracking = true;
        }

        return builder;
    }

    /// <summary>
    /// Configures the specification to apply AsNoTrackingWithIdentityResolution behavior.
    /// It will also disable AsNoTracking and AsTracking flags.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> AsNoTrackingWithIdentityResolution<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder) where T : class
    {
        AsNoTrackingWithIdentityResolution((ISpecificationBuilder<T>)builder, true);
        return builder;
    }

    /// <summary>
    /// Configures the specification to apply AsNoTrackingWithIdentityResolution behavior if the condition is true.
    /// It will also disable AsNoTracking and AsTracking flags.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> AsNoTrackingWithIdentityResolution<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        bool condition) where T : class
    {
        AsNoTrackingWithIdentityResolution((ISpecificationBuilder<T>)builder, condition);
        return builder;
    }

    /// <summary>
    /// Configures the specification to apply AsNoTrackingWithIdentityResolution behavior.
    /// It will also disable AsNoTracking and AsTracking flags.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> AsNoTrackingWithIdentityResolution<T>(
        this ISpecificationBuilder<T> builder) where T : class
        => AsNoTrackingWithIdentityResolution(builder, true);

    /// <summary>
    /// Configures the specification to apply AsNoTrackingWithIdentityResolution behavior if the condition is true.
    /// It will also disable AsNoTracking and AsTracking flags.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> AsNoTrackingWithIdentityResolution<T>(
        this ISpecificationBuilder<T> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.AsTracking = false;
            builder.Specification.AsNoTracking = false;
            builder.Specification.AsNoTrackingWithIdentityResolution = true;
        }

        return builder;
    }

    /// <summary>
    /// Configures the specification to apply AsTracking behavior.
    /// It will also disable AsNoTrackingWithIdentityResolution and AsNoTracking flags.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> AsTracking<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder) where T : class
    {
        AsTracking((ISpecificationBuilder<T>)builder, true);
        return builder;
    }

    /// <summary>
    /// Configures the specification to apply AsTracking behavior if the condition is true.
    /// It will also disable AsNoTrackingWithIdentityResolution and AsNoTracking flags.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T, TResult> AsTracking<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        bool condition) where T : class
    {
        AsTracking((ISpecificationBuilder<T>)builder, condition);
        return builder;
    }

    /// <summary>
    /// Configures the specification to apply AsTracking behavior.
    /// It will also disable AsNoTrackingWithIdentityResolution and AsNoTracking flags.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> AsTracking<T>(
        this ISpecificationBuilder<T> builder) where T : class
        => AsTracking(builder, true);

    /// <summary>
    /// Configures the specification to apply AsTracking behavior if the condition is true.
    /// It will also disable AsNoTrackingWithIdentityResolution and AsNoTracking flags.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>The updated specification builder.</returns>
    public static ISpecificationBuilder<T> AsTracking<T>(
        this ISpecificationBuilder<T> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.AsNoTracking = false;
            builder.Specification.AsNoTrackingWithIdentityResolution = false;
            builder.Specification.AsTracking = true;
        }

        return builder;
    }
}
