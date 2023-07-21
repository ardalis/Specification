using Ardalis.Specification.EntityFrameworkCore;

namespace Ardalis.Sample.App2;

public class AppSpecificationEvaluator : SpecificationEvaluator
{
    public static AppSpecificationEvaluator Instance { get; } = new AppSpecificationEvaluator();

    public AppSpecificationEvaluator() : base()
    {
        Evaluators.Add(QueryTagEvaluator.Instance);
    }
}
