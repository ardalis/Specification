using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification
{
    public interface IIncludableSpecificationBuilder<T, out TProperty> : ISpecificationBuilder<T>
    {
        IIncludeAggregator Aggregator { get; }
    }
}
