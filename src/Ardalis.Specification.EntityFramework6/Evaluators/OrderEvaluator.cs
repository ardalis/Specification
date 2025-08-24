namespace Ardalis.Specification.EntityFramework6;

public class OrderEvaluator : IEvaluator
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
                    OrderTypeEnum.OrderBy => Queryable.OrderBy(query, (dynamic)orderExpression.KeySelector.RemoveConvert()),
                    OrderTypeEnum.OrderByDescending => Queryable.OrderByDescending(query, (dynamic)orderExpression.KeySelector.RemoveConvert()),
                    _ => query
                };
            }
        }

        IOrderedQueryable<T> orderedQuery = null;
        var chainCount = 0;
        foreach (var orderExpression in specification.OrderExpressions)
        {
            if (orderExpression.OrderType == OrderTypeEnum.OrderBy)
            {
                chainCount++;
                if (chainCount == 2) throw new DuplicateOrderChainException();
                orderedQuery = Queryable.OrderBy(query, (dynamic)orderExpression.KeySelector.RemoveConvert());
            }
            else if (orderExpression.OrderType == OrderTypeEnum.OrderByDescending)
            {
                chainCount++;
                if (chainCount == 2) throw new DuplicateOrderChainException();
                orderedQuery = Queryable.OrderByDescending(query, (dynamic)orderExpression.KeySelector.RemoveConvert());
            }
            else if (orderExpression.OrderType == OrderTypeEnum.ThenBy)
            {
                orderedQuery = Queryable.ThenBy(orderedQuery, (dynamic)orderExpression.KeySelector.RemoveConvert());
            }
            else if (orderExpression.OrderType == OrderTypeEnum.ThenByDescending)
            {
                orderedQuery = Queryable.ThenByDescending(orderedQuery, (dynamic)orderExpression.KeySelector.RemoveConvert());
            }
        }

        if (orderedQuery is not null)
        {
            query = orderedQuery;
        }

        return query;
    }
}
