namespace Ardalis.Specification;

/// <inheritdoc cref="IInMemorySpecificationEvaluator"/>
public class InMemorySpecificationEvaluator : IInMemorySpecificationEvaluator
{
    /// <summary>
    /// Gets the default singleton instance of the <see cref="InMemorySpecificationEvaluator"/> class.
    /// </summary>
    /// <remarks>This instance is a singleton and is intended for scenarios where a shared, default
    /// configuration is sufficient. If customization is required, a new instance of <see
    /// cref="InMemorySpecificationEvaluator"/> can be created.</remarks>
    public static InMemorySpecificationEvaluator Default { get; } = new InMemorySpecificationEvaluator();

    /// <summary>
    /// List of partial evaluators that will be used to evaluate specifications in memory.
    /// </summary>
    protected List<IInMemoryEvaluator> Evaluators { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InMemorySpecificationEvaluator"/> class.
    /// </summary>
    /// <remarks>This constructor sets up the evaluator with a predefined set of in-memory evaluators.</remarks> 
    public InMemorySpecificationEvaluator()
    {
        Evaluators =
        [
            WhereEvaluator.Instance,
            SearchMemoryEvaluator.Instance,
            OrderEvaluator.Instance,
            PaginationEvaluator.Instance
        ];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InMemorySpecificationEvaluator"/> class  with the specified
    /// collection of in-memory evaluators.
    /// </summary>
    /// <param name="evaluators">A collection of <see cref="IInMemoryEvaluator"/> instances used to evaluate specifications.</param>
    public InMemorySpecificationEvaluator(IEnumerable<IInMemoryEvaluator> evaluators)
    {
        Evaluators = evaluators.ToList();
    }

    /// <inheritdoc/>
    public virtual IEnumerable<TResult> Evaluate<T, TResult>(IEnumerable<T> source, ISpecification<T, TResult> specification)
    {
        if (specification.Selector is null && specification.SelectorMany is null) throw new SelectorNotFoundException();
        if (specification.Selector != null && specification.SelectorMany != null) throw new ConcurrentSelectorsException();

        var baseQuery = Evaluate(source, (ISpecification<T>)specification);

        var resultQuery = specification.Selector != null
          ? baseQuery.Select(specification.Selector.Compile())
          : baseQuery.SelectMany(specification.SelectorMany!.Compile());

        return specification.PostProcessingAction is null
            ? resultQuery
            : specification.PostProcessingAction(resultQuery);
    }

    /// <inheritdoc/>
    public virtual IEnumerable<T> Evaluate<T>(IEnumerable<T> source, ISpecification<T> specification)
    {
        foreach (var evaluator in Evaluators)
        {
            source = evaluator.Evaluate(source, specification);
        }

        return specification.PostProcessingAction is null
            ? source
            : specification.PostProcessingAction(source);
    }
}
