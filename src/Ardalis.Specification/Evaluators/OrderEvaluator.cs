namespace Ardalis.Specification;

public class OrderEvaluator : IEvaluator, IInMemoryEvaluator
{
    private OrderEvaluator() { }
    public static OrderEvaluator Instance { get; } = new OrderEvaluator();

    public bool IsCriteriaEvaluator { get; } = false;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        if (specification is Specification<T> spec)
        {
            if (spec.OneOrManyOrderExpressions.IsEmpty) return query;
            if (spec.OneOrManyOrderExpressions.SingleOrDefault is { } orderExpression)
            {
                return orderExpression.OrderType switch
                {
                    OrderTypeEnum.OrderBy => query.OrderBy(orderExpression.KeySelector),
                    OrderTypeEnum.OrderByDescending => query.OrderByDescending(orderExpression.KeySelector),
                    _ => query
                };
            }
        }

        IOrderedQueryable<T>? orderedQuery = null;
        var chainCount = 0;
        foreach (var orderExpression in specification.OrderExpressions)
        {
            if (orderExpression.OrderType == OrderTypeEnum.OrderBy)
            {
                chainCount++;
                if (chainCount == 2) throw new DuplicateOrderChainException();
                orderedQuery = query.OrderBy(orderExpression.KeySelector);
            }
            else if (orderExpression.OrderType == OrderTypeEnum.OrderByDescending)
            {
                chainCount++;
                if (chainCount == 2) throw new DuplicateOrderChainException();
                orderedQuery = query.OrderByDescending(orderExpression.KeySelector);
            }
            else if (orderExpression.OrderType == OrderTypeEnum.ThenBy)
            {
                orderedQuery = orderedQuery!.ThenBy(orderExpression.KeySelector);
            }
            else if (orderExpression.OrderType == OrderTypeEnum.ThenByDescending)
            {
                orderedQuery = orderedQuery!.ThenByDescending(orderExpression.KeySelector);
            }
        }

        if (orderedQuery is not null)
        {
            query = orderedQuery;
        }

        return query;
    }

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification)
    {
        if (specification is Specification<T> spec)
        {
            if (spec.OneOrManyOrderExpressions.IsEmpty) return query;
            if (spec.OneOrManyOrderExpressions.SingleOrDefault is { } orderExpression)
            {
                return orderExpression.OrderType switch
                {
                    OrderTypeEnum.OrderBy => query.OrderBy(orderExpression.KeySelectorFunc),
                    OrderTypeEnum.OrderByDescending => query.OrderByDescending(orderExpression.KeySelectorFunc),
                    _ => query
                };
            }
        }

        IOrderedEnumerable<T>? orderedQuery = null;
        var chainCount = 0;
        foreach (var orderExpression in specification.OrderExpressions)
        {
            if (orderExpression.OrderType == OrderTypeEnum.OrderBy)
            {
                chainCount++;
                if (chainCount == 2) throw new DuplicateOrderChainException();
                orderedQuery = query.OrderBy(orderExpression.KeySelectorFunc);
            }
            else if (orderExpression.OrderType == OrderTypeEnum.OrderByDescending)
            {
                chainCount++;
                if (chainCount == 2) throw new DuplicateOrderChainException();
                orderedQuery = query.OrderByDescending(orderExpression.KeySelectorFunc);
            }
            else if (orderExpression.OrderType == OrderTypeEnum.ThenBy)
            {
                orderedQuery = orderedQuery!.ThenBy(orderExpression.KeySelectorFunc);
            }
            else if (orderExpression.OrderType == OrderTypeEnum.ThenByDescending)
            {
                orderedQuery = orderedQuery!.ThenByDescending(orderExpression.KeySelectorFunc);
            }
        }

        if (orderedQuery is not null)
        {
            query = orderedQuery;
        }

        return query;
    }
}
