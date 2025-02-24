namespace Tests.Builders;

public class Builder_Include
{
    public record Customer(int Id, Address Address, Contact Contact);
    public record Address(int Id, string City);
    public record Contact(int Id, string Email);

    [Fact]
    public void DoesNothing_GivenNoInclude()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        spec1.IncludeExpressions.Should().BeEmpty();
        spec2.IncludeExpressions.Should().BeEmpty();
    }

    [Fact]
    public void DoesNothing_GivenIncludeWithFalseCondition()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .Include(x => x.Address, false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Include(x => x.Address, false);

        spec1.IncludeExpressions.Should().BeEmpty();
        spec2.IncludeExpressions.Should().BeEmpty();
    }

    [Fact]
    public void AddsInclude_GivenInclude()
    {
        Expression<Func<Customer, Address>> expr = x => x.Address;

        var spec1 = new Specification<Customer>();
        spec1.Query
            .Include(expr);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Include(expr);

        spec1.IncludeExpressions.Should().ContainSingle();
        spec1.IncludeExpressions.First().LambdaExpression.Should().BeSameAs(expr);
        spec1.IncludeExpressions.First().Type.Should().Be(IncludeTypeEnum.Include);
        spec2.IncludeExpressions.Should().ContainSingle();
        spec2.IncludeExpressions.First().LambdaExpression.Should().BeSameAs(expr);
        spec2.IncludeExpressions.First().Type.Should().Be(IncludeTypeEnum.Include);
    }

    [Fact]
    public void AddsInclude_GivenMultipleInclude()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .Include(x => x.Address)
            .Include(x => x.Contact);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Include(x => x.Address)
            .Include(x => x.Contact);

        spec1.IncludeExpressions.Should().HaveCount(2);
        spec1.IncludeExpressions.Should().AllSatisfy(x => x.Type.Should().Be(IncludeTypeEnum.Include));
        spec2.IncludeExpressions.Should().HaveCount(2);
        spec2.IncludeExpressions.Should().AllSatisfy(x => x.Type.Should().Be(IncludeTypeEnum.Include));
    }
}
