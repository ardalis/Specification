namespace Tests.Builders;

public class Builder_TagWith
{
    public record Customer(int Id, string Name);

    [Fact]
    public void DoesNothing_GivenNoTag()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        spec1.Tag.Should().BeNull();
        spec2.Tag.Should().BeNull();
    }

    [Fact]
    public void DoesNothing_GivenTagWithFalseCondition()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .TagWith("asd", false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .TagWith("asd", false);

        spec1.Tag.Should().BeNull();
        spec2.Tag.Should().BeNull();
    }

    [Fact]
    public void SetsTag_GivenTag()
    {
        var tag = "asd";

        var spec1 = new Specification<Customer>();
        spec1.Query
            .TagWith(tag);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .TagWith(tag);

        spec1.Tag.Should().Be(tag);
        spec2.Tag.Should().Be(tag);
    }
}
