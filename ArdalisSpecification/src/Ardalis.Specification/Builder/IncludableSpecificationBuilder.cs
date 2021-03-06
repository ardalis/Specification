using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification
{
    public class IncludableSpecificationBuilder<T, TProperty> : IIncludableSpecificationBuilder<T, TProperty> where T : class
    {
        public Specification<T> Specification { get; }

        public IncludableSpecificationBuilder(Specification<T> specification)
        {
            Specification = specification;
        }
    }
}
