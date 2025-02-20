namespace Tests.Builders;

public class Builder_AsTracking
{
    public record Customer(int Id, string Name);

    [Fact]
    public void DoesNothing_GivenNoAsTracking()
    {
        var spec1 = new Specification<Customer>();
        var spec2 = new Specification<Customer, string>();

        spec1.AsTracking.Should().Be(false);
        spec2.AsTracking.Should().Be(false);
    }

    [Fact]
    public void DoesNothing_GivenAsTrackingWithFalseCondition()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .AsTracking(false);

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .AsTracking(false);

        spec1.AsTracking.Should().Be(false);
        spec2.AsTracking.Should().Be(false);
    }

    [Fact]
    public void SetsAsNoTracking_GivenAsTracking()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .AsTracking();

        var spec2 = new Specification<Customer, string>();
        spec2.Query
            .AsTracking();

        spec1.AsTracking.Should().Be(true);
        spec2.AsTracking.Should().Be(true);
    }

    [Fact]
    public void SetsAsTracking_GivenOtherTrackingBehavior()
    {
        var spec1 = new Specification<Customer>();
        spec1.Query
            .AsNoTracking()
            .AsNoTrackingWithIdentityResolution()
            .AsTracking();

        var spec2 = new Specification<Customer>();
        spec2.Query
            .AsNoTracking()
            .AsNoTrackingWithIdentityResolution()
            .AsTracking();

        spec1.AsNoTracking.Should().Be(false);
        spec1.AsNoTrackingWithIdentityResolution.Should().Be(false);
        spec1.AsTracking.Should().Be(true);
        spec2.AsNoTracking.Should().Be(false);
        spec2.AsNoTrackingWithIdentityResolution.Should().Be(false);
        spec2.AsTracking.Should().Be(true);
    }
}
