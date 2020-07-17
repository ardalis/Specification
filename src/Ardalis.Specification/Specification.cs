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

        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPagingEnabled { get; private set; } = false;


        protected class SpecificationBuilder<TSource> : ISpecificationBuilder<TSource>
        {
            private readonly Specification<TSource> parent;

            public SpecificationBuilder(Specification<TSource> parent)
            {
                this.parent = parent;
            }

            public ISpecificationBuilder<TSource> Where(Expression<Func<TSource, bool>> criteria)
            {
                ((List<Expression<Func<TSource, bool>>>)parent.WhereExpressions).Add(criteria);
                return this;
            }

            public ISpecificationBuilder<TSource> Paginate(int skip, int take)
            {
                parent.Skip = skip;
                parent.Take = take;
                parent.IsPagingEnabled = true;
                return this;
            }
        }
    }
}
