using Ardalis.Specification.EntityFrameworkCore;

namespace Ardalis.Specification.Benchmarks;

[MemoryDiagnoser]
public class Benchmark1_IQueryable
{
    /* This benchmark measures building the IQueryable state. It's the overhead of using specifications (there might be some jitter due to EFs internal caching).
     * Types:
     * 0 -> Empty
     * 1 -> Single Where clause
     * 2 -> Where and OrderBy
     * 3 -> Where, OrderBy, Include
     * 4 -> Where, Order chain, Include chain, Flag (AsNoTracking)
     * 5 -> Where, Order chain, Include chain, Like, Skip, Take, Flag (AsNoTracking)
          
         Results from version 9.2.0 on .NET 9.0 (2025-06-12)

        | Method | Type | Mean          | Error       | StdDev      | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
        |------- |----- |--------------:|------------:|------------:|-------:|--------:|-------:|----------:|------------:|
        | EFCore | 0    |     0.8469 ns |   0.0067 ns |   0.0056 ns |   1.00 |    0.01 |      - |         - |          NA |
        | Spec   | 0    |   155.7269 ns |   2.3665 ns |   2.2137 ns | 183.89 |    2.79 | 0.0172 |     144 B |          NA |
        |        |      |               |             |             |        |         |        |           |             |
        | EFCore | 1    |   682.6474 ns |   5.1237 ns |   4.5420 ns |   1.00 |    0.01 | 0.1183 |    1016 B |        1.00 |
        | Spec   | 1    |   870.7886 ns |   4.3108 ns |   3.5997 ns |   1.28 |    0.01 | 0.1554 |    1304 B |        1.28 |
        |        |      |               |             |             |        |         |        |           |             |
        | EFCore | 2    | 1,358.6505 ns |   9.9430 ns |   8.3029 ns |   1.00 |    0.01 | 0.2289 |    1952 B |        1.00 |
        | Spec   | 2    | 1,694.2367 ns |  22.3394 ns |  18.6544 ns |   1.25 |    0.02 | 0.2899 |    2440 B |        1.25 |
        |        |      |               |             |             |        |         |        |           |             |
        | EFCore | 3    | 2,197.6741 ns |  15.5856 ns |  13.0147 ns |   1.00 |    0.01 | 0.3433 |    2912 B |        1.00 |
        | Spec   | 3    | 2,801.0768 ns |  11.1079 ns |   9.8469 ns |   1.27 |    0.01 | 0.4120 |    3552 B |        1.22 |
        |        |      |               |             |             |        |         |        |           |             |
        | EFCore | 4    | 4,719.4941 ns |  39.1815 ns |  36.6504 ns |   1.00 |    0.01 | 0.6409 |    5472 B |        1.00 |
        | Spec   | 4    | 5,238.6977 ns |  38.4033 ns |  32.0685 ns |   1.11 |    0.01 | 0.7019 |    6000 B |        1.10 |
        |        |      |               |             |             |        |         |        |           |             |
        | EFCore | 5    | 6,601.1200 ns |  44.9208 ns |  39.8211 ns |   1.00 |    0.01 | 0.9155 |    7728 B |        1.00 |
        | Spec   | 5    | 7,165.3328 ns | 137.3564 ns | 128.4833 ns |   1.09 |    0.02 | 0.9766 |    8272 B |        1.07 |

         Results from version 9.3.0 on .NET 9.0 (2025-08-21)

        | Method | Type | Mean          | Error      | StdDev     | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
        |------- |----- |--------------:|-----------:|-----------:|-------:|--------:|-------:|----------:|------------:|
        | EFCore | 0    |     0.5752 ns |  0.0102 ns |  0.0085 ns |   1.00 |    0.02 |      - |         - |          NA |
        | Spec   | 0    |   118.4065 ns |  0.5723 ns |  0.5073 ns | 205.89 |    3.01 | 0.0172 |     144 B |          NA |
        |        |      |               |            |            |        |         |        |           |             |
        | EFCore | 1    |   658.2708 ns | 12.4211 ns | 11.6187 ns |   1.00 |    0.02 | 0.1183 |    1016 B |        1.00 |
        | Spec   | 1    |   929.8103 ns | 17.5015 ns | 16.3709 ns |   1.41 |    0.03 | 0.1421 |    1192 B |        1.17 |
        |        |      |               |            |            |        |         |        |           |             |
        | EFCore | 2    | 1,332.8425 ns |  9.0197 ns |  7.9958 ns |   1.00 |    0.01 | 0.2327 |    1952 B |        1.00 |
        | Spec   | 2    | 1,596.2919 ns |  5.8616 ns |  4.5764 ns |   1.20 |    0.01 | 0.2594 |    2216 B |        1.14 |
        |        |      |               |            |            |        |         |        |           |             |
        | EFCore | 3    | 2,193.2508 ns | 12.5628 ns | 11.1366 ns |   1.00 |    0.01 | 0.3433 |    2912 B |        1.00 |
        | Spec   | 3    | 2,728.9742 ns | 25.1605 ns | 21.0102 ns |   1.24 |    0.01 | 0.3815 |    3216 B |        1.10 |
        |        |      |               |            |            |        |         |        |           |             |
        | EFCore | 4    | 4,380.9674 ns | 66.0562 ns | 58.5571 ns |   1.00 |    0.02 | 0.6104 |    5168 B |        1.00 |
        | Spec   | 4    | 5,327.0482 ns | 35.3173 ns | 29.4915 ns |   1.22 |    0.02 | 0.7019 |    5896 B |        1.14 |
        |        |      |               |            |            |        |         |        |           |             |
        | EFCore | 5    | 6,597.2443 ns | 53.6835 ns | 47.5891 ns |   1.00 |    0.01 | 0.8850 |    7440 B |        1.00 |
        | Spec   | 5    | 7,307.0483 ns | 55.6593 ns | 49.3406 ns |   1.11 |    0.01 | 0.9766 |    8328 B |        1.12 |
     */

