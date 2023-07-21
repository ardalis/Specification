using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ardalis.Specification;

public static class IncludableBuilderExtensions
{
    public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        this IIncludableSpecificationBuilder<TEntity, TPreviousProperty> previousBuilder,
        Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression)
        where TEntity : class
        => ThenInclude(previousBuilder, thenIncludeExpression, true);

    public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        this IIncludableSpecificationBuilder<TEntity, TPreviousProperty> previousBuilder,
        Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression,
        bool condition)
        where TEntity : class
    {
        if (condition && !previousBuilder.IsChainDiscarded)
        {
            var info = new IncludeExpressionInfo(thenIncludeExpression, typeof(TEntity), typeof(TProperty), typeof(TPreviousProperty));

            ((List<IncludeExpressionInfo>)previousBuilder.Specification.IncludeExpressions).Add(info);
        }

        var includeBuilder = new IncludableSpecificationBuilder<TEntity, TProperty>(previousBuilder.Specification, !condition || previousBuilder.IsChainDiscarded);

        return includeBuilder;
    }

    public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        this IIncludableSpecificationBuilder<TEntity, IEnumerable<TPreviousProperty>> previousBuilder,
        Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression)
        where TEntity : class
        => ThenInclude(previousBuilder, thenIncludeExpression, true);

    public static IIncludableSpecificationBuilder<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        this IIncludableSpecificationBuilder<TEntity, IEnumerable<TPreviousProperty>> previousBuilder,
        Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression,
        bool condition)
        where TEntity : class
    {
        if (condition && !previousBuilder.IsChainDiscarded)
        {
            var info = new IncludeExpressionInfo(thenIncludeExpression, typeof(TEntity), typeof(TProperty), typeof(IEnumerable<TPreviousProperty>));

            ((List<IncludeExpressionInfo>)previousBuilder.Specification.IncludeExpressions).Add(info);
        }

        var includeBuilder = new IncludableSpecificationBuilder<TEntity, TProperty>(previousBuilder.Specification, !condition || previousBuilder.IsChainDiscarded);

        return includeBuilder;
    }
}
