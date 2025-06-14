using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace Ardalis.Specification.EntityFrameworkCore;

public static class SearchExtension
{
    // We'll name the property Format just so we match the produced SQL query parameter name (in case of interpolated strings).
    private record StringVar(string Format);
    private static readonly PropertyInfo _stringFormatProperty = typeof(StringVar).GetProperty(nameof(StringVar.Format))!;
    private static readonly MemberExpression _functions = Expression.Property(null, typeof(EF).GetProperty(nameof(EF.Functions))!);
    private static readonly MethodInfo _likeMethodInfo = typeof(DbFunctionsExtensions)
        .GetMethod(nameof(DbFunctionsExtensions.Like), [typeof(DbFunctions), typeof(string), typeof(string)])!;

    // It's required so EF can generate parameterized query.
    // In the past I've been creating closures for this, e.g. var patternAsExpression = ((Expression<Func<string>>)(() => pattern)).Body;
    // But, that allocates 168 bytes. So, this is more efficient way.
    private static MemberExpression StringAsExpression(string value)
        => Expression.Property(Expression.Constant(new StringVar(value)), _stringFormatProperty);

    public static IQueryable<T> ApplySingleLike<T>(this IQueryable<T> source, SearchExpressionInfo<T> searchExpression)
    {
        Debug.Assert(_likeMethodInfo is not null);

        var param = searchExpression.Selector.Parameters[0];
        var selectorExpr = searchExpression.Selector.Body;
        var patternExpr = StringAsExpression(searchExpression.SearchTerm);

        var likeExpr = Expression.Call(
            null,
            _likeMethodInfo,
            _functions,
            selectorExpr,
            patternExpr);

        return source.Where(Expression.Lambda<Func<T, bool>>(likeExpr, param));
    }

#if NET9_0_OR_GREATER
    [System.Runtime.CompilerServices.OverloadResolutionPriority(1)]
#endif
    public static IQueryable<T> ApplyLikesAsOrGroup<T>(this IQueryable<T> source, ReadOnlySpan<SearchExpressionInfo<T>> searchExpressions)
    {
        Debug.Assert(_likeMethodInfo is not null);

        Expression? combinedExpr = null;
        ParameterExpression? mainParam = null;
        ParameterReplacerVisitor? visitor = null;

        foreach (var searchExpression in searchExpressions)
        {
            ApplyLikeAsOrGroup(ref combinedExpr, ref mainParam, ref visitor, searchExpression);
        }

        return combinedExpr is null || mainParam is null
            ? source
            : source.Where(Expression.Lambda<Func<T, bool>>(combinedExpr, mainParam));
    }

    public static IQueryable<T> ApplyLikesAsOrGroup<T>(this IQueryable<T> source, IEnumerable<SearchExpressionInfo<T>> searchExpressions)
    {
        Debug.Assert(_likeMethodInfo is not null);

        Expression? combinedExpr = null;
        ParameterExpression? mainParam = null;
        ParameterReplacerVisitor? visitor = null;

        foreach (var searchExpression in searchExpressions)
        {
            ApplyLikeAsOrGroup(ref combinedExpr, ref mainParam, ref visitor, searchExpression);
        }

        return combinedExpr is null || mainParam is null
            ? source
            : source.Where(Expression.Lambda<Func<T, bool>>(combinedExpr, mainParam));
    }

    private static void ApplyLikeAsOrGroup<T>(
        ref Expression? combinedExpr,
        ref ParameterExpression? mainParam,
        ref ParameterReplacerVisitor? visitor,
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
            _functions,
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

