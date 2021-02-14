using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Ardalis.Specification.EntityFrameworkCore
{
    public static class IncludeExtensions
    {
        public static IQueryable<T> Include<T>(this IQueryable<T> source, IncludeExpressionInfo info)
        {
            _ = info ?? throw new ArgumentNullException(nameof(info));

            var expression = Expression.Lambda(info.ExpressionBody, info.Parameters);

            var queryExpr = Expression.Call(
                typeof(EntityFrameworkQueryableExtensions),
                "Include",
                new Type[] {
                    info.EntityType,
                    info.PropertyType
                },
                source.Expression,
                expression
                );

            return source.Provider.CreateQuery<T>(queryExpr);
        }
        public static IQueryable<T> ThenInclude<T>(this IQueryable<T> source, IncludeExpressionInfo info)
        {
            _ = info ?? throw new ArgumentNullException(nameof(info));
            _ = info.PreviousPropertyType ?? throw new ArgumentNullException(nameof(info.PreviousPropertyType));

            var expression = Expression.Lambda(info.ExpressionBody, info.Parameters);

            var queryExpr = Expression.Call(
                typeof(EntityFrameworkQueryableExtensions),
                "ThenInclude",
                new Type[] {
                    info.EntityType,
                    info.PreviousPropertyType,
                    info.PropertyType
                },
                source.Expression,
                expression
                );

            return source.Provider.CreateQuery<T>(queryExpr);
        }
    }
}
