using Ardalis.Specification.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ardalis.Specification.Benchmarks;

[MemoryDiagnoser]
public class Benchmark5_IncludeEvaluator
{
    /*
     * This benchmark only measures applying Include to IQueryable.
     * It tends to measure the pure overhead of the reflection calls.

         Results from version 9.3.0 on .NET 9.0 (2025-08-21)

        | Method | Mean     | Error     | StdDev    | Ratio | Gen0   | Allocated | Alloc Ratio |
        |------- |---------:|----------:|----------:|------:|-------:|----------:|------------:|
        | EFCore | 1.119 us | 0.0086 us | 0.0080 us |  1.00 | 0.1106 |     928 B |        1.00 |
        | Spec   | 1.226 us | 0.0089 us | 0.0083 us |  1.10 | 0.1144 |     968 B |        1.04 |
     */

    private static readonly Expression<Func<Store, Company>> _includeCompany = x => x.Company;
    private static readonly Expression<Func<Company, Country>> _includeCountry = x => x.Country;

    private DbSet<Store> _queryable = default!;
    private Specification<Store> _spec = default!;

    [GlobalSetup]
    public void Setup()
    {
        _queryable = new BenchmarkDbContext().Stores;
        _spec = new Specification<Store>();
        _spec.Query
            .Include(_includeCompany)
            .ThenInclude(_includeCountry);
    }

    [Benchmark(Baseline = true)]
    public object EFCore()
    {
        var result = _queryable
            .Include(_includeCompany)
            .ThenInclude(_includeCountry);

        return result;
    }

    [Benchmark]
    public object Spec()
    {
        var evaluator = IncludeEvaluator.Instance;
        var result = evaluator.GetQuery(_queryable, _spec);
        return result;
    }
}
