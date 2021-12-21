using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Ardalis.Specification
{
    public class OrderedSpecificationBuilder<T> : IOrderedSpecificationBuilder<T>
    {
        public Specification<T> Specification { get; }
        public bool IsChainDiscarded { get; set; }

        public OrderedSpecificationBuilder(Specification<T> specification)
            :this(specification, false)
        {
        }

        public OrderedSpecificationBuilder(Specification<T> specification, bool isChainDiscarded)
        {
            this.Specification = specification;
            this.IsChainDiscarded = isChainDiscarded;
        }
    }
}
