using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification
{
    public class IncludableSpecificationBuilder<T, TProperty> : IIncludableSpecificationBuilder<T, TProperty>
    {
        public Specification<T> Specification { get; }

        public IIncludeAggregator Aggregator { get; }

        public IncludableSpecificationBuilder(Specification<T> specification, IIncludeAggregator aggregator)
        {
            Specification = specification;
            Aggregator = aggregator;
        }
    }
}
