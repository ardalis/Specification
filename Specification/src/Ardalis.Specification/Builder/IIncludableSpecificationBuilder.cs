namespace Ardalis.Specification;

public interface IIncludableSpecificationBuilder<T, out TProperty> : ISpecificationBuilder<T> where T : class
{
    bool IsChainDiscarded { get; set; }
}
