using Microsoft.EntityFrameworkCore;

namespace Ardalis.Specification.EntityFrameworkCore;

public class AsTrackingEvaluator : IEvaluator
{
    private AsTrackingEvaluator() { }
    public static AsTrackingEvaluator Instance { get; } = new AsTrackingEvaluator();

    public bool IsCriteriaEvaluator { get; } = true;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        if (specification.AsTracking)
        {
            query = query.AsTracking();
        }

        return query;
    }
}
