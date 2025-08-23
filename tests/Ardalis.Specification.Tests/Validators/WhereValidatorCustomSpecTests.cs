namespace Tests.Validators;

public class WhereValidatorCustomSpecTests
{
    private static readonly WhereValidator _validator = WhereValidator.Instance;

    public record Customer(int Id, string Name);

    [Fact]
    public void ReturnsTrue_GivenEmptySpec()
    {
        var customer = new Customer(1, "Customer1");

        var spec = Substitute.For<ISpecification<Customer>>();

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsTrue_GivenSpecWithSingleWhere_WithValidEntity()
    {
        var customer = new Customer(1, "Customer1");

        var spec = Substitute.For<ISpecification<Customer>>();
        spec.WhereExpressions.Returns(
        [
            new WhereExpressionInfo<Customer>(x => x.Id == 1)
        ]);

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithSingleWhere_WithInvalidEntity()
    {
        var customer = new Customer(1, "Customer1");

        var spec = Substitute.For<ISpecification<Customer>>();
        spec.WhereExpressions.Returns(
        [
            new WhereExpressionInfo<Customer>(x => x.Id == 2)
        ]);

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsTrue_GivenSpecWithMultipleWhere_WithValidEntity()
    {
        var customer = new Customer(1, "Customer1");

        var spec = Substitute.For<ISpecification<Customer>>();
        spec.WhereExpressions.Returns(
        [
            new WhereExpressionInfo<Customer>(x => x.Id == 1),
            new WhereExpressionInfo<Customer>(x => x.Name == "Customer1")
        ]);

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithMultipleWhere_WithSingleInvalidValue()
    {
        var customer = new Customer(1, "Customer1");

        var spec = Substitute.For<ISpecification<Customer>>();
        spec.WhereExpressions.Returns(
        [
            new WhereExpressionInfo<Customer>(x => x.Id == 2),
            new WhereExpressionInfo<Customer>(x => x.Name == "Customer1")
        ]);

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithMultipleWhere_WithAllInvalidValues()
    {
        var customer = new Customer(1, "Customer1");

        var spec = Substitute.For<ISpecification<Customer>>();
        spec.WhereExpressions.Returns(
        [
            new WhereExpressionInfo<Customer>(x => x.Id == 1),
            new WhereExpressionInfo<Customer>(x => x.Name == "Customer2")
        ]);

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }
}
