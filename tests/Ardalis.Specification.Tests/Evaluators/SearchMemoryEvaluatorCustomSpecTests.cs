
namespace Tests.Evaluators;

// This is a special case where users have custom ISpecification<T> implementations but use our evaluator.
public class SearchMemoryEvaluatorCustomSpecTests
{
    private static readonly SearchMemoryEvaluator _evaluator = SearchMemoryEvaluator.Instance;

    public record Customer(int Id, string FirstName, string? LastName);

    [Fact]
    public void Filters_GivenSingleSearch()
    {
        List<Customer> input =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "aaaa"),
            new(3, "aaaa", "axya"),
            new(4, "aaaa", null)
        ];

        List<Customer> expected =
        [
            new(2, "aaaa", "aaaa"),
        ];

        var spec = new CustomSpecification<Customer>();
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.LastName, "%aa%"));

        // Not materializing with ToList() intentionally to test cloning in the iterator
        var actual = _evaluator.Evaluate(input, spec);

        // Multiple iterations will force cloning
        actual.Should().HaveSameCount(expected);
        actual.Should().Equal(expected);
    }

    [Fact]
    public void Filters_GivenSearchInSameGroup()
    {
        List<Customer> input =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "aaaa"),
            new(3, "aaaa", "axya"),
            new(4, "aaaa", null)
        ];

        List<Customer> expected =
        [
            new(1, "axxa", "axya"),
            new(3, "aaaa", "axya")
        ];

        var spec = new CustomSpecification<Customer>();
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.FirstName, "%xx%"));
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.LastName, "%xy%"));

        // Not materializing with ToList() intentionally to test cloning in the iterator
        var actual = _evaluator.Evaluate(input, spec);

        // Multiple iterations will force cloning
        actual.Should().HaveSameCount(expected);
        actual.Should().Equal(expected);
    }

    [Fact]
    public void Filters_GivenSearchInDifferentGroup()
    {
        List<Customer> input =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "aaaa"),
            new(3, "aaaa", "axya"),
            new(4, "aaaa", null)
        ];

        List<Customer> expected =
        [
            new(1, "axxa", "axya")
        ];

        var spec = new CustomSpecification<Customer>();
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.FirstName, "%xx%", 1));
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.LastName, "%xy%", 2));

        var actual = _evaluator.Evaluate(input, spec).ToList();

        actual.Should().Equal(expected);
    }

    [Fact]
    public void Filters_GivenSearchComplexGrouping()
    {
        List<Customer> input =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "aaaa"),
            new(3, "axxa", "axza"),
            new(4, "aaaa", null),
            new(5, "axxa", null)
        ];

        List<Customer> expected =
        [
            new(1, "axxa", "axya"),
            new(3, "axxa", "axza"),
        ];

        var spec = new CustomSpecification<Customer>();
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.FirstName, "%xx%", 1));
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.LastName, "%xy%", 2));
        spec.Search.Add(new SearchExpressionInfo<Customer>(x => x.LastName, "%xz%", 2));

        var actual = _evaluator.Evaluate(input, spec).ToList();

        actual.Should().Equal(expected);
    }

    [Fact]
    public void DoesNotFilter_GivenNoSearch()
    {
        List<Customer> input =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "aaaa"),
            new(3, "aaaa", "axya")
        ];

        List<Customer> expected =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "aaaa"),
            new(3, "aaaa", "axya")
        ];

        var spec = new CustomSpecification<Customer>();
        spec.Where.Add(new WhereExpressionInfo<Customer>(x => x.Id > 0));

        var actual = _evaluator.Evaluate(input, spec);

        actual.Should().Equal(expected);
    }

    [Fact]
    public void DoesNotFilter_GivenEmptySpec()
    {
        List<Customer> input =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "aaaa"),
            new(3, "aaaa", "axya")
        ];

        List<Customer> expected =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "aaaa"),
            new(3, "aaaa", "axya")
        ];

        var spec = new CustomSpecification<Customer>();

        var actual = _evaluator.Evaluate(input, spec);

        actual.Should().Equal(expected);
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
