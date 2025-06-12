using Ardalis.Specification.EntityFrameworkCore;

namespace Ardalis.Specification.Benchmarks;

[MemoryDiagnoser]
public class Benchmark3_DbQuery
{
    /* This benchmark measures the end-to-end cycle, including the round trip to the database.
     * Types:
     * 0 -> Empty
     * 1 -> Single Where clause
     * 2 -> Where and OrderBy
     * 3 -> Where, OrderBy, Include
     * 4 -> Where, Order chain, Include chain, Flag (AsNoTracking)
     * 5 -> Where, Order chain, Include chain, Like, Skip, Take, Flag (AsNoTracking)
     
         Results from version 9.2.0 on .NET 9.0 (2025-06-12)

        | Method | Type | Mean     | Error    | StdDev   | Median   | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
        |------- |----- |---------:|---------:|---------:|---------:|------:|--------:|--------:|-------:|----------:|------------:|
        | EFCore | 0    | 308.2 us |  9.33 us | 27.06 us | 316.9 us |  1.01 |    0.15 | 10.7422 | 0.9766 |  91.46 KB |        1.00 |
        | Spec   | 0    | 287.9 us | 11.49 us | 33.52 us | 304.4 us |  0.94 |    0.15 | 10.7422 | 0.9766 |   91.6 KB |        1.00 |
        |        |      |          |          |          |          |       |         |         |        |           |             |
        | EFCore | 1    | 299.6 us | 11.83 us | 34.32 us | 316.2 us |  1.02 |    0.19 | 10.7422 | 0.9766 |  94.34 KB |        1.00 |
        | Spec   | 1    | 300.9 us | 12.10 us | 35.11 us | 317.2 us |  1.02 |    0.19 | 10.7422 | 0.9766 |  94.57 KB |        1.00 |
        |        |      |          |          |          |          |       |         |         |        |           |             |
        | EFCore | 2    | 310.4 us | 11.13 us | 31.92 us | 323.2 us |  1.01 |    0.17 | 11.7188 | 0.9766 |  95.82 KB |        1.00 |
        | Spec   | 2    | 306.4 us | 12.47 us | 34.75 us | 321.6 us |  1.00 |    0.17 | 11.7188 | 0.9766 |  96.44 KB |        1.01 |
        |        |      |          |          |          |          |       |         |         |        |           |             |
        | EFCore | 3    | 329.0 us | 12.37 us | 34.68 us | 342.2 us |  1.01 |    0.18 | 11.7188 | 0.9766 | 100.95 KB |        1.00 |
        | Spec   | 3    | 331.5 us | 11.31 us | 32.62 us | 344.3 us |  1.02 |    0.17 | 11.7188 | 0.9766 | 101.86 KB |        1.01 |
        |        |      |          |          |          |          |       |         |         |        |           |             |
        | EFCore | 4    | 336.9 us | 10.72 us | 30.75 us | 346.7 us |  1.01 |    0.15 | 12.6953 | 0.9766 | 104.08 KB |        1.00 |
        | Spec   | 4    | 329.9 us | 12.56 us | 35.62 us | 344.7 us |  0.99 |    0.16 | 12.6953 | 0.9766 | 104.87 KB |        1.01 |
        |        |      |          |          |          |          |       |         |         |        |           |             |
        | EFCore | 5    | 384.7 us | 11.05 us | 29.88 us | 394.4 us |  1.01 |    0.13 | 14.6484 | 0.9766 | 122.07 KB |        1.00 |
        | Spec   | 5    | 389.4 us | 11.58 us | 31.90 us | 400.3 us |  1.02 |    0.13 | 14.6484 | 0.9766 | 122.89 KB |        1.01 |
     */

    [GlobalSetup]
    public async Task Setup()
    {
        await BenchmarkDbContext.SeedAsync();
    }

    [Params(0, 1, 2, 3, 4, 5)]
    public int Type { get; set; }

    [Benchmark(Baseline = true)]
    public async Task<Store> EFCore()
    {
        using var context = new BenchmarkDbContext();

        if (Type == 0)
        {
            return await context.Stores
                .FirstAsync();
        }
        else if (Type == 1)
        {
            return await context.Stores
                .Where(x => x.Id > 0)
                .FirstAsync();
        }
        else if (Type == 2)
        {
            return await context.Stores
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id)
                .FirstAsync();
        }
        else if (Type == 3)
        {
            return await context.Stores
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id)
                .Include(x => x.Company)
                .FirstAsync();
        }
        else if (Type == 4)
        {
            return await context.Stores
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id)
                    .ThenBy(x => x.Name)
                .Include(x => x.Company)
                    .ThenInclude(x => x.Country)
                .AsNoTracking()
                .FirstAsync();
        }
        else
        {
            var name = "%tore%";
            return await context.Stores
                .Where(x => x.Id > 0)
                .Where(x => EF.Functions.Like(x.Name, name))
                .OrderBy(x => x.Id)
                    .ThenBy(x => x.Name)
                .Include(x => x.Company)
                    .ThenInclude(x => x.Country)
                .Skip(1)
                .Take(1)
                .AsNoTracking()
                .FirstAsync();
        }
    }

    [Benchmark]
    public async Task<Store> Spec()
    {
        using var context = new BenchmarkDbContext();

        if (Type == 0)
        {
            var spec = new Specification<Store>();

            return await context.Stores
                .WithSpecification(spec)
                .FirstAsync();
        }
        else if (Type == 1)
        {
            var spec = new Specification<Store>();
            spec.Query
                .Where(x => x.Id > 0);

            return await context.Stores
                .WithSpecification(spec)
                .FirstAsync();
        }
        else if (Type == 2)
        {
            var spec = new Specification<Store>();
            spec.Query
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id);

            return await context.Stores
                .WithSpecification(spec)
                .FirstAsync();
        }
        else if (Type == 3)
        {
            var spec = new Specification<Store>();
            spec.Query
                .Where(x => x.Id > 0)
                .OrderBy(x => x.Id)
                .Include(x => x.Company);

            return await context.Stores
                .WithSpecification(spec)
                .FirstAsync();
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

            return await context.Stores
                .WithSpecification(spec)
                .FirstAsync();
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

            return await context.Stores
                .WithSpecification(spec)
                .FirstAsync();
        }
    }
}
