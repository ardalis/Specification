using System.Linq.Expressions;

namespace Ardalis.Specification.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class Benchmark0_SpecSize
{
    public static class Expressions
    {
        public static Expression<Func<Store, bool>> Criteria { get; } = x => x.Id > 0;
        public static Expression<Func<Store, object?>> OrderBy { get; } = x => x.Id;
        public static Expression<Func<Store, object?>> OrderThenBy { get; } = x => x.Name;
        public static Expression<Func<Store, Company>> IncludeStoreCompany { get; } = x => x.Company;
        public static Expression<Func<Company, Country>> IncludeCompanyCountry { get; } = x => x.Country;
        public static Expression<Func<Store, string?>> Like { get; } = x => x.Name;
    }

    [Params(0, 1, 2, 3, 4, 5)]
    public int Type { get; set; }

    [Benchmark]
    public object Spec()
    {
        if (Type == 0)
        {
            var spec = new Specification<Store>();
            return spec;
        }
        else if (Type == 1)
        {
            var spec = new Specification<Store>();
            spec.Query
                .Where(Expressions.Criteria);

            return spec;
        }
        else if (Type == 2)
        {
            var spec = new Specification<Store>();
            spec.Query
                .Where(Expressions.Criteria)
                .OrderBy(Expressions.OrderBy);

            return spec;
        }
        else if (Type == 3)
        {
            var spec = new Specification<Store>();
            spec.Query
                .Where(Expressions.Criteria)
                .OrderBy(Expressions.OrderBy)
                .Include(Expressions.IncludeStoreCompany);

            return spec;
        }
        else if (Type == 4)
        {
            var spec = new Specification<Store>();
            spec.Query
                .Where(Expressions.Criteria)
                .OrderBy(Expressions.OrderBy)
                    .ThenBy(Expressions.OrderThenBy)
                .Include(Expressions.IncludeStoreCompany)
                    .ThenInclude(Expressions.IncludeCompanyCountry)
                .AsNoTracking();

            return spec;
        }
        else
        {
            var spec = new Specification<Store>();
            spec.Query
                .Where(Expressions.Criteria)
                .OrderBy(Expressions.OrderBy)
                    .ThenBy(Expressions.OrderThenBy)
                .Include(Expressions.IncludeStoreCompany)
                    .ThenInclude(Expressions.IncludeCompanyCountry)
                .Search(Expressions.Like, "%tore%")
                .Skip(1)
                .Take(1)
                .AsNoTracking();

            return spec;
        }
    }
}
