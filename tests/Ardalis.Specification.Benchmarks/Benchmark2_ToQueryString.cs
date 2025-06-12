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
