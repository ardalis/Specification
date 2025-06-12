using Ardalis.Specification.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ardalis.Specification.Benchmarks;

[MemoryDiagnoser]
public class Benchmark6_IncludeEvaluator
{
    /*
     * This benchmark only measures applying Include to IQueryable.
     * It tends to measure the pure overhead of the reflection calls.
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
