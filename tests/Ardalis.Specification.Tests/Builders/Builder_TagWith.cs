namespace Tests.Builders;

public class Builder_TagWith
{
    public record Customer(int Id, string Name);

    [Fact]
    public void DoesNothing_GivenNoTag()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        spec1.QueryTags.Should().BeSameAs(Enumerable.Empty<string>());
        spec2.QueryTags.Should().BeSameAs(Enumerable.Empty<string>());
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

        spec1.QueryTags.Should().BeSameAs(Enumerable.Empty<string>());
        spec2.QueryTags.Should().BeSameAs(Enumerable.Empty<string>());
    }

    [Fact]
    public void SetsTag_GivenSingleTag()
    {
        var tag = "asd";

        var spec1 = new Specification<Customer>();
        spec1.Query
            .TagWith(tag);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .TagWith(tag);

        spec1.QueryTags.Should().ContainSingle();
        spec1.QueryTags.First().Should().Be(tag);
        spec2.QueryTags.Should().ContainSingle();
        spec2.QueryTags.First().Should().Be(tag);
    }

    [Fact]
    public void SetsTags_GivenTwoTags()
    {
        var tag1 = "asd";
        var tag2 = "qwe";

        var spec1 = new Specification<Customer>();
        spec1.Query
            .TagWith(tag1)
            .TagWith(tag2);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .TagWith(tag1)
            .TagWith(tag2);

        spec1.QueryTags.Should().HaveCount(2);
        spec1.QueryTags.First().Should().Be(tag1);
        spec1.QueryTags.Skip(1).First().Should().Be(tag2);
        spec2.QueryTags.Should().HaveCount(2);
        spec2.QueryTags.First().Should().Be(tag1);
        spec2.QueryTags.Skip(1).First().Should().Be(tag2);
    }

    [Fact]
    public void SetsTags_GivenMultipleTags()
    {
        var tag1 = "asd";
        var tag2 = "qwe";
        var tag3 = "zxc";

        var spec1 = new Specification<Customer>();
        spec1.Query
            .TagWith(tag1)
            .TagWith(tag2)
            .TagWith(tag3);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .TagWith(tag1)
            .TagWith(tag2)
            .TagWith(tag3);

        spec1.QueryTags.Should().HaveCount(3);
        spec1.QueryTags.First().Should().Be(tag1);
        spec1.QueryTags.Skip(1).First().Should().Be(tag2);
        spec1.QueryTags.Skip(2).First().Should().Be(tag3);

        spec2.QueryTags.Should().HaveCount(3);
        spec2.QueryTags.First().Should().Be(tag1);
        spec2.QueryTags.Skip(1).First().Should().Be(tag2);
        spec2.QueryTags.Skip(2).First().Should().Be(tag3);
    }
}
