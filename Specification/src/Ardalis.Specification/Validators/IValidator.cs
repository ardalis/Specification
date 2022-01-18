using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification
{
  public interface IValidator
  {
    bool IsValid<T>(T entity, ISpecification<T> specification);
  }
}
