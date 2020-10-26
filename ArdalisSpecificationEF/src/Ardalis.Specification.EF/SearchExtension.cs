using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Ardalis.Specification.EntityFrameworkCore
{
    public static class SearchExtension
    {
        public static IQueryable<T> Search<T>(this IQueryable<T> source, IEnumerable<(Expression<Func<T, string>> selector, string searchTerm)> criterias)
        {
            Expression? expr = null;
            var parameter = Expression.Parameter(typeof(T), "x");

            foreach (var criteria in criterias)
            {
                if (criteria.selector == null || string.IsNullOrEmpty(criteria.searchTerm))
                    continue;

                var functions = Expression.Property(null, typeof(EF).GetProperty(nameof(EF.Functions)));
                var like = typeof(DbFunctionsExtensions).GetMethod(nameof(DbFunctionsExtensions.Like), new Type[] { functions.Type, typeof(string), typeof(string) });

                var propertySelector = ParameterReplacerVisitor.Replace(criteria.selector, criteria.selector.Parameters[0], parameter);

                var likeExpression = Expression.Call(
                                        null,
                                        like,
                                        functions,
                                        (propertySelector as LambdaExpression)?.Body,
                                        Expression.Constant("%" + criteria.searchTerm + "%"));

                expr = expr == null ? (Expression)likeExpression : Expression.OrElse(expr, likeExpression);
            }

            return expr == null
                ? source
                : source.Where(Expression.Lambda<Func<T, bool>>(expr, parameter));
        }
    }
}
