namespace Tests.Evaluators;

// This is a special case where users have custom ISpecification<T> implementation but use our evaluator.
[Collection("SharedCollection")]
public class SearchEvaluatorCustomSpecTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly SearchEvaluator _evaluator = SearchEvaluator.Instance;

    [Fact]
    public void QueriesMatch_GivenNoSearch()
    {
        var spec = new CustomSpecification<Store>();
        spec.Where.Add(new WhereExpressionInfo<Store>(x => x.Id > 0));

        var actual = _evaluator.GetQuery(DbContext.Stores, spec)
            .ToQueryString();

        var expected = DbContext.Stores
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenSingleSearch()
    {
        var storeTerm = "ab1";

        var spec = new CustomSpecification<Store>();
        spec.Where.Add(new WhereExpressionInfo<Store>(x => x.Id > 0));
        spec.Search.Add(new SearchExpressionInfo<Store>(x => x.Name, $"%{storeTerm}%"));

        var actual = _evaluator.GetQuery(DbContext.Stores, spec)
            .ToQueryString();

        var expected = DbContext.Stores
            .Where(x => EF.Functions.Like(x.Name, $"%{storeTerm}%"))
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenMultipleSearch()
    {
        var storeTerm = "ab1";
        var companyTerm = "ab2";
        var countryTerm = "ab3";
        var streetTerm = "ab4";

        var spec = new CustomSpecification<Store>();
        spec.Where.Add(new WhereExpressionInfo<Store>(x => x.Id > 0));
        spec.Search.Add(new SearchExpressionInfo<Store>(x => x.Name, $"%{storeTerm}%"));
        spec.Search.Add(new SearchExpressionInfo<Store>(x => x.Company.Name, $"%{companyTerm}%"));
        spec.Search.Add(new SearchExpressionInfo<Store>(x => x.Address.Street, $"%{streetTerm}%", 2));
        spec.Search.Add(new SearchExpressionInfo<Store>(x => x.Company.Country.Name, $"%{countryTerm}%", 3));

        var actual = _evaluator.GetQuery(DbContext.Stores, spec)
            .ToQueryString();

        var expected = DbContext.Stores
            .Where(x => EF.Functions.Like(x.Name, $"%{storeTerm}%")
                    || EF.Functions.Like(x.Company.Name, $"%{companyTerm}%"))
            .Where(x => EF.Functions.Like(x.Address.Street, $"%{streetTerm}%"))
            .Where(x => EF.Functions.Like(x.Company.Country.Name, $"%{countryTerm}%"))
            .ToQueryString();

        actual.Should().Be(expected);
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

        void ISpecification<T>.CopyTo(Specification<T> otherSpec)
        {
            throw new NotImplementedException();
        }
    }
}
