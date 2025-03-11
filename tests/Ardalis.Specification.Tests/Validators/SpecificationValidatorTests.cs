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

#if NET8_0_OR_GREATER
    [Fact]
    public void Constructor_SetsProvidedValidators()
    {
        var validators = new List<IValidator>
        {
            WhereValidator.Instance,
            SearchValidator.Instance,
            WhereValidator.Instance,
        };

        var validator = new SpecificationValidator(validators);

        var result = ValidatorsOf(validator);
        result.Should().HaveSameCount(validators);
        result.Should().Equal(validators);
    }

    [Fact]
    public void DerivedSpecificationValidatorCanAlterDefaultValidators()
    {
        var validator = new SpecificationValidatorDerived();

        var result = ValidatorsOf(validator);
        result.Should().HaveCount(4);
        result[0].Should().BeOfType<SearchValidator>();
        result[1].Should().BeOfType<WhereValidator>();
        result[2].Should().BeOfType<SearchValidator>();
        result[3].Should().BeOfType<WhereValidator>();
    }

    private class SpecificationValidatorDerived : SpecificationValidator
    {
        public SpecificationValidatorDerived()
        {
            Validators.Add(WhereValidator.Instance);
            Validators.Insert(0, SearchValidator.Instance);
        }
    }

    [System.Runtime.CompilerServices.UnsafeAccessor(System.Runtime.CompilerServices.UnsafeAccessorKind.Field, Name = "<Validators>k__BackingField")]
    public static extern ref List<IValidator> ValidatorsOf(SpecificationValidator @this);

#endif
}
