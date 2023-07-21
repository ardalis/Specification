using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Reflection;

namespace Ardalis.Specification.EntityFrameworkCore;

public class IncludeEvaluator : IEvaluator
{
    private static readonly MethodInfo _includeMethodInfo = typeof(EntityFrameworkQueryableExtensions)
        .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.Include))
        .Single(mi => mi.GetGenericArguments().Length == 2
            && mi.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IQueryable<>)
            && mi.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>));

    private static readonly MethodInfo _thenIncludeAfterReferenceMethodInfo
        = typeof(EntityFrameworkQueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.ThenInclude))
            .Single(mi => mi.GetGenericArguments().Length == 3
                && mi.GetParameters()[0].ParameterType.GenericTypeArguments[1].IsGenericParameter
                && mi.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IIncludableQueryable<,>)
                && mi.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>));

    private static readonly MethodInfo _thenIncludeAfterEnumerableMethodInfo
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

    private static readonly CachedReadConcurrentDictionary<(Type EntityType, Type PropertyType, Type? PreviousPropertyType), Lazy<Func<IQueryable, LambdaExpression, IQueryable>>> _delegatesCache = new();

    private readonly bool _cacheEnabled;

    private IncludeEvaluator(bool cacheEnabled)
    {
        _cacheEnabled = cacheEnabled;
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
                query = BuildInclude<T>(query, includeInfo);
            }
            else if (includeInfo.Type == IncludeTypeEnum.ThenInclude)
            {
                query = BuildThenInclude<T>(query, includeInfo);
            }
        }

        return query;
    }

    private IQueryable<T> BuildInclude<T>(IQueryable query, IncludeExpressionInfo includeInfo)
    {
        _ = includeInfo ?? throw new ArgumentNullException(nameof(includeInfo));

        if (!_cacheEnabled)
        {
            var result = _includeMethodInfo.MakeGenericMethod(includeInfo.EntityType, includeInfo.PropertyType).Invoke(null, new object[] { query, includeInfo.LambdaExpression });

            _ = result ?? throw new TargetException();

            return (IQueryable<T>)result;
        }

        var include = _delegatesCache.GetOrAdd((includeInfo.EntityType, includeInfo.PropertyType, null), CreateIncludeDelegate).Value;

        return (IQueryable<T>)include(query, includeInfo.LambdaExpression);
    }

    private IQueryable<T> BuildThenInclude<T>(IQueryable query, IncludeExpressionInfo includeInfo)
    {
        _ = includeInfo ?? throw new ArgumentNullException(nameof(includeInfo));
        _ = includeInfo.PreviousPropertyType ?? throw new ArgumentNullException(nameof(includeInfo.PreviousPropertyType));

        if (!_cacheEnabled)
        {
            var result = (IsGenericEnumerable(includeInfo.PreviousPropertyType, out var previousPropertyType)
                    ? _thenIncludeAfterEnumerableMethodInfo
                    : _thenIncludeAfterReferenceMethodInfo).MakeGenericMethod(includeInfo.EntityType, previousPropertyType, includeInfo.PropertyType)
                .Invoke(null, new object[] { query, includeInfo.LambdaExpression, });

            _ = result ?? throw new TargetException();

            return (IQueryable<T>)result;
        }

        var thenInclude = _delegatesCache.GetOrAdd((includeInfo.EntityType, includeInfo.PropertyType, includeInfo.PreviousPropertyType), CreateThenIncludeDelegate).Value;

        return (IQueryable<T>)thenInclude(query, includeInfo.LambdaExpression);
    }

    // (source, selector) => EntityFrameworkQueryableExtensions.Include<TEntity, TProperty>((IQueryable<TEntity>)source, (Expression<Func<TEntity, TProperty>>)selector);
    private static Lazy<Func<IQueryable, LambdaExpression, IQueryable>> CreateIncludeDelegate((Type EntityType, Type PropertyType, Type? PreviousPropertyType) cacheKey)
        => new(() =>
        {
            var concreteInclude = _includeMethodInfo.MakeGenericMethod(cacheKey.EntityType, cacheKey.PropertyType);
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
    //         (IIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>>)source,
    //         (Expression<Func<TPreviousProperty, TProperty>>)selector);
    private static Lazy<Func<IQueryable, LambdaExpression, IQueryable>> CreateThenIncludeDelegate((Type EntityType, Type PropertyType, Type? PreviousPropertyType) cacheKey)
        => new(() =>
        {
            _ = cacheKey.PreviousPropertyType ?? throw new ArgumentNullException(nameof(cacheKey.PreviousPropertyType));

            var thenIncludeInfo = _thenIncludeAfterReferenceMethodInfo;
            if (IsGenericEnumerable(cacheKey.PreviousPropertyType, out var previousPropertyType))
            {
                thenIncludeInfo = _thenIncludeAfterEnumerableMethodInfo;
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
