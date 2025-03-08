using Microsoft.EntityFrameworkCore.Query;
using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;

namespace Ardalis.Specification.EntityFrameworkCore;

public class IncludeEvaluator : IEvaluator
{
    private static readonly MethodInfo _includeMethodInfo = typeof(EntityFrameworkQueryableExtensions)
        .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.Include))
        .Single(mi => mi.IsPublic && mi.GetGenericArguments().Length == 2
            && mi.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IQueryable<>)
            && mi.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>));

    private static readonly MethodInfo _thenIncludeAfterReferenceMethodInfo
        = typeof(EntityFrameworkQueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.ThenInclude))
            .Single(mi => mi.IsPublic && mi.GetGenericArguments().Length == 3
                && mi.GetParameters()[0].ParameterType.GenericTypeArguments[1].IsGenericParameter
                && mi.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IIncludableQueryable<,>)
                && mi.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>));

    private static readonly MethodInfo _thenIncludeAfterEnumerableMethodInfo
        = typeof(EntityFrameworkQueryableExtensions)
            .GetTypeInfo().GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.ThenInclude))
            .Single(mi => mi.IsPublic && mi.GetGenericArguments().Length == 3
                && !mi.GetParameters()[0].ParameterType.GenericTypeArguments[1].IsGenericParameter
                && mi.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(IIncludableQueryable<,>)
                && mi.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>));

    private readonly record struct CacheKey(Type EntityType, Type PropertyType, Type? PreviousReturnType);
    private static readonly ConcurrentDictionary<CacheKey, Func<IQueryable, LambdaExpression, IQueryable>> _cache = new();

    private IncludeEvaluator() { }
    public static IncludeEvaluator Instance = new();

    public bool IsCriteriaEvaluator => false;

    /// <inheritdoc/>
    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        Type? previousReturnType = null;
        foreach (var includeExpression in specification.IncludeExpressions)
        {
            var lambdaExpr = includeExpression.LambdaExpression;

            if (includeExpression.Type == IncludeTypeEnum.Include)
            {
                var key = new CacheKey(typeof(T), lambdaExpr.ReturnType, null);
                previousReturnType = lambdaExpr.ReturnType;
                var include = _cache.GetOrAdd(key, CreateIncludeDelegate);
                query = (IQueryable<T>)include(query, lambdaExpr);
            }
            else if (includeExpression.Type == IncludeTypeEnum.ThenInclude)
            {
                var key = new CacheKey(typeof(T), lambdaExpr.ReturnType, previousReturnType);
                previousReturnType = lambdaExpr.ReturnType;
                var include = _cache.GetOrAdd(key, CreateThenIncludeDelegate);
                query = (IQueryable<T>)include(query, lambdaExpr);
            }
        }

        return query;
    }

    private static Func<IQueryable, LambdaExpression, IQueryable> CreateIncludeDelegate(CacheKey cacheKey)
    {
        var includeMethod = _includeMethodInfo.MakeGenericMethod(cacheKey.EntityType, cacheKey.PropertyType);
        var sourceParameter = Expression.Parameter(typeof(IQueryable));
        var selectorParameter = Expression.Parameter(typeof(LambdaExpression));

        var call = Expression.Call(
              includeMethod,
              Expression.Convert(sourceParameter, typeof(IQueryable<>).MakeGenericType(cacheKey.EntityType)),
              Expression.Convert(selectorParameter, typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(cacheKey.EntityType, cacheKey.PropertyType))));

        var lambda = Expression.Lambda<Func<IQueryable, LambdaExpression, IQueryable>>(call, sourceParameter, selectorParameter);
        return lambda.Compile();
    }

    private static Func<IQueryable, LambdaExpression, IQueryable> CreateThenIncludeDelegate(CacheKey cacheKey)
    {
        Debug.Assert(cacheKey.PreviousReturnType is not null);

        var thenIncludeInfo = IsGenericEnumerable(cacheKey.PreviousReturnType, out var previousPropertyType)
            ? _thenIncludeAfterEnumerableMethodInfo
            : _thenIncludeAfterReferenceMethodInfo;

        var thenIncludeMethod = thenIncludeInfo.MakeGenericMethod(cacheKey.EntityType, previousPropertyType, cacheKey.PropertyType);
        var sourceParameter = Expression.Parameter(typeof(IQueryable));
        var selectorParameter = Expression.Parameter(typeof(LambdaExpression));

        var call = Expression.Call(
                thenIncludeMethod,
                // We must pass cacheKey.PreviousReturnType. It must be exact type, not the generic type argument
                Expression.Convert(sourceParameter, typeof(IIncludableQueryable<,>).MakeGenericType(cacheKey.EntityType, cacheKey.PreviousReturnType)),
                Expression.Convert(selectorParameter, typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(previousPropertyType, cacheKey.PropertyType))));

        var lambda = Expression.Lambda<Func<IQueryable, LambdaExpression, IQueryable>>(call, sourceParameter, selectorParameter);
        return lambda.Compile();
    }

    private static bool IsGenericEnumerable(Type type, out Type propertyType)
    {
        if (type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type))
        {
            propertyType = type.GenericTypeArguments[0];
            return true;
        }

        propertyType = type;
        return false;
    }
}
