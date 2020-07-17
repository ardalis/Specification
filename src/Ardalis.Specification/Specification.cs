using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Ardalis.Specification
{
    public abstract class Specification<T> : ISpecification<T>
    {
        protected virtual ISpecificationBuilder<T> Query { get; set; }

        protected Specification()
        {
            this.Query = new SpecificationBuilder<T>(this);
        }

        public IEnumerable<Expression<Func<T, bool>>> WhereExpressions { get; } = new List<Expression<Func<T, bool>>>();
        public IEnumerable<(Expression<Func<T, object>> KeySelector, OrderTypeEnum OrderType)> OrderExpressions { get; } = 
            new List<(Expression<Func<T, object>> KeySelector, OrderTypeEnum OrderType)>();

        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPagingEnabled { get; private set; } = false;


        protected class SpecificationBuilder<TSource> : ISpecificationBuilder<TSource>
        {
            private readonly Specification<TSource> parent;
            private readonly IOrderedSpecificationBuilder<TSource> orderedSpecificationBuilder;

            public SpecificationBuilder(Specification<TSource> parent)
            {
                this.parent = parent;
                this.orderedSpecificationBuilder = new OrderedSpecificationBuilder<TSource>(parent);
            }

            public ISpecificationBuilder<TSource> Where(Expression<Func<TSource, bool>> criteria)
            {
                ((List<Expression<Func<TSource, bool>>>)parent.WhereExpressions).Add(criteria);
                return this;
            }

            public virtual IOrderedSpecificationBuilder<TSource> OrderBy(Expression<Func<TSource, object>> orderExpression)
            {
                ((List<(Expression<Func<TSource, object>> OrderExpression, OrderTypeEnum OrderType)>)parent.OrderExpressions).Add((orderExpression, OrderTypeEnum.OrderBy));
                return orderedSpecificationBuilder;
            }

            public virtual IOrderedSpecificationBuilder<TSource> OrderByDescending(Expression<Func<TSource, object>> orderExpression)
            {
                ((List<(Expression<Func<TSource, object>> OrderExpression, OrderTypeEnum OrderType)>)parent.OrderExpressions).Add((orderExpression, OrderTypeEnum.OrderByDescending));
                return orderedSpecificationBuilder;
            }

            public ISpecificationBuilder<TSource> Paginate(int skip, int take)
            {
                parent.Skip = skip;
                parent.Take = take;
                parent.IsPagingEnabled = true;
                return this;
            }
        }

        protected class OrderedSpecificationBuilder<TSourceOrdered> : IOrderedSpecificationBuilder<TSourceOrdered>
        {
            private readonly Specification<TSourceOrdered> parent;

            public OrderedSpecificationBuilder(Specification<TSourceOrdered> parent)
            {
                this.parent = parent;
            }

            public virtual IOrderedSpecificationBuilder<TSourceOrdered> ThenBy(Expression<Func<TSourceOrdered, object>> orderExpression)
            {
                ((List<(Expression<Func<TSourceOrdered, object>> OrderExpression, OrderTypeEnum OrderType)>)parent.OrderExpressions).Add((orderExpression, OrderTypeEnum.OrderBy));
                return this;
            }

            public virtual IOrderedSpecificationBuilder<TSourceOrdered> ThenByDescending(Expression<Func<TSourceOrdered, object>> orderExpression)
            {
                ((List<(Expression<Func<TSourceOrdered, object>> OrderExpression, OrderTypeEnum OrderType)>)parent.OrderExpressions).Add((orderExpression, OrderTypeEnum.OrderByDescending));
                return this;
            }
        }
    }
}
