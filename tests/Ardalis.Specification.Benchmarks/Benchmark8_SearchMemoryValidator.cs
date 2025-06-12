namespace Ardalis.Specification.Benchmarks;

[MemoryDiagnoser]
public class Benchmark8_SearchMemoryValidator
{
    /*
     * This benchmark measures search memory validator compared to version 8.
     * In version 9 we're utilizing allocation free GroupBy.

        Results from version 9.2.0 on .NET 9.0 (2025-06-12). They're diabolical :)

        | Method     | Mean      | Error    | StdDev   | Ratio | Gen0    | Allocated | Alloc Ratio |
        |----------- |----------:|---------:|---------:|------:|--------:|----------:|------------:|
        | ValidateV8 | 222.87 us | 1.797 us | 1.500 us |  1.00 | 73.9746 |  619016 B |        1.00 |
        | Validate   |  92.96 us | 0.261 us | 0.218 us |  0.42 |       - |         - |        0.00 |
     */

    private List<Customer> _source = default!;
    private CustomerSpec _specification = default!;
    private SearchValidatorV8 _validatorV8 = default!;
    private SearchValidator _validator = default!;

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
        _validator = SearchValidator.Instance;
        _validatorV8 = new SearchValidatorV8();
    }

    [Benchmark(Baseline = true)]
    public bool ValidateV8()
    {
        var validator = _validatorV8;

        var result = false;
        foreach (var item in _source)
        {
            result = validator.IsValid(item, _specification);
        }
        return result;
    }

    [Benchmark]
    public bool Validate()
    {
        var validator = _validator;

        var result = false;
        foreach (var item in _source)
        {
            result = validator.IsValid(item, _specification);
        }
        return result;
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

    private sealed class SearchValidatorV8 : IValidator
    {
        public bool IsValid<T>(T entity, ISpecification<T> specification)
        {
            foreach (var searchGroup in specification.SearchCriterias.GroupBy(x => x.SearchGroup))
            {
                if (searchGroup.Any(c => c.SelectorFunc(entity)?.Like(c.SearchTerm) ?? false) == false) return false;
            }

            return true;
        }
    }
}

