namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class CompanyByIdWithFalseConditions : Specification<Company>, ISingleResultSpecification
{
    public CompanyByIdWithFalseConditions(int id)
    {
        Query.Where(x => x.Id == id, false)
            .OrderBy(x => x.Id, false)
                .ThenBy(x => x.Name)
                .ThenByDescending(x => x.Name)
            .OrderByDescending(x => x.Id, false)
                .ThenBy(x => x.Name)
                .ThenByDescending(x => x.Name)
            .Include(x => x.Stores, false)
                .ThenInclude(x => x.Products)
            .Include(nameof(Store), false)
            .Take(10, false)
            .Skip(10, false)
            .AsNoTracking(false)
            .AsNoTrackingWithIdentityResolution(false)
            .AsSplitQuery(false)
            .IgnoreQueryFilters(false)
            .Search(x => x.Name!, "asd", false)
            .EnableCache(nameof(CompanyByIdWithFalseConditions), false, id);
    }
}