    private DbSet<Store> _queryable = default!;

    [GlobalSetup]
    public void Setup()
    {
        _queryable = new BenchmarkDbContext().Stores;
    }

    [Params(0, 1, 2, 3, 4, 5)]
    public int Type { get; set; }

    [Benchmark(Baseline = true)]
    public object EFCore()
    {
        if (Type == 0)
        {
            return _queryable;
        }
        else if (Type == 1)
        {
            return _queryable
                .Where(x => x.Id > 0);
        }
        else if (Type == 2)
        {
            return _queryable
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id);
        }
        else if (Type == 3)
        {
            return _queryable
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id)
                .Include(x => x.Company);
        }
        else if (Type == 4)
        {
            return _queryable
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id)
                    .ThenBy(x => x.Name)
                .Include(x => x.Company)
                    .ThenInclude(x => x.Country)
                .AsNoTracking();
        }
        else
        {
            var name = "%tore%";
            return _queryable
                .Where(x => x.Id > 0)
                .Where(x => EF.Functions.Like(x.Name, name))
                .OrderBy(x => x.Id)
                    .ThenBy(x => x.Name)
                .Include(x => x.Company)
                    .ThenInclude(x => x.Country)
                .Skip(1)
                .Take(1)
                .AsNoTracking();
        }
    }

    [Benchmark]
    public object Spec()
    {
        if (Type == 0)
        {
            var spec = new Specification<Store>();
            return _queryable
                .WithSpecification(spec);
        }
        else if (Type == 1)
        {
            var spec = new Specification<Store>();
            spec.Query
                .Where(x => x.Id > 0);

            return _queryable
                .WithSpecification(spec);
        }
        else if (Type == 2)
        {
            var spec = new Specification<Store>();
            spec.Query
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id);

            return _queryable
                .WithSpecification(spec);
        }
        else if (Type == 3)
        {
            var spec = new Specification<Store>();
            spec.Query
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id)
                .Include(x => x.Company);

            return _queryable
                .WithSpecification(spec);
        }
        else if (Type == 4)
        {
            var spec = new Specification<Store>();
            spec.Query
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id)
                    .ThenBy(x => x.Name)
                .Include(x => x.Company)
                    .ThenInclude(x => x.Country)
                .AsNoTracking();

            return _queryable
                .WithSpecification(spec);
        }
        else
        {
            var name = "%tore%";
            var spec = new Specification<Store>();
            spec.Query
                .Where(x => x.Id > 0)
                .Search(x => x.Name, name)
                .OrderBy(x => x.Id)
                    .ThenBy(x => x.Name)
                .Include(x => x.Company)
                    .ThenInclude(x => x.Country)
                .Skip(1)
                .Take(1)
                .AsNoTracking();

            return _queryable
                .WithSpecification(spec);
        }
    }
}
