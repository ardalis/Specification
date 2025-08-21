namespace Ardalis.Specification.Benchmarks;

[MemoryDiagnoser]
public class Benchmark7_SearchMemoryEvaluator
{
    /*
     * This benchmark measures search memory evaluation compared to version 8.
     * In version 9 we're utilizing allocation free GroupBy.

        Results from version 9.2.0 on .NET 9.0 (2025-06-12). They're diabolical :)

        | Method     | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0    | Allocated | Alloc Ratio |
        |----------- |----------:|---------:|---------:|------:|--------:|--------:|----------:|------------:|
        | EvaluateV8 | 136.54 us | 2.571 us | 2.640 us |  1.00 |    0.03 | 28.8086 |  241832 B |       1.000 |
        | Evaluate   |  95.22 us | 1.554 us | 1.454 us |  0.70 |    0.02 |       - |      96 B |       0.000 |

         Results from version 9.2.0 on .NET 9.0 (2025-06-12)

        | Method     | Mean      | Error    | StdDev   | Ratio | Gen0    | Allocated | Alloc Ratio |
        |----------- |----------:|---------:|---------:|------:|--------:|----------:|------------:|
        | EvaluateV8 | 126.50 us | 0.741 us | 0.693 us |  1.00 | 28.8086 |  241832 B |       1.000 |
        | Evaluate   |  90.88 us | 0.258 us | 0.229 us |  0.72 |       - |      96 B |       0.000 |
     */

    private List<Customer> _source = default!;
    private CustomerSpec _specification = default!;
    private SearchMemoryEvaluatorV8 _evaluatorV8 = default!;
    private SearchMemoryEvaluator _evaluator = default!;

    [GlobalSetup]
    public void Setup()
    {
        _source =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "aaaa"),
            new(3, "axxa", "axza"),
            new(4, "aaaa", null),
            new(5, "axxa", null),
            .. Enumerable.Range(6, 1000).Select(x => new Customer(x, "axxa", "axya"))
        ];

        _specification = new CustomerSpec();
        _evaluatorV8 = new SearchMemoryEvaluatorV8();
        _evaluator = SearchMemoryEvaluator.Instance;
    }

    [Benchmark(Baseline = true)]
    public int EvaluateV8()
    {
        var evaluator = _evaluatorV8;
        var result = evaluator.Evaluate(_source, _specification);
        return result.Count();
    }

    [Benchmark]
    public int Evaluate()
    {
        var evaluator = _evaluator;
        var result = evaluator.Evaluate(_source, _specification);
        return result.Count();
    }

    private record Customer(int Id, string FirstName, string? LastName);
    private class CustomerSpec : Specification<Customer>
    {
        public CustomerSpec()
        {
            Query
                .Search(x => x.FirstName, "%xx%", 1)
                .Search(x => x.LastName, "%xy%", 2)
                .Search(x => x.LastName, "%xz%", 2);
        }
    }

    private sealed class SearchMemoryEvaluatorV8 : IInMemoryEvaluator
    {
        public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification)
        {
            foreach (var searchGroup in specification.SearchCriterias.GroupBy(x => x.SearchGroup))
            {
                query = query.Where(x => searchGroup.Any(c => c.SelectorFunc(x)?.Like(c.SearchTerm) ?? false));
            }

            return query;
        }
    }
}

