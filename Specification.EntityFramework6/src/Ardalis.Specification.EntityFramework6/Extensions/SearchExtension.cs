using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Ardalis.Specification.EntityFramework6;

public static class SearchExtension
{
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
        Expression expr = null;
        var parameter = Expression.Parameter(typeof(T), "x");

        foreach (var criteria in criterias)
        {
            var (selector, searchTerm) = (criteria.Selector, criteria.SearchTerm);
            if (string.IsNullOrEmpty(criteria.SearchTerm))
            {
                continue;
            }

            var like = typeof(DbFunctions).GetMethod(nameof(DbFunctions.Like), new Type[] { typeof(string), typeof(string) });

            var propertySelector = ParameterReplacerVisitor.Replace(selector, selector.Parameters[0], parameter);

            var likeExpression = Expression.Call(
                                    null,
                                    like,
                                    (propertySelector as LambdaExpression)?.Body,
                                    Expression.Constant(searchTerm));

            expr = expr == null ? (Expression)likeExpression : Expression.OrElse(expr, likeExpression);
        }

        return expr == null
            ? source
            : source.Where(Expression.Lambda<Func<T, bool>>(expr, parameter));
    }
}
