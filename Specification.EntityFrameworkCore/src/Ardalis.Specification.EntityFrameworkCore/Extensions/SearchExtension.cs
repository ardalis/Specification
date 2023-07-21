using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace Ardalis.Specification.EntityFrameworkCore;

public static class SearchExtension
{
    private static readonly MethodInfo _likeMethodInfo = typeof(DbFunctionsExtensions)
        .GetMethod(nameof(DbFunctionsExtensions.Like), new Type[] { typeof(DbFunctions), typeof(string), typeof(string) })
        ?? throw new TargetException("The EF.Functions.Like not found");

    private static readonly MemberExpression _functions = Expression.Property(null, typeof(EF).GetProperty(nameof(EF.Functions))
        ?? throw new TargetException("The EF.Functions not found!"));

    /// <summary>
    /// Filters <paramref name="source"/> by applying an 'SQL LIKE' operation to it.
    /// </summary>
    /// <typeparam name="T">The type being queried against.</typeparam>
    /// <param name="source">The sequence of <typeparamref name="T"/></param>
    /// <param name="criterias">
    /// <list type="bullet">
    ///     <item>Selector, the property to apply the SQL LIKE against.</item>
    ///     <item>SearchTerm, the value to use for the SQL LIKE.</item>
    /// </list>
    /// </param>
    /// <returns></returns>
    public static IQueryable<T> Search<T>(this IQueryable<T> source, IEnumerable<SearchExpressionInfo<T>> criterias)
    {
        Expression? expr = null;
        var parameter = Expression.Parameter(typeof(T), "x");

        foreach (var criteria in criterias)
        {
            if (string.IsNullOrEmpty(criteria.SearchTerm))
                continue;

            var propertySelector = ParameterReplacerVisitor.Replace(criteria.Selector, criteria.Selector.Parameters[0], parameter) as LambdaExpression;
            _ = propertySelector ?? throw new InvalidExpressionException();

            // Create a closure
            var searchTermAsExpression = ((Expression<Func<string>>)(() => criteria.SearchTerm)).Body;

            var likeExpression = Expression.Call(
                                    null,
                                    _likeMethodInfo,
                                    _functions,
                                    propertySelector.Body,
                                    searchTermAsExpression);

            expr = expr == null ? (Expression)likeExpression : Expression.OrElse(expr, likeExpression);
        }

        return expr == null
            ? source
            : source.Where(Expression.Lambda<Func<T, bool>>(expr, parameter));
    }
}
