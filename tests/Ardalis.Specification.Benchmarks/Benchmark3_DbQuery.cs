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
