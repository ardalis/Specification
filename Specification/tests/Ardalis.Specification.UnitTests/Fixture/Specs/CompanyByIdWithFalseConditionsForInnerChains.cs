namespace Ardalis.Specification.UnitTests.Fixture.Specs;

public class CompanyByIdWithFalseConditionsForInnerChains : Specification<Company>, ISingleResultSpecification
{
    public CompanyByIdWithFalseConditionsForInnerChains(int id)
    {
        Query.Where(x => x.Id == id, false)
            .OrderBy(x => x.Id)
                .ThenBy(x => x.Name, false)
                .ThenByDescending(x => x.Name)
            .OrderByDescending(x => x.Id)
                .ThenByDescending(x => x.Name, false)
                .ThenBy(x => x.Name)
            .Include(x => x.Stores)
                .ThenInclude(x => x.Products, false)
                .ThenInclude(x => x.Store)
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
