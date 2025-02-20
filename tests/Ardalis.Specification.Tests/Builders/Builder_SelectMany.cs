namespace Tests.Builders;

public class Builder_SelectMany
{
    public record Customer(int Id, List<string> FirstName, List<string> LastName);

    [Fact]
    public void DoesNothing_GivenNoSelectMany()
    {
        var spec = new Specification<Customer, string>();

        spec.SelectorMany.Should().BeNull();
    }

    [Fact]
    public void AddsSelectorMany_GivenSelectMany()
    {
        Expression<Func<Customer, IEnumerable<string>>> expr = x => x.FirstName;

        var spec = new Specification<Customer, string>();
        spec.Query
            .SelectMany(expr);

        spec.SelectorMany.Should().NotBeNull();
        spec.SelectorMany.Should().BeSameAs(expr);
    }

    [Fact]
    public void OverwritesSelectorMany_GivenMultipleSelectMany()
    {
        Expression<Func<Customer, IEnumerable<string>>> expr = x => x.FirstName;

        var spec = new Specification<Customer, string>();
        spec.Query
            .SelectMany(x => x.LastName);
        spec.Query
            .SelectMany(expr);

        spec.SelectorMany.Should().NotBeNull();
        spec.SelectorMany.Should().BeSameAs(expr);
    }
}
