using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification
{
    public interface ISpecificationValidator
    {
        bool IsValid<T>(T entity, ISpecification<T> specification);
    }
}
