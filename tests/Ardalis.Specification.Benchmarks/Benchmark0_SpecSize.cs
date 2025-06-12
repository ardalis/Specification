using System.Linq.Expressions;

namespace Ardalis.Specification.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class Benchmark0_SpecSize
{
    /* This benchmark is used to measure the Specification sizes and detect eventual regressions.
     * We measure with provided expressions, so it measures pure spec overhead.
     * Types:
     * 0 -> Empty
     * 1 -> Single Where clause
     * 2 -> Where and OrderBy
     * 3 -> Where, OrderBy, Include
     * 4 -> Where, Order chain, Include chain, Flag (AsNoTracking)
     * 5 -> Where, Order chain, Include chain, Like, Skip, Take, Flag (AsNoTracking)
     
         Results from version 9.2.0 on .NET 9.0 (2025-06-12)

        | Method | Type | Mean      | Error     | StdDev    | Gen0   | Gen1   | Allocated |
        |------- |----- |----------:|----------:|----------:|-------:|-------:|----------:|
        | Spec   | 0    |  5.113 ns |  4.230 ns | 0.2318 ns | 0.0124 |      - |     104 B |
        | Spec   | 1    | 13.929 ns |  7.006 ns | 0.3840 ns | 0.0249 |      - |     208 B |
        | Spec   | 2    | 27.080 ns | 14.668 ns | 0.8040 ns | 0.0382 | 0.0000 |     320 B |
        | Spec   | 3    | 34.670 ns | 18.903 ns | 1.0361 ns | 0.0516 | 0.0001 |     432 B |
        | Spec   | 4    | 45.458 ns | 19.777 ns | 1.0841 ns | 0.0612 | 0.0001 |     512 B |
        | Spec   | 5    | 60.715 ns | 18.731 ns | 1.0267 ns | 0.0783 | 0.0001 |     656 B |
     */

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
