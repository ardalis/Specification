namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class QueryTagEvaluatorCustomTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly QueryTagEvaluator _evaluator = QueryTagEvaluator.Instance;

    [Fact]
    public void QueriesMatch_GivenTag()
    {
        var tag = "asd";

        var spec = new CustomSpecification<Country>();
        spec.Tags.Add(tag);

        var actual = _evaluator.GetQuery(DbContext.Countries, spec)
            .ToQueryString();

        var expected = DbContext.Countries
            .TagWith(tag)
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenMultipleTags()
    {
        var tag1 = "asd";
        var tag2 = "qwe";

        var spec = new CustomSpecification<Country>();
        spec.Tags.Add(tag1);
        spec.Tags.Add(tag2);

        var actual = _evaluator.GetQuery(DbContext.Countries, spec)
            .ToQueryString();

        var expected = DbContext.Countries
            .TagWith(tag1)
            .TagWith(tag2)
            .ToQueryString();

        actual.Should().Be(expected);
    }


    [Fact]
    public void DoesNothing_GivenNoTag()
    {
        var spec = new CustomSpecification<Country>();

        var actual = _evaluator.GetQuery(DbContext.Countries, spec)
            .Expression
            .ToString();

        var expected = DbContext.Countries
            .AsQueryable()
            .Expression
            .ToString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void Applies_GivenSingleTag()
    {
        var tag = "asd";

        var spec = new CustomSpecification<Country>();
        spec.Tags.Add(tag);

        var actual = _evaluator.GetQuery(DbContext.Countries, spec)
            .Expression
            .ToString();

        var expected = DbContext.Countries
            .TagWith(tag)
            .Expression
            .ToString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void Applies_GivenTwoTags()
    {
        var tag1 = "asd";
        var tag2 = "qwe";

        var spec = new CustomSpecification<Country>();
        spec.Tags.Add(tag1);
        spec.Tags.Add(tag2);

        var actual = _evaluator.GetQuery(DbContext.Countries, spec)
            .Expression
            .ToString();

        var expected = DbContext.Countries
            .TagWith(tag1)
            .TagWith(tag2)
            .Expression
            .ToString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void Applies_GivenMultipleTags()
    {
        var tag1 = "asd";
        var tag2 = "qwe";
        var tag3 = "zxc";

        var spec = new CustomSpecification<Country>();
        spec.Tags.Add(tag1);
        spec.Tags.Add(tag2);
        spec.Tags.Add(tag3);

        var actual = _evaluator.GetQuery(DbContext.Countries, spec)
            .Expression
            .ToString();

        var expected = DbContext.Countries
            .TagWith(tag1)
            .TagWith(tag2)
            .TagWith(tag3)
            .Expression
            .ToString();

        actual.Should().Be(expected);
    }

    public class CustomSpecification<T> : ISpecification<T>
    {
        public List<string> Tags { get; set; } = new();
        public List<WhereExpressionInfo<T>> Where { get; set; } = new();
        public List<SearchExpressionInfo<T>> Search { get; set; } = new();
        public IEnumerable<string> QueryTags => Tags;
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
