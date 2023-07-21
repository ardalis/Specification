namespace Ardalis.Specification;

public interface IOrderedSpecificationBuilder<T> : ISpecificationBuilder<T>
{
    bool IsChainDiscarded { get; set; }
}
