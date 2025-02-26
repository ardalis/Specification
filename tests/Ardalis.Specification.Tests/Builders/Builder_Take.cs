namespace Tests.Builders;

public class Builder_Take
{
    public record Customer(int Id, string Name);

    [Fact]
    public void DoesNothing_GivenNoTake()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        spec1.Take.Should().Be(-1);
        spec2.Take.Should().Be(-1);
    }

    [Fact]
    public void DoesNothing_GivenTakeWithFalseCondition()
    {
        var take = 1;

        var spec1 = new Specification<Customer>();
        spec1.Query
            .Take(take, false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Take(take, false);

        spec1.Take.Should().Be(-1);
        spec2.Take.Should().Be(-1);
    }

    [Fact]
    public void SetsTake_GivenTake()
    {
        var take = 1;

        var spec1 = new Specification<Customer>();
        spec1.Query
            .Take(take);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .Take(take);

        spec1.Take.Should().Be(take);
        spec2.Take.Should().Be(take);
    }

    [Fact]
    public void ThrowsDuplicateTakeException_GivenNewTake()
    {
        var take = 1;
        var takeNew = 2;

        var spec1 = new Specification<Customer>();
        var sut1 = () => spec1.Query
            .Take(take)
            .Take(takeNew);

        var spec2 = new Specification<Customer, string>();
        var sut2 = () => spec2.Query
            .Take(take)
            .Take(takeNew);

        sut1.Should().Throw<DuplicateTakeException>();
        sut2.Should().Throw<DuplicateTakeException>();
    }
}
