using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Ardalis.Specification.EntityFrameworkCore
{
    public class IncludeEvaluator : IEvaluator
    {
        private static readonly MethodInfo IncludeMethodInfo = typeof(EntityFrameworkQueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.Include))
            .Single(mi => mi.GetGenericArguments().Length == 2
                && mi.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IQueryable<>)
                && mi.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>));

        private static readonly MethodInfo ThenIncludeAfterReferenceMethodInfo
            = typeof(EntityFrameworkQueryableExtensions)
                .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.ThenInclude))
                .Single(mi => mi.GetGenericArguments().Length == 3
                    && mi.GetParameters()[0].ParameterType.GenericTypeArguments[1].IsGenericParameter
                    && mi.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IIncludableQueryable<,>)
                    && mi.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>));

        private static readonly MethodInfo ThenIncludeAfterEnumerableMethodInfo
            = typeof(EntityFrameworkQueryableExtensions)
                .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.ThenInclude))
                .Where(mi => mi.GetGenericArguments().Length == 3)
                .Single(
                    mi =>
                    {
                        var typeInfo = mi.GetParameters()[0].ParameterType.GenericTypeArguments[1];

                        return typeInfo.IsGenericType
                            && typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                            && mi.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IIncludableQueryable<,>)
                            && mi.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>);
                    });

        private static readonly CachedReadConcurrentDictionary<(Type EntityType, Type PropertyType, Type? PreviousPropertyType), Lazy<Func<IQueryable, LambdaExpression, IQueryable>>> DelegatesCache =
            new CachedReadConcurrentDictionary<(Type EntityType, Type PropertyType, Type? PreviousPropertyType), Lazy<Func<IQueryable, LambdaExpression, IQueryable>>>();

        private readonly bool cacheEnabled;

        private IncludeEvaluator(bool cacheEnabled)
        {
            this.cacheEnabled = cacheEnabled;
        }

        /// <summary>
        /// <see cref="IncludeEvaluator"/> instance without any additional features.
        /// </summary>
        public static IncludeEvaluator Default { get; } = new IncludeEvaluator(false);

        /// <summary>
        /// <see cref="IncludeEvaluator"/> instance with caching to provide better performance.
        /// </summary>
        public static IncludeEvaluator Cached { get; } = new IncludeEvaluator(true);

        public bool IsCriteriaEvaluator => false;

        public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
        {
            foreach (var includeString in specification.IncludeStrings)
            {
                query = query.Include(includeString);
            }

            foreach (var includeInfo in specification.IncludeExpressions)
            {
                if (includeInfo.Type == IncludeTypeEnum.Include)
                {
                    query = this.BuildInclude<T>(query, includeInfo);
                }
                else if (includeInfo.Type == IncludeTypeEnum.ThenInclude)
                {
                    query = this.BuildThenInclude<T>(query, includeInfo);
                }
            }

            return query;
        }

        private IQueryable<T> BuildInclude<T>(IQueryable query, IncludeExpressionInfo includeInfo)
        {
            _ = includeInfo ?? throw new ArgumentNullException(nameof(includeInfo));

            if (!this.cacheEnabled)
            {
                return (IQueryable<T>)IncludeMethodInfo.MakeGenericMethod(includeInfo.EntityType, includeInfo.PropertyType).Invoke(null, new object[] { query, includeInfo.LambdaExpression, });
            }

            var include = DelegatesCache.GetOrAdd((includeInfo.EntityType, includeInfo.PropertyType, null), CreateIncludeDelegate).Value;

            return (IQueryable<T>)include(query, includeInfo.LambdaExpression);
        }

        private IQueryable<T> BuildThenInclude<T>(IQueryable query, IncludeExpressionInfo includeInfo)
        {
            _ = includeInfo ?? throw new ArgumentNullException(nameof(includeInfo));
            _ = includeInfo.PreviousPropertyType ?? throw new ArgumentNullException(nameof(includeInfo.PreviousPropertyType));

            if (!this.cacheEnabled)
            {
                return (IQueryable<T>)(IsGenericEnumerable(includeInfo.PreviousPropertyType, out var previousPropertyType)
                        ? ThenIncludeAfterEnumerableMethodInfo
                        : ThenIncludeAfterReferenceMethodInfo).MakeGenericMethod(includeInfo.EntityType, previousPropertyType, includeInfo.PropertyType)
                    .Invoke(null, new object[] { query, includeInfo.LambdaExpression, });
            }

            var thenInclude = DelegatesCache.GetOrAdd((includeInfo.EntityType, includeInfo.PropertyType, includeInfo.PreviousPropertyType), CreateThenIncludeDelegate).Value;

            return (IQueryable<T>)thenInclude(query, includeInfo.LambdaExpression);
        }

        // (source, selector) => EntityFrameworkQueryableExtensions.Include<TEntity, TProperty>((IQueryable<TEntity>)source, (Expression<Func<TEntity, TProperty>>)selector);
        private static Lazy<Func<IQueryable, LambdaExpression, IQueryable>> CreateIncludeDelegate((Type EntityType, Type PropertyType, Type? PreviousPropertyType) cacheKey)
            => new Lazy<Func<IQueryable, LambdaExpression, IQueryable>>(() =>
            {
                var concreteInclude = IncludeMethodInfo.MakeGenericMethod(cacheKey.EntityType, cacheKey.PropertyType);
                var sourceParameter = Expression.Parameter(typeof(IQueryable));
                var selectorParameter = Expression.Parameter(typeof(LambdaExpression));

                var call = Expression.Call(
                    concreteInclude,
                    Expression.Convert(sourceParameter, typeof(IQueryable<>).MakeGenericType(cacheKey.EntityType)),
                    Expression.Convert(selectorParameter, typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(cacheKey.EntityType, cacheKey.PropertyType))));

                var lambda = Expression.Lambda<Func<IQueryable, LambdaExpression, IQueryable>>(call, sourceParameter, selectorParameter);

                return lambda.Compile();
            });

        // ((source, selector) =>
        //     EntityFrameworkQueryableExtensions.ThenInclude<TEntity, TPreviousProperty, TProperty>(
        //         (IIncludableQueryable<TEntity, TPreviousProperty>)source,
        //         (Expression<Func<TPreviousProperty, TProperty>>)selector);
        // (source, selector) =>
        //     EntityFrameworkQueryableExtensions.ThenInclude<TEntity, TPreviousProperty, TProperty>(
        //         (IIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>>)queryable,
        //         (Expression<Func<TPreviousProperty, TProperty>>)lambdaExpression);
        private static Lazy<Func<IQueryable, LambdaExpression, IQueryable>> CreateThenIncludeDelegate((Type EntityType, Type PropertyType, Type? PreviousPropertyType) cacheKey)
            => new Lazy<Func<IQueryable, LambdaExpression, IQueryable>>(() =>
            {
                MethodInfo thenIncludeInfo = ThenIncludeAfterReferenceMethodInfo;
                if (IsGenericEnumerable(cacheKey.PreviousPropertyType!, out var previousPropertyType))
                {
                    thenIncludeInfo = ThenIncludeAfterEnumerableMethodInfo;
                }

                var concreteThenInclude = thenIncludeInfo.MakeGenericMethod(cacheKey.EntityType, previousPropertyType, cacheKey.PropertyType);
                var sourceParameter = Expression.Parameter(typeof(IQueryable));
                var selectorParameter = Expression.Parameter(typeof(LambdaExpression));

                var call = Expression.Call(
                    concreteThenInclude,
                    Expression.Convert(
                        sourceParameter,
                        typeof(IIncludableQueryable<,>).MakeGenericType(cacheKey.EntityType, cacheKey.PreviousPropertyType)), // cacheKey.PreviousPropertyType must be exact type, not generic type argument
                    Expression.Convert(selectorParameter, typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(previousPropertyType, cacheKey.PropertyType))));

                var lambda = Expression.Lambda<Func<IQueryable, LambdaExpression, IQueryable>>(call, sourceParameter, selectorParameter);

                return lambda.Compile();
            });

        private static bool IsGenericEnumerable(Type type, out Type propertyType)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                propertyType = type.GenericTypeArguments[0];

                return true;
            }

            propertyType = type;

            return false;
        }
    }
}
