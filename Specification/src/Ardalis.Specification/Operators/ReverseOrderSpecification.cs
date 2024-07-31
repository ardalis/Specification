using System.Linq;

namespace Ardalis.Specification.Operators
{
    public sealed class ReverseOrderSpecification<T> : CompositeSpecification<T> where T:class
    {
        public ReverseOrderSpecification(Specification<T> specification)
        {
            CombineWhereExpressions(specification, new FalseSpecification<T>());
            CombineIncludeAggregators(specification, new FalseSpecification<T>());
            CombineOrderExpressions(specification,new FalseSpecification<T>());
        }

        protected override void CombineWhereExpressions(Specification<T> leftSpec, Specification<T> rightSpec)
        {
            var expressions = leftSpec.WhereExpressions.ToList();
            if (expressions.Any())
            {
                // var leftSpecExpr = ReduceExpressions(expressions);
                // Query.Where(leftSpecExpr);
            }
        }

        protected override void CombineOrderExpressions(Specification<T> spec1, Specification<T> s)
        {
            
            var orderSpecs=spec1.OrderExpressions.ToList();
            if(!orderSpecs.Any())
                return;
            var orderByFirst = orderSpecs.First();
            var orderByRest = orderSpecs.Skip(1);
            IOrderedSpecificationBuilder<T> orderQuery = null;
            switch (orderByFirst.OrderType)
            {
                case OrderTypeEnum.OrderBy:
                case OrderTypeEnum.ThenBy:
                    //Reverse ordering
                    orderQuery = Query.OrderByDescending(orderByFirst.KeySelector);
                    break;
                case OrderTypeEnum.OrderByDescending:
                case OrderTypeEnum.ThenByDescending:
                    //Reverse ordering
                    orderQuery = Query.OrderBy(orderByFirst.KeySelector);
                    break;
            }
            foreach (var rest in orderByRest)
            {
                switch (rest.OrderType)
                {
                    case OrderTypeEnum.OrderBy:
                    case OrderTypeEnum.ThenBy:
                        //Reverse ordering
                        orderQuery=orderQuery?.ThenByDescending(rest.KeySelector);
                        break;
                    case OrderTypeEnum.OrderByDescending:
                    case OrderTypeEnum.ThenByDescending:
                        //Reverse ordering
                        orderQuery=orderQuery?.ThenBy(rest.KeySelector);
                        break;
                }
            }
        }
    }
}
