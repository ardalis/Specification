using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification
{
    public class IncludableSpecificationBuilder<T, TProperty> : IIncludableSpecificationBuilder<T, TProperty>
    {
        public IIncludeAggregator Aggregator { get; }

        public IncludableSpecificationBuilder(IIncludeAggregator aggregator)
        {
            Aggregator = aggregator;
        }
    }
}
