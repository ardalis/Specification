using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Reflection;

namespace Ardalis.Specification.EntityFramework6;

public static class SearchExtension
{
    // We'll name the property Format just so we match the produced SQL query parameter name (in case of interpolated strings).
    private class StringVar
    {
        public string Format { get; set; }
    }
    private static readonly PropertyInfo _stringFormatProperty = typeof(StringVar).GetProperty(nameof(StringVar.Format));
    private static readonly MethodInfo _likeMethodInfo = typeof(DbFunctions)
        .GetMethod(nameof(DbFunctions.Like), [typeof(string), typeof(string)]);

    // It's required so EF can generate parameterized query.
    // In the past I've been creating closures for this, e.g. var patternAsExpression = ((Expression<Func<string>>)(() => pattern)).Body;
    // But, that allocates 168 bytes. So, this is more efficient way.
    private static MemberExpression StringAsExpression(string value)
        => Expression.Property(Expression.Constant(new StringVar { Format = value }), _stringFormatProperty);

    public static IQueryable<T> ApplySingleLike<T>(this IQueryable<T> source, SearchExpressionInfo<T> searchExpression)
    {
        Debug.Assert(_likeMethodInfo is not null);

        var param = searchExpression.Selector.Parameters[0];
        var selectorExpr = searchExpression.Selector.Body;
        var patternExpr = StringAsExpression(searchExpression.SearchTerm);

        var likeExpr = Expression.Call(
            null,
            _likeMethodInfo,
            selectorExpr,
            patternExpr);

        return source.Where(Expression.Lambda<Func<T, bool>>(likeExpr, param));
    }

    public static IQueryable<T> ApplyLikesAsOrGroup<T>(
        this IQueryable<T> source,
        List<SearchExpressionInfo<T>> searchExpressions,
        int from,
        int to)
    {
        Debug.Assert(_likeMethodInfo is not null);

        Expression combinedExpr = null;
        ParameterExpression mainParam = null;
        ParameterReplacerVisitor visitor = null;

        for (int i = from; i < to; i++)
        {
            var searchExpression = searchExpressions[i];
            ApplyLikeAsOrGroup(ref combinedExpr, ref mainParam, ref visitor, searchExpression);
        }

        return combinedExpr is null || mainParam is null
            ? source
            : source.Where(Expression.Lambda<Func<T, bool>>(combinedExpr, mainParam));
    }

    public static IQueryable<T> ApplyLikesAsOrGroup<T>(
        this IQueryable<T> source,
        IEnumerable<SearchExpressionInfo<T>> searchExpressions)
    {
        Debug.Assert(_likeMethodInfo is not null);

        Expression combinedExpr = null;
        ParameterExpression mainParam = null;
        ParameterReplacerVisitor visitor = null;

        foreach (var searchExpression in searchExpressions)
        {
            ApplyLikeAsOrGroup(ref combinedExpr, ref mainParam, ref visitor, searchExpression);
        }

        return combinedExpr is null || mainParam is null
            ? source
            : source.Where(Expression.Lambda<Func<T, bool>>(combinedExpr, mainParam));
    }

    private static void ApplyLikeAsOrGroup<T>(
        ref Expression combinedExpr,
        ref ParameterExpression mainParam,
        ref ParameterReplacerVisitor visitor,
        SearchExpressionInfo<T> searchExpression)
    {
        mainParam ??= searchExpression.Selector.Parameters[0];

        var selectorExpr = searchExpression.Selector.Body;
        if (mainParam != searchExpression.Selector.Parameters[0])
        {
            visitor ??= new ParameterReplacerVisitor(searchExpression.Selector.Parameters[0], mainParam);

            // If there are more than 2 search items, we want to avoid creating a new visitor instance (saving 32 bytes per instance).
            // We're in a sequential loop, no concurrency issues.
            visitor.Update(searchExpression.Selector.Parameters[0], mainParam);
            selectorExpr = visitor.Visit(selectorExpr);
        }

        var patternExpr = StringAsExpression(searchExpression.SearchTerm);

        var likeExpr = Expression.Call(
            null,
            _likeMethodInfo,
            selectorExpr,
            patternExpr);

        combinedExpr = combinedExpr is null
            ? likeExpr
            : Expression.OrElse(combinedExpr, likeExpr);
    }
}

public sealed class ParameterReplacerVisitor : ExpressionVisitor
{
    private ParameterExpression _oldParameter;
    private Expression _newExpression;

    public ParameterReplacerVisitor(ParameterExpression oldParameter, Expression newExpression) =>
        (_oldParameter, _newExpression) = (oldParameter, newExpression);

    internal void Update(ParameterExpression oldParameter, Expression newExpression) =>
        (_oldParameter, _newExpression) = (oldParameter, newExpression);

    protected override Expression VisitParameter(ParameterExpression node) =>
        node == _oldParameter ? _newExpression : node;
}

