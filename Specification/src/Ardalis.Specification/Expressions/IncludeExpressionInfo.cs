using System;
using System.Linq.Expressions;

namespace Ardalis.Specification;

/// <summary>
/// Encapsulates data needed to build Include/ThenInclude query.
/// </summary>
public class IncludeExpressionInfo
{
    /// <summary>
    /// If <see cref="Type" /> is <see cref="IncludeTypeEnum.Include" />, represents a related entity that should be included.<para />
    /// If <see cref="Type" /> is <see cref="IncludeTypeEnum.ThenInclude" />, represents a related entity that should be included as part of the previously included entity.
    /// </summary>
    public LambdaExpression LambdaExpression { get; }

    /// <summary>
    /// The type of the source entity.
    /// </summary>
    public Type EntityType { get; }

    /// <summary>
    /// The type of the included entity.
    /// </summary>
    public Type PropertyType { get; }

    /// <summary>
    /// The type of the previously included entity.
    /// </summary>
    public Type? PreviousPropertyType { get; }

    /// <summary>
    /// The include type.
    /// </summary>
    public IncludeTypeEnum Type { get; }

    private IncludeExpressionInfo(LambdaExpression expression,
                                  Type entityType,
                                  Type propertyType,
                                  Type? previousPropertyType,
                                  IncludeTypeEnum includeType)

    {
        _ = expression ?? throw new ArgumentNullException(nameof(expression));
        _ = entityType ?? throw new ArgumentNullException(nameof(entityType));
        _ = propertyType ?? throw new ArgumentNullException(nameof(propertyType));

        if (includeType == IncludeTypeEnum.ThenInclude)
        {
            _ = previousPropertyType ?? throw new ArgumentNullException(nameof(previousPropertyType));
        }

        LambdaExpression = expression;
        EntityType = entityType;
        PropertyType = propertyType;
        PreviousPropertyType = previousPropertyType;
        Type = includeType;
    }

    /// <summary>
    /// Creates instance of <see cref="IncludeExpressionInfo" /> which describes 'Include' query part.<para />
    /// Source (entityType) -> Include (propertyType).
    /// </summary>
    /// <param name="expression">The expression represents a related entity that should be included.</param>
    /// <param name="entityType">The type of the source entity.</param>
    /// <param name="propertyType">The type of the included entity.</param>
    public IncludeExpressionInfo(LambdaExpression expression,
                                 Type entityType,
                                 Type propertyType)
        : this(expression, entityType, propertyType, null, IncludeTypeEnum.Include)
    {
    }

    /// <summary>
    /// Creates instance of <see cref="IncludeExpressionInfo" /> which describes 'ThenInclude' query part.<para />
    /// Source (entityType) -> Include (previousPropertyType) -> ThenInclude (propertyType).
    /// </summary>
    /// <param name="expression">The expression represents a related entity that should be included as part of the previously included entity.</param>
    /// <param name="entityType">The type of the source entity.</param>
    /// <param name="propertyType">The type of the included entity.</param>
    /// <param name="previousPropertyType">The type of the previously included entity.</param>
    public IncludeExpressionInfo(LambdaExpression expression,
                                 Type entityType,
                                 Type propertyType,
                                 Type previousPropertyType)
        : this(expression, entityType, propertyType, previousPropertyType, IncludeTypeEnum.ThenInclude)
    {
    }
}
