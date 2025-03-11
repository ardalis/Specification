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
