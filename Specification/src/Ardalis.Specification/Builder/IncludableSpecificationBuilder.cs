namespace Ardalis.Specification;

public class IncludableSpecificationBuilder<T, TProperty>(
    Specification<T> specification,
    bool isChainDiscarded) : IIncludableSpecificationBuilder<T, TProperty> where T : class
{
    public Specification<T> Specification { get; } = specification;
    public bool IsChainDiscarded { get; set; } = isChainDiscarded;

    public IncludableSpecificationBuilder(Specification<T> specification)
        : this(specification, false)
    {
    }
}
