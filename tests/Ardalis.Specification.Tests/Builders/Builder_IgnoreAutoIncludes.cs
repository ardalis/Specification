namespace Tests.Builders;

public class Builder_IgnoreAutoIncludes
{
    public record Customer(int Id, string Name);

    [Fact]
    public void DoesNothing_GivenNoAutoIncludes()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        spec1.IgnoreAutoIncludes.Should().Be(false);
        spec2.IgnoreAutoIncludes.Should().Be(false);
    }

    [Fact]
    public void DoesNothing_GivenAutoIncludesWithFalseCondition()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .IgnoreAutoIncludes(false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .IgnoreAutoIncludes(false);

        spec1.IgnoreAutoIncludes.Should().Be(false);
        spec2.IgnoreAutoIncludes.Should().Be(false);
    }

    [Fact]
    public void SetsAutoIncludes_GivenAutoIncludes()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .IgnoreAutoIncludes();

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .IgnoreAutoIncludes();

        spec1.IgnoreAutoIncludes.Should().Be(true);
        spec2.IgnoreAutoIncludes.Should().Be(true);
    }
}
