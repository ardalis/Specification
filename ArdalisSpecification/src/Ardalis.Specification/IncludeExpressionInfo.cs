using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Text;

namespace Ardalis.Specification
{
    public class IncludeExpressionInfo
    {
        public Expression ExpressionBody { get; }
        public ReadOnlyCollection<ParameterExpression> Parameters { get; }
        public Type EntityType { get; }
        public Type PropertyType { get; }
        public Type? PreviousPropertyType { get; }
        public IncludeTypeEnum Type { get; }

        private IncludeExpressionInfo(Expression expressionBody,
                                      ReadOnlyCollection<ParameterExpression> parameters,
                                      Type entityType,
                                      Type propertyType,
                                      Type? previousPropertyType,
                                      IncludeTypeEnum includeType)

        {
            _ = expressionBody ?? throw new ArgumentNullException(nameof(expressionBody));
            _ = parameters ?? throw new ArgumentNullException(nameof(parameters));
            _ = entityType ?? throw new ArgumentNullException(nameof(entityType));
            _ = propertyType ?? throw new ArgumentNullException(nameof(propertyType));

            if (includeType == IncludeTypeEnum.ThenInclude)
            {
                _ = previousPropertyType ?? throw new ArgumentNullException(nameof(previousPropertyType));
            }

            this.ExpressionBody = expressionBody;
            this.Parameters = parameters;
            this.EntityType = entityType;
            this.PropertyType = propertyType;
            this.PreviousPropertyType = previousPropertyType;
            this.Type = includeType;
        }

        public IncludeExpressionInfo(Expression expressionBody,
                                     ReadOnlyCollection<ParameterExpression> parameters,
                                     Type entityType,
                                     Type propertyType)
            : this(expressionBody, parameters, entityType, propertyType, null, IncludeTypeEnum.Include)
        {
        }

        public IncludeExpressionInfo(Expression expressionBody,
                                     ReadOnlyCollection<ParameterExpression> parameters,
                                     Type entityType,
                                     Type propertyType,
                                     Type previousPropertyType)
            : this(expressionBody, parameters, entityType, propertyType, previousPropertyType, IncludeTypeEnum.ThenInclude)
        {
        }
    }
}
