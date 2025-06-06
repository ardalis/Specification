namespace Ardalis.Specification;

public static class SpecificationExtensions
{
    public static Specification<T, TResult> WithProjectionOf<T, TResult>(this ISpecification<T> source, ISpecification<T, TResult> projectionSpec)
    {
        var newSpec = new Specification<T, TResult>();
        source.CopyTo(newSpec);
        newSpec.Selector = projectionSpec.Selector;
        newSpec.SelectorFunc = projectionSpec.SelectorFunc;
        newSpec.SelectorMany = projectionSpec.SelectorMany;
        newSpec.PostProcessingAction = projectionSpec.PostProcessingAction;
        return newSpec;
    }
}
