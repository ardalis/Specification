using System.Data.Entity;
using System.Linq;

namespace Ardalis.Specification.EntityFramework6;

public class AsNoTrackingEvaluator : IEvaluator
{
    private AsNoTrackingEvaluator() { }
    public static AsNoTrackingEvaluator Instance { get; } = new AsNoTrackingEvaluator();

    public bool IsCriteriaEvaluator { get; } = true;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        if (specification.AsNoTracking)
        {
            query = query.AsNoTracking();
        }

        return query;
    }
}
