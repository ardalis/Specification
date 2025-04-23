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
    /// The type of the previously included entity.
    /// </summary>
    public Type? PreviousPropertyType { get; }

    /// <summary>
    /// The include type.
    /// </summary>
    public IncludeTypeEnum Type { get; }

    public IncludeExpressionInfo(LambdaExpression expression)
    {
        _ = expression ?? throw new ArgumentNullException(nameof(expression));

        LambdaExpression = expression;
        PreviousPropertyType = null;
        Type = IncludeTypeEnum.Include;
    }

    public IncludeExpressionInfo(LambdaExpression expression, Type previousPropertyType)
    {
        _ = expression ?? throw new ArgumentNullException(nameof(expression));
        _ = previousPropertyType ?? throw new ArgumentNullException(nameof(previousPropertyType));

        LambdaExpression = expression;
        PreviousPropertyType = previousPropertyType;
        Type = IncludeTypeEnum.ThenInclude;
    }
}
