namespace Tests.Builders;

public class Builder_Skip
{
    public record Customer(int Id, string Name);

    [Fact]
    public void DoesNothing_GivenNoSkip()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        spec1.Skip.Should().Be(-1);
        spec2.Skip.Should().Be(-1);
    }

    [Fact]
    public void DoesNothing_GivenSkipWithFalseCondition()
    {
        var skip = 1;

        var spec1 = new Specification<Customer>();
        spec1.Query
            .Skip(skip, false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Skip(skip, false);

        spec1.Skip.Should().Be(-1);
        spec2.Skip.Should().Be(-1);
    }

    [Fact]
    public void SetsSkip_GivenSkip()
    {
        var skip = 1;

        var spec1 = new Specification<Customer>();
        spec1.Query
            .Skip(skip);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Skip(skip);

        spec1.Skip.Should().Be(skip);
        spec2.Skip.Should().Be(skip);
    }

    [Fact]
    public void ThrowsDuplicateSkipException_GivenNewSkip()
    {
        var skip = 1;
        var skipNew = 2;

        var spec1 = new Specification<Customer>();
        var sut1 = () => spec1.Query
            .Skip(skip)
            .Skip(skipNew);

        var spec2 = new Specification<Customer, string>();
        var sut2 = () => spec2.Query
            .Skip(skip)
            .Skip(skipNew);

        sut1.Should().Throw<DuplicateSkipException>();
        sut2.Should().Throw<DuplicateSkipException>();
    }
}
