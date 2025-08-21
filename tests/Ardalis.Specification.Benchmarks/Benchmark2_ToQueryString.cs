using Ardalis.Specification.EntityFrameworkCore;

namespace Ardalis.Specification.Benchmarks;

[MemoryDiagnoser]
public class Benchmark2_ToQueryString
{
    /* This benchmark measures building the final SQL query.
     * Types:
     * 0 -> Empty
     * 1 -> Single Where clause
     * 2 -> Where and OrderBy
     * 3 -> Where, OrderBy, Include
     * 4 -> Where, Order chain, Include chain, Flag (AsNoTracking)
     * 5 -> Where, Order chain, Include chain, Like, Skip, Take, Flag (AsNoTracking)
     
         Results from version 9.2.0 on .NET 9.0 (2025-06-12)

        | Method | Type | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
        |------- |----- |----------:|---------:|---------:|------:|--------:|--------:|-------:|----------:|------------:|
        | EFCore | 0    |  77.51 us | 0.702 us | 0.548 us |  1.00 |    0.01 |  9.7656 | 0.4883 |  83.39 KB |        1.00 |
        | Spec   | 0    |  80.31 us | 1.110 us | 0.927 us |  1.04 |    0.01 |  9.7656 | 0.9766 |  83.61 KB |        1.00 |
        |        |      |           |          |          |       |         |         |        |           |             |
        | EFCore | 1    |  88.69 us | 1.237 us | 1.033 us |  1.00 |    0.02 | 10.2539 | 0.9766 |  86.29 KB |        1.00 |
        | Spec   | 1    |  89.64 us | 1.453 us | 1.213 us |  1.01 |    0.02 | 10.2539 | 0.9766 |  86.25 KB |        1.00 |
        |        |      |           |          |          |       |         |         |        |           |             |
        | EFCore | 2    |  90.29 us | 1.285 us | 1.202 us |  1.00 |    0.02 | 10.7422 | 0.9766 |   87.8 KB |        1.00 |
        | Spec   | 2    |  95.54 us | 1.767 us | 1.653 us |  1.06 |    0.02 | 10.7422 | 0.9766 |   88.3 KB |        1.01 |
        |        |      |           |          |          |       |         |         |        |           |             |
        | EFCore | 3    |  94.29 us | 1.057 us | 0.883 us |  1.00 |    0.01 | 10.7422 | 0.4883 |  89.64 KB |        1.00 |
        | Spec   | 3    |  98.24 us | 0.540 us | 0.451 us |  1.04 |    0.01 | 10.7422 | 0.4883 |  90.26 KB |        1.01 |
        |        |      |           |          |          |       |         |         |        |           |             |
        | EFCore | 4    | 104.56 us | 1.709 us | 1.427 us |  1.00 |    0.02 | 11.2305 | 0.4883 |  94.87 KB |        1.00 |
        | Spec   | 4    | 105.56 us | 1.988 us | 1.660 us |  1.01 |    0.02 | 11.2305 | 0.9766 |  95.31 KB |        1.00 |

         Results from version 9.3.0 on .NET 9.0 (2025-08-21)

        | Method | Type | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
        |------- |----- |----------:|---------:|---------:|------:|--------:|--------:|-------:|----------:|------------:|
        | EFCore | 0    |  78.67 us | 0.531 us | 0.414 us |  1.00 |    0.01 |  9.7656 | 0.4883 |  83.39 KB |        1.00 |
        | Spec   | 0    |  78.71 us | 1.058 us | 0.826 us |  1.00 |    0.01 |  9.7656 | 0.9766 |  83.61 KB |        1.00 |
        |        |      |           |          |          |       |         |         |        |           |             |
        | EFCore | 1    |  90.22 us | 0.704 us | 0.588 us |  1.00 |    0.01 | 10.2539 | 0.4883 |  85.94 KB |        1.00 |
        | Spec   | 1    |  90.29 us | 1.790 us | 1.495 us |  1.00 |    0.02 | 10.2539 | 0.4883 |  86.11 KB |        1.00 |
        |        |      |           |          |          |       |         |         |        |           |             |
        | EFCore | 2    |  90.38 us | 0.457 us | 0.382 us |  1.00 |    0.01 | 10.7422 | 0.9766 |   87.8 KB |        1.00 |
        | Spec   | 2    |  93.25 us | 1.354 us | 1.131 us |  1.03 |    0.01 | 10.7422 | 0.9766 |  88.06 KB |        1.00 |
        |        |      |           |          |          |       |         |         |        |           |             |
        | EFCore | 3    |  95.52 us | 0.483 us | 0.403 us |  1.00 |    0.01 | 10.7422 | 0.4883 |  89.64 KB |        1.00 |
        | Spec   | 3    |  96.88 us | 1.888 us | 1.576 us |  1.01 |    0.02 | 10.7422 | 0.4883 |  90.07 KB |        1.00 |
        |        |      |           |          |          |       |         |         |        |           |             |
        | EFCore | 4    | 101.05 us | 0.585 us | 0.489 us |  1.00 |    0.01 | 11.2305 | 0.4883 |  94.96 KB |        1.00 |
        | Spec   | 4    | 105.32 us | 0.580 us | 0.484 us |  1.04 |    0.01 | 11.2305 | 0.9766 |  95.22 KB |        1.00 |
        |        |      |           |          |          |       |         |         |        |           |             |
        | EFCore | 5    | 140.21 us | 1.504 us | 1.846 us |  1.00 |    0.02 | 13.6719 | 0.9766 | 115.11 KB |        1.00 |
        | Spec   | 5    | 140.22 us | 1.589 us | 1.240 us |  1.00 |    0.02 | 13.6719 | 0.9766 | 115.57 KB |        1.00 |
     */

