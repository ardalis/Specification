using System;

namespace Ardalis.Specification;

/// <summary>
/// A marker interface for specifications that are meant to return a single entity. Used to constrain methods
/// that accept a Specification and return a single result rather than a collection of results.
/// </summary>
[Obsolete("Use ISingleResultSpecification<T> instead. This interface will be removed in a future version of Ardalis.Specification.")]
public interface ISingleResultSpecification
{
}

/// <summary>
/// Encapsulates query logic for <typeparamref name="T"/>. It is meant to return a single result.
/// </summary>
/// <typeparam name="T">The type being queried against.</typeparam>
public interface ISingleResultSpecification<T> : ISpecification<T>//, ISingleResultSpecification
{
}

/// <summary>
/// Encapsulates query logic for <typeparamref name="T"/>,
/// and projects the result into <typeparamref name="TResult"/>. It is meant to return a single result.
/// </summary>
/// <typeparam name="T">The type being queried against.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public interface ISingleResultSpecification<T, TResult> : ISpecification<T, TResult>//, ISingleResultSpecification
{
}
