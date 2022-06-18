namespace Ardalis.Specification
{
  /// <summary>
  /// A marker interface for specifications that are meant to return a single entity. Used to constrain methods
  /// that accept a Specification and return a single result rather than a collection of results.
  /// </summary>
  public class SingleResultSpecification<T> : Specification<T>, ISingleResultSpecification<T>
  {
  }

  /// <summary>
  /// A marker interface for specifications that are meant to return a single entity. Used to constrain methods
  /// that accept a Specification and return a single result rather than a collection of results.
  /// </summary>
  public class SingleResultSpecification<T, TResult> : Specification<T, TResult>, ISingleResultSpecification<T, TResult>
  {
  }
}
