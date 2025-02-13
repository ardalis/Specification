namespace Tests.Validators;

public class WhereValidatorTests
{
    private static readonly WhereValidator _validator = WhereValidator.Instance;

    public record Customer(int Id, string Name);

    [Fact]
    public void ReturnsTrue_GivenEmptySpec()
    {
        var customer = new Customer(1, "Customer1");

        var spec = new Specification<Customer>();

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsTrue_GivenSpecWithSingleWhere_WithValidEntity()
    {
        var customer = new Customer(1, "Customer1");

        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id == 1);

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithSingleWhere_WithInvalidEntity()
    {
        var customer = new Customer(1, "Customer1");

        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id == 2);

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsTrue_GivenSpecWithMultipleWhere_WithValidEntity()
    {
        var customer = new Customer(1, "Customer1");

        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id == 1)
            .Where(x => x.Name == "Customer1");

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithMultipleWhere_WithSingleInvalidValue()
    {
        var customer = new Customer(1, "Customer1");

        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id == 2)
            .Where(x => x.Name == "Customer1");

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithMultipleWhere_WithAllInvalidValues()
    {
        var customer = new Customer(1, "Customer1");

        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id == 2)
            .Where(x => x.Name == "Customer2");

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }
}
