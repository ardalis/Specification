namespace Tests.Validators;

public class WhereValidatorCustomSpecTests
{
    private static readonly WhereValidator _validator = WhereValidator.Instance;

    public record Customer(int Id, string Name);

    [Fact]
    public void ReturnsTrue_GivenEmptySpec()
    {
        var customer = new Customer(1, "Customer1");

        var spec = new CustomSpecification<Customer>();

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsTrue_GivenSpecWithSingleWhere_WithValidEntity()
    {
        var customer = new Customer(1, "Customer1");

        var spec = new CustomSpecification<Customer>();
        spec.Where.Add(new WhereExpressionInfo<Customer>(x => x.Id == 1));

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithSingleWhere_WithInvalidEntity()
    {
        var customer = new Customer(1, "Customer1");

        var spec = new CustomSpecification<Customer>();
        spec.Where.Add(new WhereExpressionInfo<Customer>(x => x.Id == 2));

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsTrue_GivenSpecWithMultipleWhere_WithValidEntity()
    {
        var customer = new Customer(1, "Customer1");

        var spec = new CustomSpecification<Customer>();
        spec.Where.Add(new WhereExpressionInfo<Customer>(x => x.Id == 1));
        spec.Where.Add(new WhereExpressionInfo<Customer>(x => x.Name == "Customer1"));

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithMultipleWhere_WithSingleInvalidValue()
    {
        var customer = new Customer(1, "Customer1");

        var spec = new CustomSpecification<Customer>();
        spec.Where.Add(new WhereExpressionInfo<Customer>(x => x.Id == 2));
        spec.Where.Add(new WhereExpressionInfo<Customer>(x => x.Name == "Customer1"));

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithMultipleWhere_WithAllInvalidValues()
    {
        var customer = new Customer(1, "Customer1");

        var spec = new CustomSpecification<Customer>();
        spec.Where.Add(new WhereExpressionInfo<Customer>(x => x.Id == 1));
        spec.Where.Add(new WhereExpressionInfo<Customer>(x => x.Name == "Customer2"));

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }

    public class CustomSpecification<T> : ISpecification<T>
    {
        public List<WhereExpressionInfo<T>> Where { get; set; } = new();
        public List<SearchExpressionInfo<T>> Search { get; set; } = new();
        public IEnumerable<SearchExpressionInfo<T>> SearchCriterias => Search;
        public IEnumerable<WhereExpressionInfo<T>> WhereExpressions => Where;

        public ISpecificationBuilder<T> Query => throw new NotImplementedException();
        public IEnumerable<OrderExpressionInfo<T>> OrderExpressions => throw new NotImplementedException();
        public IEnumerable<IncludeExpressionInfo> IncludeExpressions => throw new NotImplementedException();
        public IEnumerable<string> IncludeStrings => throw new NotImplementedException();
        public Dictionary<string, object> Items => throw new NotImplementedException();
        public int Take => throw new NotImplementedException();
        public int Skip => throw new NotImplementedException();
        public Func<IEnumerable<T>, IEnumerable<T>>? PostProcessingAction => throw new NotImplementedException();
        public IEnumerable<string> QueryTags => throw new NotImplementedException();
        public bool CacheEnabled => throw new NotImplementedException();
        public string? CacheKey => throw new NotImplementedException();
        public bool AsTracking => throw new NotImplementedException();
        public bool AsNoTracking => throw new NotImplementedException();
        public bool AsSplitQuery => throw new NotImplementedException();
        public bool AsNoTrackingWithIdentityResolution => throw new NotImplementedException();
        public bool IgnoreQueryFilters => throw new NotImplementedException();
        public bool IgnoreAutoIncludes => throw new NotImplementedException();
        public IEnumerable<T> Evaluate(IEnumerable<T> entities)
            => throw new NotImplementedException();
        public bool IsSatisfiedBy(T entity)
            => throw new NotImplementedException();
    }
}
