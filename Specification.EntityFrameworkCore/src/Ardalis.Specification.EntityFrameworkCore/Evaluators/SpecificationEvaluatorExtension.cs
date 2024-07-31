namespace Ardalis.Specification.EntityFrameworkCore;

public static class SpecificationEvaluatorExtension
{
    public static IQueryable<T> GetQuery<T>(this IQueryable<T> queryable, Specification<T> specification) where T:class
    {
        return SpecificationEvaluator.Default.GetQuery(queryable, specification);
    }
}
