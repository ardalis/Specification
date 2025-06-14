namespace Tests;

public class SpecificationTests
{
    public record Customer(int Id, string Name, Address Address);
    public record Address(int Id, City City);
    public record City(int Id, string Name);

    [Fact]
    public void CollectionsProperties_ReturnEmptyEnumerable_GivenEmptySpec()
    {
        var spec = new Specification<Customer>();

        spec.WhereExpressions.Should().BeSameAs(Enumerable.Empty<WhereExpressionInfo<Customer>>());
        spec.SearchCriterias.Should().BeSameAs(Enumerable.Empty<SearchExpressionInfo<Customer>>());
        spec.OrderExpressions.Should().BeSameAs(Enumerable.Empty<OrderExpressionInfo<Customer>>());
        spec.IncludeExpressions.Should().BeSameAs(Enumerable.Empty<IncludeExpressionInfo>());
        spec.IncludeStrings.Should().BeSameAs(Enumerable.Empty<string>());
    }

    [Fact]
    public void Clone_ReturnsCopy()
    {
        var spec = new Specification<Customer>();
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

        var newSpec = spec.Clone();

        newSpec.Should().BeOfType<Specification<Customer>>();

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
    }

    [Fact]
    public void Clone_ReturnsCopyWithProjectionType()
    {
        var spec = new Specification<Customer>();
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

        var newSpec = spec.Clone<string>();

        newSpec.Should().BeOfType<Specification<Customer, string>>();

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
    }

#if NET8_0_OR_GREATER
    [Fact]
    public void Items_InitializesOnFirstAccess()
    {
        var spec = new Specification<Customer>();

        Accessors<Customer>.ItemsFieldOf(spec).Should().BeNull();
        _ = spec.Items.Count;
        Accessors<Customer>.ItemsFieldOf(spec).Should().NotBeNull();
    }

    private class Accessors<T>
    {
        [System.Runtime.CompilerServices.UnsafeAccessor(System.Runtime.CompilerServices.UnsafeAccessorKind.Field, Name = "_items")]
        public static extern ref Dictionary<string, object>? ItemsFieldOf(Specification<Customer> @this);
    }
#endif
}