    [Params(0, 1, 2, 3, 4, 5)]
    public int Type { get; set; }

    [Benchmark(Baseline = true)]
    public string EFCore()
    {
        using var context = new BenchmarkDbContext();

        if (Type == 0)
        {
            return context.Stores
                .ToQueryString();
        }
        else if (Type == 1)
        {
            return context.Stores
                .Where(x => x.Id > 0)
                .ToQueryString();
        }
        else if (Type == 2)
        {
            return context.Stores
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id)
                .ToQueryString();
        }
        else if (Type == 3)
        {
            return context.Stores
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id)
                .Include(x => x.Company)
                .ToQueryString();
        }
        else if (Type == 4)
        {
            return context.Stores
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id)
                    .ThenBy(x => x.Name)
                .Include(x => x.Company)
                    .ThenInclude(x => x.Country)
                .AsNoTracking()
                .ToQueryString();
        }
        else
        {
            var name = "%tore%";
            return context.Stores
                .Where(x => x.Id > 0)
                .Where(x => EF.Functions.Like(x.Name, name))
                .OrderBy(x => x.Id)
                    .ThenBy(x => x.Name)
                .Include(x => x.Company)
                    .ThenInclude(x => x.Country)
                .Skip(1)
                .Take(1)
                .AsNoTracking()
                .ToQueryString();
        }
    }

    [Benchmark]
    public string Spec()
    {
        using var context = new BenchmarkDbContext();

        if (Type == 0)
        {
            var spec = new Specification<Store>();

            return context.Stores
                .WithSpecification(spec)
                .ToQueryString();
        }
        else if (Type == 1)
        {
            var spec = new Specification<Store>();
            spec.Query
                .Where(x => x.Id > 0);

            return context.Stores
                .WithSpecification(spec)
                .ToQueryString();
        }
        else if (Type == 2)
        {
            var spec = new Specification<Store>();
            spec.Query
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id);

            return context.Stores
                .WithSpecification(spec)
                .ToQueryString();
        }
        else if (Type == 3)
        {
            var spec = new Specification<Store>();
            spec.Query
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id)
                .Include(x => x.Company);

            return context.Stores
                .WithSpecification(spec)
                .ToQueryString();
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

            return context.Stores
                .WithSpecification(spec)
                .ToQueryString();
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

            return context.Stores
                .WithSpecification(spec)
                .ToQueryString();
        }
    }
}
