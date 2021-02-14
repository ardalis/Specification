using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Ardalis.Specification
{
    public static class IncludableBuilderExtensions
    {
        public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            this IIncludableSpecificationBuilder<TEntity, TPreviousProperty> previousBuilder,
            Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression)
            where TEntity : class
        {
            var info = new IncludeExpressionInfo(thenIncludeExpression.Body, thenIncludeExpression.Parameters, typeof(TEntity), typeof(TProperty), typeof(TPreviousProperty));

            ((List<IncludeExpressionInfo>)previousBuilder.Specification.IncludeExpressions).Add(info);

            var includeBuilder = new IncludableSpecificationBuilder<TEntity, TProperty>(previousBuilder.Specification);

            return includeBuilder;
        }

        public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            this IIncludableSpecificationBuilder<TEntity, IEnumerable<TPreviousProperty>> previousBuilder,
            Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression)
            where TEntity : class
        {
            var info = new IncludeExpressionInfo(thenIncludeExpression.Body, thenIncludeExpression.Parameters, typeof(TEntity), typeof(TProperty), typeof(TPreviousProperty));

            ((List<IncludeExpressionInfo>)previousBuilder.Specification.IncludeExpressions).Add(info);

            var includeBuilder = new IncludableSpecificationBuilder<TEntity, TProperty>(previousBuilder.Specification);

            return includeBuilder;
        }
    }
}
