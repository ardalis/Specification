using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ardalis.Specification.EntityFramework6;

public static class IncludeExtensions
{
    public static IQueryable<T> Include<T>(this IQueryable<T> source, IncludeExpressionInfo info)
    {
        _ = info ?? throw new ArgumentNullException(nameof(info));
        var propertyName = GetPropertyName(info.LambdaExpression);

        return QueryableExtensions.Include(source, propertyName);
    }

    public static IQueryable<T> ThenInclude<T>(this IQueryable<T> source, IncludeExpressionInfo info)
    {
        _ = info ?? throw new ArgumentNullException(nameof(info));
        _ = info.PreviousPropertyType ?? throw new ArgumentNullException(nameof(info.PreviousPropertyType));

        var exp = source.Expression as MethodCallExpression;
        var arg = exp.Arguments[0] as ConstantExpression;

        string previousPropertyName;
        if (arg.Value is string)
        {
            previousPropertyName = arg.Value.ToString();
        }
        else
        {
            // System.Data.Entity.Core.Objects.Span is an internal class, so here's some reflection to get to the previous property.

            var propertyInfo = arg.Value.GetType().GetProperty("SpanList", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var spanList = propertyInfo.GetValue(arg.Value);

            // Get the first item of the span list
            propertyInfo = propertyInfo.PropertyType.GetProperty("Item");
            var spanPath = propertyInfo.GetValue(spanList, new object[] { 0 });

            var fieldInfo = spanPath.GetType().GetField("Navigations", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var navigations = fieldInfo.GetValue(spanPath) as List<string>;
            previousPropertyName = string.Join(".", navigations);
        }

        var propertyName = GetPropertyName(info.LambdaExpression);

        return QueryableExtensions.Include(source, $"{previousPropertyName}.{propertyName}");
    }

    private static string GetPropertyName(this Expression propertySelector, char delimiter = '.', char endTrim = ')')
    {

        var asString = propertySelector.ToString();
        var firstDelim = asString.IndexOf(delimiter);

        return firstDelim < 0
            ? asString
            : asString.Substring(firstDelim + 1).TrimEnd(endTrim);
    }
}
