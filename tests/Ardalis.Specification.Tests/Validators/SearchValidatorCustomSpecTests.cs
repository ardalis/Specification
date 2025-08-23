namespace Tests.Validators;

// This is a special case where users have custom ISpecification<T> implementations but use our validator.
public class SearchValidatorCustomSpecTests
{
    private static readonly SearchValidator _validator = SearchValidator.Instance;

    public record Customer(int Id, string FirstName, string? LastName);

    [Fact]
    public void ReturnsTrue_GivenEmptySpec()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var spec = Substitute.For<ISpecification<Customer>>();

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsTrue_GivenNoSearch()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var spec = Substitute.For<ISpecification<Customer>>();
        spec.WhereExpressions.Returns(
        [
            new WhereExpressionInfo<Customer>(x => x.Id == 1)
        ]);

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsTrue_GivenSpecWithSingleSearch_WithValidEntity()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var term = "irst";
        var spec = Substitute.For<ISpecification<Customer>>();
        spec.SearchCriterias.Returns(
        [
            new SearchExpressionInfo<Customer>(x => x.FirstName, $"%{term}%")
        ]);

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithSingleSearch_WithInvalidEntity()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var term = "irstt";
        var spec = Substitute.For<ISpecification<Customer>>();
        spec.SearchCriterias.Returns(
        [
            new SearchExpressionInfo<Customer>(x => x.FirstName, $"%{term}%")
        ]);

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithSingleSearch_WithNullProperty()
    {
        var customer = new Customer(1, "FirstName1", null);

        var term = "irst";
        var spec = Substitute.For<ISpecification<Customer>>();
        spec.SearchCriterias.Returns(
        [
            new SearchExpressionInfo<Customer>(x => x.LastName, $"%{term}%")
        ]);

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsTrue_GivenSpecWithMultipleSearchSameGroup_WithValidEntity()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var term = "irst";
        var spec = Substitute.For<ISpecification<Customer>>();
        spec.SearchCriterias.Returns(
        [
            new SearchExpressionInfo<Customer>(x => x.FirstName, $"%{term}%"),
            new SearchExpressionInfo<Customer>(x => x.LastName, $"%{term}%")
        ]);

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithMultipleSearchSameGroup_WithInvalidEntity()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var term = "irstt";
        var spec = Substitute.For<ISpecification<Customer>>();
        spec.SearchCriterias.Returns(
        [
            new SearchExpressionInfo<Customer>(x => x.FirstName, $"%{term}%"),
            new SearchExpressionInfo<Customer>(x => x.LastName, $"%{term}%")
        ]);

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsTrue_GivenSpecWithMultipleSearchDifferentGroup_WithValidEntity()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var term = "Name";
        var spec = Substitute.For<ISpecification<Customer>>();
        spec.SearchCriterias.Returns(
        [
            new SearchExpressionInfo<Customer>(x => x.FirstName, $"%{term}%", 1),
            new SearchExpressionInfo<Customer>(x => x.LastName, $"%{term}%", 2)
        ]);

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithMultipleSearchDifferentGroup_WithInvalidEntity()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var term = "irst";
        var spec = Substitute.For<ISpecification<Customer>>();
        spec.SearchCriterias.Returns(
        [
            new SearchExpressionInfo<Customer>(x => x.FirstName, $"%{term}%", 1),
            new SearchExpressionInfo<Customer>(x => x.LastName, $"%{term}%", 2)
        ]);

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsTrue_GivenSpecWithMultipleSearchSameGroup_WithNullProperty()
    {
        var customer = new Customer(1, "FirstName1", null);

        var term = "irst";
        var spec = Substitute.For<ISpecification<Customer>>();
        spec.SearchCriterias.Returns(
        [
            new SearchExpressionInfo<Customer>(x => x.FirstName, $"%{term}%", 1),
            new SearchExpressionInfo<Customer>(x => x.LastName, $"%{term}%", 1)
        ]);

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithMultipleSearchDifferentGroup_WithNullProperty()
    {
        var customer = new Customer(1, "FirstName1", null);

        var term = "irst";
        var spec = Substitute.For<ISpecification<Customer>>();
        spec.SearchCriterias.Returns(
        [
            new SearchExpressionInfo<Customer>(x => x.FirstName, $"%{term}%", 1),
            new SearchExpressionInfo<Customer>(x => x.LastName, $"%{term}%", 2)
        ]);

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }
}
