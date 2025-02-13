namespace Tests.Builders;

public class Builder_IncludeString
{
    public record Customer(int Id, Address Address, Contact Contact);
    public record Address(int Id, string City);
    public record Contact(int Id, string Email);

    [Fact]
    public void DoesNothing_GivenNoIncludeString()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        spec1.IncludeStrings.Should().BeEmpty();
        spec2.IncludeStrings.Should().BeEmpty();
    }

    [Fact]
    public void DoesNothing_GivenIncludeStringWithFalseCondition()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .Include(nameof(Address), false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Include(nameof(Address), false);

        spec1.IncludeStrings.Should().BeEmpty();
        spec2.IncludeStrings.Should().BeEmpty();
    }

    [Fact]
    public void AddsIncludeString_GivenIncludeString()
    {
        var includeString = nameof(Address);
        var spec1 = new Specification<Customer>();
        spec1.Query
            .Include(includeString);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Include(includeString);

        spec1.IncludeStrings.Should().ContainSingle();
        spec1.IncludeStrings.First().Should().BeSameAs(includeString);
        spec2.IncludeStrings.Should().ContainSingle();
        spec2.IncludeStrings.First().Should().BeSameAs(includeString);
    }

    [Fact]
    public void AddsIncludeString_GivenMultipleIncludeString()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .Include(nameof(Address))
            .Include(nameof(Contact));

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Include(nameof(Address))
            .Include(nameof(Contact));

        spec1.IncludeStrings.Should().HaveCount(2);
        spec2.IncludeStrings.Should().HaveCount(2);
    }
}
