namespace Tests.Validators;

public class SearchValidatorTests
{
    private static readonly SearchValidator _validator = SearchValidator.Instance;

    public record Customer(int Id, string FirstName, string? LastName);

    [Fact]
    public void ReturnsTrue_GivenEmptySpec()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var spec = new Specification<Customer>();

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsTrue_GivenNoSearch()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id == 1);

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsTrue_GivenSpecWithSingleSearch_WithValidEntity()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var term = "irst";
        var spec = new Specification<Customer>();
        spec.Query
            .Search(x => x.FirstName, $"%{term}%");

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithSingleSearch_WithInvalidEntity()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var term = "irstt";
        var spec = new Specification<Customer>();
        spec.Query
            .Search(x => x.FirstName, $"%{term}%");

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithSingleSearch_WithNullProperty()
    {
        var customer = new Customer(1, "FirstName1", null);

        var term = "irst";
        var spec = new Specification<Customer>();
        spec.Query
            .Search(x => x.LastName, $"%{term}%");

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsTrue_GivenSpecWithMultipleSearchSameGroup_WithValidEntity()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var term = "irst";
        var spec = new Specification<Customer>();
        spec.Query
            .Search(x => x.FirstName, $"%{term}%")
            .Search(x => x.LastName, $"%{term}%");

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithMultipleSearchSameGroup_WithInvalidEntity()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var term = "irstt";
        var spec = new Specification<Customer>();
        spec.Query
            .Search(x => x.FirstName, $"%{term}%")
            .Search(x => x.LastName, $"%{term}%");

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsTrue_GivenSpecWithMultipleSearchDifferentGroup_WithValidEntity()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var term = "Name";
        var spec = new Specification<Customer>();
        spec.Query
            .Search(x => x.FirstName, $"%{term}%", 1)
            .Search(x => x.LastName, $"%{term}%", 2);

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithMultipleSearchDifferentGroup_WithInvalidEntity()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var term = "irst";
        var spec = new Specification<Customer>();
        spec.Query
            .Search(x => x.FirstName, $"%{term}%", 1)
            .Search(x => x.LastName, $"%{term}%", 2);

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsTrue_GivenSpecWithMultipleSearchSameGroup_WithNullProperty()
    {
        var customer = new Customer(1, "FirstName1", null);

        var term = "irst";
        var spec = new Specification<Customer>();
        spec.Query
            .Search(x => x.FirstName, $"%{term}%", 1)
            .Search(x => x.LastName, $"%{term}%", 1);

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithMultipleSearchDifferentGroup_WithNullProperty()
    {
        var customer = new Customer(1, "FirstName1", null);

        var term = "irst";
        var spec = new Specification<Customer>();
        spec.Query
            .Search(x => x.FirstName, $"%{term}%", 1)
            .Search(x => x.LastName, $"%{term}%", 2);

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }
}
