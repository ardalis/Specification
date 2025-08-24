using System.Data.Entity;

namespace Ardalis.Specification.EntityFramework6;

public class IncludeEvaluator : IEvaluator
{
    private IncludeEvaluator() { }
    public static IncludeEvaluator Instance { get; } = new IncludeEvaluator();

    public bool IsCriteriaEvaluator { get; } = false;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        if (specification is Specification<T> spec)
        {
            if (spec.OneOrManyIncludeExpressions.IsEmpty) return query;
            if (spec.OneOrManyIncludeExpressions.SingleOrDefault is { } includeExpression)
            {
                var includePath = ParseIncludePath(includeExpression);
                return query.Include(includePath);
            }
        }

        string includeString = null;

        foreach (var includeInfo in specification.IncludeExpressions)
        {
            if (includeInfo.Type == IncludeTypeEnum.Include)
            {
                if (includeString is not null)
                {
                    query = query.Include(includeString);
                }

                includeString = ParseIncludePath(includeInfo);
            }
            else if (includeInfo.Type == IncludeTypeEnum.ThenInclude)
            {
                includeString += ExpressionExtensions.MEMBER_DELIMITER + ParseIncludePath(includeInfo);
            }
        }

        if (includeString is not null)
        {
            query = query.Include(includeString);
        }

        return query;
    }

    private static string ParseIncludePath(IncludeExpressionInfo includeInfo)
    {
        if (!ExpressionExtensions.TryParsePath(includeInfo.LambdaExpression.Body, out var path) || path == null)
        {
            throw new InvalidIncludeExpressionException();
        }
        return path;
    }
}
