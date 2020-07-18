using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Ardalis.Specification
{
    public static class IncludableBuilderExtensions
    {
        public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            this IIncludableSpecificationBuilder<TEntity, TPreviousProperty> value,
            Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression)
            where TEntity : class
        {
            var propertyName = (thenIncludeExpression.Body as MemberExpression)?.Member?.Name;
            value.Aggregator.AddNavigationPropertyName(propertyName);

            return new IncludableSpecificationBuilder<TEntity, TProperty>(value.Aggregator);
        }

        public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            this IIncludableSpecificationBuilder<TEntity, IEnumerable<TPreviousProperty>> value,
            Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression)
            where TEntity : class
        {
            var propertyName = (thenIncludeExpression.Body as MemberExpression)?.Member?.Name;
            value.Aggregator.AddNavigationPropertyName(propertyName);

            return new IncludableSpecificationBuilder<TEntity, TProperty>(value.Aggregator);
        }
    }
}
