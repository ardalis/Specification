namespace Ardalis.Specification;

public class CacheSpecificationBuilder<T>(
    Specification<T> specification, 
    bool isChainDiscarded) : ICacheSpecificationBuilder<T> where T : class
{
    public Specification<T> Specification { get; } = specification;
    public bool IsChainDiscarded { get; set; } = isChainDiscarded;

    public CacheSpecificationBuilder(Specification<T> specification)
        : this(specification, false)
    {
    }
}
