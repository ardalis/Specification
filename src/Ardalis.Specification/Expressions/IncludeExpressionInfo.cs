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
    /// The include type.
    /// </summary>
    public IncludeTypeEnum Type { get; }

    public IncludeExpressionInfo(LambdaExpression expression, IncludeTypeEnum includeType)
    {
        _ = expression ?? throw new ArgumentNullException(nameof(expression));

        LambdaExpression = expression;
        Type = includeType;
    }
}
