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

        var spec = new CustomSpecification<Customer>();

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsTrue_GivenNoSearch()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var spec = new CustomSpecification<Customer>();
        spec.Where.Add(new WhereExpressionInfo<Customer>(x => x.Id == 1));

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsTrue_GivenSpecWithSingleSearch_WithValidEntity()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var term = "irst";
        var spec = new CustomSpecification<Customer>();
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.FirstName, $"%{term}%"));

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithSingleSearch_WithInvalidEntity()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var term = "irstt";
        var spec = new CustomSpecification<Customer>();
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.FirstName, $"%{term}%"));

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithSingleSearch_WithNullProperty()
    {
        var customer = new Customer(1, "FirstName1", null);

        var term = "irst";
        var spec = new CustomSpecification<Customer>();
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.LastName, $"%{term}%"));

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsTrue_GivenSpecWithMultipleSearchSameGroup_WithValidEntity()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var term = "irst";
        var spec = new CustomSpecification<Customer>();
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.FirstName, $"%{term}%"));
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.LastName, $"%{term}%"));

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithMultipleSearchSameGroup_WithInvalidEntity()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var term = "irstt";
        var spec = new CustomSpecification<Customer>();
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.FirstName, $"%{term}%"));
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.LastName, $"%{term}%"));

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsTrue_GivenSpecWithMultipleSearchDifferentGroup_WithValidEntity()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var term = "Name";
        var spec = new CustomSpecification<Customer>();
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.FirstName, $"%{term}%", 1));
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.LastName, $"%{term}%", 2));

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithMultipleSearchDifferentGroup_WithInvalidEntity()
    {
        var customer = new Customer(1, "FirstName1", "LastName1");

        var term = "irst";
        var spec = new CustomSpecification<Customer>();
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.FirstName, $"%{term}%", 1));
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.LastName, $"%{term}%", 2));

        var result = _validator.IsValid(customer, spec);

        result.Should().BeFalse();
    }

    [Fact]
    public void ReturnsTrue_GivenSpecWithMultipleSearchSameGroup_WithNullProperty()
    {
        var customer = new Customer(1, "FirstName1", null);

        var term = "irst";
        var spec = new CustomSpecification<Customer>();
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.FirstName, $"%{term}%", 1));
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.LastName, $"%{term}%", 1));

        var result = _validator.IsValid(customer, spec);

        result.Should().BeTrue();
    }

    [Fact]
    public void ReturnsFalse_GivenSpecWithMultipleSearchDifferentGroup_WithNullProperty()
    {
        var customer = new Customer(1, "FirstName1", null);

        var term = "irst";
        var spec = new CustomSpecification<Customer>();
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.FirstName, $"%{term}%", 1));
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.LastName, $"%{term}%", 2));

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
