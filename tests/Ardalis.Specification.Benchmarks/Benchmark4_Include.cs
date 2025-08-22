using Ardalis.Specification.EntityFrameworkCore;

namespace Ardalis.Specification.Benchmarks;

[MemoryDiagnoser]
public class Benchmark4_Include
{
    private DbSet<Store> _queryable = default!;

    [GlobalSetup]
    public void Setup()
    {
        _queryable = new BenchmarkDbContext().Stores;
    }

    [Benchmark(Baseline = true)]
    public object EFCore()
    {
        var result = _queryable
            .Include(x => x.Company)
            .ThenInclude(x => x.Country);

        return result;
    }

    [Benchmark]
    public object Spec()
    {
        var spec = new Specification<Store>();
        spec.Query
            .Include(x => x.Company)
            .ThenInclude(x => x.Country);

        return _queryable.WithSpecification(spec);
    }
}
