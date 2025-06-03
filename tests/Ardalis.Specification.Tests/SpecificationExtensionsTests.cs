namespace Tests;

public class SpecificationExtensionsTests
{
    private record Address(int Id, string Street);
    private record Person(int Id, string Name, List<string> Names, Address Address);

    [Fact]
    public void WithProjectionOf_ReturnsCopyWithProjection()
    {
        var spec = new Specification<Person>();
        spec.Items.Add("test", "test");
        spec.Query
            .Where(x => x.Name == "test")
            .Include(x => x.Address)
            .Include("Address")
            .OrderBy(x => x.Id)
            .Search(x => x.Name, "test")
            .Take(2)
            .Skip(3)
            .WithCacheKey("testKey")
            .IgnoreQueryFilters()
            .IgnoreQueryFilters()
            .AsSplitQuery()
            .AsNoTracking()
            .TagWith("testQuery1")
            .PostProcessingAction(x => x.Where(x => x.Id > 0));

        var projectionSpec = new Specification<Person, string>();
        projectionSpec.Query.Select(x => x.Name);
        projectionSpec.Query.SelectMany(x => x.Names);
        projectionSpec.Query.PostProcessingAction(x => x.Select(x => x + "A"));

        var newSpec = spec.WithProjectionOf(projectionSpec);

        newSpec.Items.Should().NotBeSameAs(spec.Items);
        newSpec.Items.Should().BeEquivalentTo(spec.Items);

        newSpec.WhereExpressions.Should().NotBeSameAs(spec.WhereExpressions);
        newSpec.WhereExpressions.Should().Equal(spec.WhereExpressions);

        newSpec.IncludeExpressions.Should().NotBeSameAs(spec.IncludeExpressions);
        newSpec.IncludeExpressions.Should().Equal(spec.IncludeExpressions);

        newSpec.IncludeStrings.Should().NotBeSameAs(spec.IncludeStrings);
        newSpec.IncludeStrings.Should().Equal(spec.IncludeStrings);

        newSpec.OrderExpressions.Should().NotBeSameAs(spec.OrderExpressions);
        newSpec.OrderExpressions.Should().Equal(spec.OrderExpressions);

        newSpec.SearchCriterias.Should().NotBeSameAs(spec.SearchCriterias);
        newSpec.SearchCriterias.Should().Equal(spec.SearchCriterias);

        newSpec.QueryTags.Should().NotBeSameAs(spec.QueryTags);
        newSpec.QueryTags.Should().Equal(spec.QueryTags);

        newSpec.Take.Should().Be(spec.Take);
        newSpec.Skip.Should().Be(spec.Skip);
        newSpec.CacheKey.Should().Be(spec.CacheKey);
        newSpec.IgnoreQueryFilters.Should().Be(spec.IgnoreQueryFilters);
        newSpec.IgnoreAutoIncludes.Should().Be(spec.IgnoreAutoIncludes);
        newSpec.AsSplitQuery.Should().Be(spec.AsSplitQuery);
        newSpec.AsNoTracking.Should().Be(spec.AsNoTracking);
        newSpec.AsNoTrackingWithIdentityResolution.Should().Be(spec.AsNoTrackingWithIdentityResolution);
        newSpec.AsTracking.Should().Be(spec.AsTracking);

        newSpec.PostProcessingAction.Should().BeSameAs(projectionSpec.PostProcessingAction);
        ((Specification<Person>)newSpec).PostProcessingAction.Should().BeSameAs(spec.PostProcessingAction);
    }

    [Fact]
    public void WithProjectionOf_ReturnsCopyWithProjection_GivenSpecWithMultipleTags()
    {
        var spec = new Specification<Person>();
        spec.Query
            .TagWith("testQuery1")
            .TagWith("testQuery2");

        var projectionSpec = new Specification<Person, string>();
        projectionSpec.Query.Select(x => x.Name);
        projectionSpec.Query.SelectMany(x => x.Names);

        var newSpec = spec.WithProjectionOf(projectionSpec);

        newSpec.QueryTags.Should().NotBeSameAs(spec.QueryTags);
        newSpec.QueryTags.Should().Equal(spec.QueryTags);
    }
}
