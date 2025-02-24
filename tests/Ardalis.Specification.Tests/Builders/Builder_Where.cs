namespace Tests.Builders;

public class Builder_Where
{
    public record Customer(int Id, string Name);

    [Fact]
    public void DoesNothing_GivenNoWhere()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        spec1.WhereExpressions.Should().BeEmpty();
        spec2.WhereExpressions.Should().BeEmpty();
    }

    [Fact]
    public void DoesNothing_GivenWhereWithFalseCondition()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .Where(x => x.Id > 1, false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Where(x => x.Id > 1, false);

        spec1.WhereExpressions.Should().BeEmpty();
        spec2.WhereExpressions.Should().BeEmpty();
    }

    [Fact]
    public void AddsWhere_GivenWhere()
    {
        Expression<Func<Customer, bool>> expr = x => x.Id > 1;
        var spec1 = new Specification<Customer>();
        spec1.Query
            .Where(expr);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Where(expr);

        spec1.WhereExpressions.Should().ContainSingle();
        spec1.WhereExpressions.First().Filter.Should().BeSameAs(expr);
        spec2.WhereExpressions.Should().ContainSingle();
        spec2.WhereExpressions.First().Filter.Should().BeSameAs(expr);
    }

    [Fact]
    public void AddsWhere_GivenMultipleWhere()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .Where(x => x.Id > 1)
            .Where(x => x.Id > 2);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Where(x => x.Id > 1)
            .Where(x => x.Id > 2);

        spec1.WhereExpressions.Should().HaveCount(2);
        spec2.WhereExpressions.Should().HaveCount(2);
    }
}
