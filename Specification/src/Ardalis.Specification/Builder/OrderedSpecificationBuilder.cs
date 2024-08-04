namespace Ardalis.Specification;

public class OrderedSpecificationBuilder<T>(
    Specification<T> specification,
    bool isChainDiscarded) : IOrderedSpecificationBuilder<T>
{
    public Specification<T> Specification { get; } = specification;
    public bool IsChainDiscarded { get; set; } = isChainDiscarded;

    public OrderedSpecificationBuilder(Specification<T> specification)
        : this(specification, false)
    {
    }
}
