using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ardalis.Specification
{
  public interface IInMemoryEvaluator
  {
    IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification);
  }
}
