namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class IncludeStringEvaluatorCustomSpecTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly IncludeStringEvaluator _evaluator = IncludeStringEvaluator.Instance;

    [Fact]
    public void QueriesMatch_GivenNoIncludeString()
    {
        var spec = new CustomSpecification<Store>();

        var actual = _evaluator
            .GetQuery(DbContext.Stores, spec)
            .ToQueryString();

        var expected = DbContext.Stores
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenIncludeString()
    {
        var spec = new CustomSpecification<Store>();
        spec.Includes.Add(nameof(Address));

        var actual = _evaluator
            .GetQuery(DbContext.Stores, spec)
            .ToQueryString();

        var expected = DbContext.Stores
            .Include(nameof(Address))
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenMultipleIncludeStrings()
    {
        var spec = new CustomSpecification<Store>();
        spec.Includes.Add(nameof(Address));
        spec.Includes.Add($"{nameof(Company)}.{nameof(Country)}");

        var actual = _evaluator
            .GetQuery(DbContext.Stores, spec)
            .ToQueryString();

        var expected = DbContext.Stores
            .Include(nameof(Address))
            .Include($"{nameof(Company)}.{nameof(Country)}")
            .ToQueryString();

        actual.Should().Be(expected);
    }

    public class CustomSpecification<T> : ISpecification<T>
    {
        public List<string> Includes { get; set; } = new();
        public List<WhereExpressionInfo<T>> Where { get; set; } = new();
        public List<SearchExpressionInfo<T>> Search { get; set; } = new();
        public IEnumerable<string> IncludeStrings => Includes;
        public IEnumerable<SearchExpressionInfo<T>> SearchCriterias => Search;
        public IEnumerable<WhereExpressionInfo<T>> WhereExpressions => Where;

        public ISpecificationBuilder<T> Query => throw new NotImplementedException();
        public IEnumerable<OrderExpressionInfo<T>> OrderExpressions => throw new NotImplementedException();
        public IEnumerable<IncludeExpressionInfo> IncludeExpressions => throw new NotImplementedException();
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
