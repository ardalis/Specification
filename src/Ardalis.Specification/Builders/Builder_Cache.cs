﻿namespace Ardalis.Specification;

public static partial class SpecificationBuilderExtensions
{
    /// <summary>
    /// Set's the cache key for the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="specificationName">Used as prefix for the cache key</param>
    /// <param name="args">To be appended to the cache key, separated by dash.</param>
    /// <returns>The updated ordered specification builder.</returns>
    /// <exception cref="ArgumentException">If specificationName is null or empty.</exception>
    public static ICacheSpecificationBuilder<T, TResult> EnableCache<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        string specificationName,
        params object[] args) where T : class
        => EnableCache(builder, specificationName, true, args);

    /// <summary>
    /// Set's the cache key for the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="specificationName">Used as prefix for the cache key</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="args">To be appended to the cache key, separated by dash.</param>
    /// <returns>The updated ordered specification builder.</returns>
    /// <exception cref="ArgumentException">If specificationName is null or empty.</exception>
    public static ICacheSpecificationBuilder<T, TResult> EnableCache<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        string specificationName,
        bool condition,
        params object[] args) where T : class
    {
        if (condition)
        {
            if (string.IsNullOrEmpty(specificationName))
            {
                throw new ArgumentException($"Required input {specificationName} was null or empty.", specificationName);
            }

            builder.Specification.CacheKey = $"{specificationName}-{string.Join("-", args)}";
        }

        Specification<T, TResult>.IsChainDiscarded = !condition;
        return (SpecificationBuilder<T, TResult>)builder;
    }

    /// <summary>
    /// Set's the cache key for the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="specificationName">Used as prefix for the cache key</param>
    /// <param name="args">To be appended to the cache key, separated by dash.</param>
    /// <returns>The updated ordered specification builder.</returns>
    /// <exception cref="ArgumentException">If specificationName is null or empty.</exception>
    public static ICacheSpecificationBuilder<T> EnableCache<T>(
        this ISpecificationBuilder<T> builder,
        string specificationName,
        params object[] args) where T : class
        => EnableCache(builder, specificationName, true, args);

    /// <summary>
    /// Set's the cache key for the specification.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="builder">The specification builder.</param>
    /// <param name="specificationName">Used as prefix for the cache key</param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="args">To be appended to the cache key, separated by dash.</param>
    /// <returns>The updated ordered specification builder.</returns>
    /// <exception cref="ArgumentException">If specificationName is null or empty.</exception>
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
                throw new ArgumentException($"Required input {specificationName} was null or empty.", specificationName);
            }

            builder.Specification.CacheKey = $"{specificationName}-{string.Join("-", args)}";
        }

        Specification<T>.IsChainDiscarded = !condition;
        return (SpecificationBuilder<T>)builder;
    }
}
