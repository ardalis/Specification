namespace Ardalis.Specification;

public interface ICacheSpecificationBuilder<T> : ISpecificationBuilder<T> where T : class
{
    bool IsChainDiscarded { get; set; }
}
