namespace Ardalis.Specification.Benchmarks;

[MemoryDiagnoser]
public class Benchmark7_SearchMemoryEvaluator
{
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

