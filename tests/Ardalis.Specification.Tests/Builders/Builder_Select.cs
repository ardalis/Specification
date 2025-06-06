namespace Tests.Builders;

public class Builder_Select
{
    public record Customer(int Id, string FirstName, string LastName);

    [Fact]
    public void DoesNothing_GivenNoSelect()
    {
        var spec = new Specification<Customer, string>();

        spec.Selector.Should().BeNull();
        spec.SelectorFunc.Should().BeNull();
    }

    [Fact]
    public void AddsSelector_GivenSelect()
    {
        Expression<Func<Customer, string>> expr = x => x.FirstName;

        var spec = new Specification<Customer, string>();
        spec.Query
            .Select(expr);

        spec.Selector.Should().NotBeNull();
        spec.SelectorFunc.Should().BeNull();
        spec.Selector.Should().BeSameAs(expr);
    }

    [Fact]
    public void OverwritesSelector_GivenMultipleSelect()
    {
        Expression<Func<Customer, string>> expr = x => x.FirstName;

        var spec = new Specification<Customer, string>();
        spec.Query
            .Select(x => x.LastName);
        spec.Query
            .Select(expr);

        spec.Selector.Should().NotBeNull();
        spec.SelectorFunc.Should().BeNull();
        spec.Selector.Should().BeSameAs(expr);
    }

    [Fact]
    public void AddsSelectorFunc_GivenSelect()
    {
        var spec = new Specification<Customer, string>();
        spec.Query
            .Select(SelectorFunc);

        spec.Selector.Should().BeNull();
        spec.SelectorMany.Should().BeNull();
        spec.SelectorFunc.Should().NotBeNull();
        spec.SelectorFunc.Should().BeOfType<Func<IQueryable<Customer>, IQueryable<string>>>();
        return;

        IQueryable<string> SelectorFunc(IQueryable<Customer> arg)
        {
            return global::System.Linq.Queryable.Select(arg, p => p.FirstName);
        }
    }
}
