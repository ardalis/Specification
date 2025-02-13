namespace Tests.Validators;

public class SpecificationValidatorTests
{
    private static readonly SpecificationValidator _validatorDefault = SpecificationValidator.Default;

    public record Customer(int Id, string FirstName, string LastName);

    [Fact]
    public void ReturnTrue_GivenAllValidatorsPass()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var searchTerm = "irst";
        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id == 1)
            .Search(x => x.FirstName, $"%{searchTerm}%");

        var result = _validatorDefault.IsValid(customer, spec);
        var resultFromSpec = spec.IsSatisfiedBy(customer);

        result.Should().Be(resultFromSpec);
        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnTrue_GivenEmptySpec()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var spec = new Specification<Customer>();

        var result = _validatorDefault.IsValid(customer, spec);
        var resultFromSpec = spec.IsSatisfiedBy(customer);

        result.Should().Be(resultFromSpec);
        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnFalse_GivenOneValidatorFails()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var searchTerm = "irst";
        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id == 1)
            .Search(x => x.FirstName, $"%{searchTerm}%");

        var result = _validatorDefault.IsValid(customer, spec);
        var resultFromSpec = spec.IsSatisfiedBy(customer);

        result.Should().Be(resultFromSpec);
        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnFalse_GivenAllValidatorsFail()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var searchTerm = "irstt";
        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id == 2)
            .Search(x => x.FirstName, $"%{searchTerm}%");

        var result = _validatorDefault.IsValid(customer, spec);
        var resultFromSpec = spec.IsSatisfiedBy(customer);

        result.Should().Be(resultFromSpec);
        result.Should().BeFalse();
    }
}
