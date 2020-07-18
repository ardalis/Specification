using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification
{
    public interface IIncludeAggregator
    {
        void AddNavigationPropertyName(string navigationPropertyName);
        string IncludeString { get; }
    }
}
