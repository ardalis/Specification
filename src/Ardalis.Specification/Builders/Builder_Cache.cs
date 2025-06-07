namespace Ardalis.Specification;

public static partial class SpecificationBuilderExtensions
{
    /// <summary>
    /// Sets the cache key for the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="specificationName">Used as prefix for the cache key.</param>
    /// <param name="args">To be appended to the cache key, separated by dash.</param>
    /// <returns>A cache specification builder, useful for applying further caching functionalities.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="specificationName"/> is null or empty.</exception>
    public static ICacheSpecificationBuilder<T, TResult> EnableCache<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        string specificationName,
        params object[] args) where T : class
    {
        EnableCache((ISpecificationBuilder<T>)builder, specificationName, true, args);
        return (SpecificationBuilder<T, TResult>)builder;
    }

    /// <summary>
    /// Sets the cache key for the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="specificationName">Used as prefix for the cache key.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="args">To be appended to the cache key, separated by dash.</param>
    /// <returns>A cache specification builder, useful for applying further caching functionalities.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="specificationName"/> is null or empty and <paramref name="condition"/> is true.</exception>
    public static ICacheSpecificationBuilder<T, TResult> EnableCache<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        string specificationName,
        bool condition,
        params object[] args) where T : class
    {
        EnableCache((ISpecificationBuilder<T>)builder, specificationName, condition, args);
        return (SpecificationBuilder<T, TResult>)builder;
    }

    /// <summary>
    /// Sets the cache key for the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="specificationName">Used as prefix for the cache key.</param>
    /// <param name="args">To be appended to the cache key, separated by dash.</param>
    /// <returns>A cache specification builder, useful for applying further caching functionalities.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="specificationName"/> is null or empty.</exception>
    public static ICacheSpecificationBuilder<T> EnableCache<T>(
        this ISpecificationBuilder<T> builder,
        string specificationName,
        params object[] args) where T : class
        => EnableCache(builder, specificationName, true, args);

    /// <summary>
    /// Sets the cache key for the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="specificationName">Used as prefix for the cache key.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="args">To be appended to the cache key, separated by dash.</param>
    /// <returns>A cache specification builder, useful for applying further caching functionalities.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="specificationName"/> is null or empty and <paramref name="condition"/> is true.</exception>
    public static ICacheSpecificationBuilder<T> EnableCache<T>(
        this ISpecificationBuilder<T> builder,
        string specificationName,
        bool condition,
        params object[] args) where T : class
    {
        if (condition)
        {
            if (string.IsNullOrEmpty(specificationName))
            {
                throw new ArgumentException($"Required input was null or empty.", nameof(specificationName));
            }

            builder.Specification.CacheKey = $"{specificationName}-{string.Join("-", args)}";
        }

        Specification<T>.IsChainDiscarded = !condition;
        return (SpecificationBuilder<T>)builder;
    }

    /// <summary>
    /// Sets the cache key for the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="cacheKey">The cache key to be used.</param>
    /// <returns>A cache specification builder, useful for applying further caching functionalities.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="cacheKey"/> is null or empty.</exception>
    public static ICacheSpecificationBuilder<T, TResult> WithCacheKey<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        string cacheKey) where T : class
    {
        WithCacheKey((ISpecificationBuilder<T>)builder, cacheKey, true);
        return (SpecificationBuilder<T, TResult>)builder;
    }

    /// <summary>
    /// Sets the cache key for the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="cacheKey">The cache key to be used.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>A cache specification builder, useful for applying further caching functionalities.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="cacheKey"/> is null or empty and <paramref name="condition"/> is true.</exception>
    public static ICacheSpecificationBuilder<T, TResult> WithCacheKey<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        string cacheKey,
        bool condition) where T : class
    {
        WithCacheKey((ISpecificationBuilder<T>)builder, cacheKey, condition);
        return (SpecificationBuilder<T, TResult>)builder;
    }

    /// <summary>
    /// Sets the cache key for the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="cacheKey">The cache key to be used.</param>
    /// <returns>A cache specification builder, useful for applying further caching functionalities.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="cacheKey"/> is null or empty.</exception>
    public static ICacheSpecificationBuilder<T> WithCacheKey<T>(
        this ISpecificationBuilder<T> builder,
        string cacheKey) where T : class
        => WithCacheKey(builder, cacheKey, true);

    /// <summary>
    /// Sets the cache key for the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="cacheKey">The cache key to be used.</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <returns>A cache specification builder, useful for applying further caching functionalities.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="cacheKey"/> is null or empty and <paramref name="condition"/> is true.</exception>
    public static ICacheSpecificationBuilder<T> WithCacheKey<T>(
        this ISpecificationBuilder<T> builder,
        string cacheKey,
        bool condition) where T : class
    {
        if (condition)
        {
            if (string.IsNullOrEmpty(cacheKey))
            {
                throw new ArgumentException($"Required input was null or empty.", nameof(cacheKey));
            }

            builder.Specification.CacheKey = cacheKey;
        }

        Specification<T>.IsChainDiscarded = !condition;
        return (SpecificationBuilder<T>)builder;
    }
}
