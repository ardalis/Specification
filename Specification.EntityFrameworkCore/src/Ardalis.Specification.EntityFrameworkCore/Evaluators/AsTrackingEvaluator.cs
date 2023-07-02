using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Ardalis.Specification.EntityFrameworkCore
{
  /// <summary>
  /// Evaluate AsTracking
  /// </summary>
  public class AsTrackingEvaluator : IEvaluator
  {
    private AsTrackingEvaluator() { }
    public static AsTrackingEvaluator Instance => new();

    /// <inheritdoc />
    public bool IsCriteriaEvaluator => true;

    /// <inheritdoc />
    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class =>
      specification.AsTracking
        ? query.AsTracking()
        : query;
  }
}
