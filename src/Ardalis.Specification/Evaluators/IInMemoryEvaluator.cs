using System.Collections.Generic;

namespace Ardalis.Specification;

public interface IInMemoryEvaluator
{
    IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification);
}
