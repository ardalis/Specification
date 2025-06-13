namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class IncludeEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly IncludeEvaluator _evaluator = IncludeEvaluator.Instance;

    [Fact]
    public void QueriesMatch_GivenNoIncludeExpression()
    {
        var spec = new Specification<Store>();

        var actual = _evaluator
            .GetQuery(DbContext.Stores, spec)
            .ToQueryString();

        var expected = DbContext.Stores
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenSingleIncludeExpression()
    {
        var spec = new Specification<Store>();
        spec.Query
            .Include(x => x.Products.Where(x => x.Id > 10));

        var actual = _evaluator
            .GetQuery(DbContext.Stores, spec)
            .ToQueryString();

        var expected = DbContext.Stores
            .Include(x => x.Products.Where(x => x.Id > 10))
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenIncludeExpressions()
    {
        var spec = new Specification<Store>();
        spec.Query
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country);

        var actual = _evaluator
            .GetQuery(DbContext.Stores, spec)
            .ToQueryString();

        var expected = DbContext.Stores
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenInheritanceModel()
    {
        var spec = new Specification<Bar>();
        spec.Query
            .Include(x => x.BarChildren)
            .ThenInclude(x => (x as BarDerived)!.BarDerivedInfo);

        var actual = _evaluator
            .GetQuery(DbContext.Bars, spec)
            .ToQueryString();

        var expected = DbContext.Bars
            .Include(x => x.BarChildren)
            .ThenInclude(x => (x as BarDerived)!.BarDerivedInfo)
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenThenIncludeWithVariousNavigationCollectionTypes()
    {
        var spec = new Specification<Foo>();
        spec.Query
            .Include(x => x.OuterNavigation)
                .ThenInclude(x => x.InnerNavigation)
                .ThenInclude(x => x.InnerNavigation2)
            .Include(x => x.OuterNavigation)
                .ThenInclude(x => x.ListNavigation)
                .ThenInclude(x => x.InnerNavigation2)
            .Include(x => x.OuterNavigation)
                .ThenInclude(x => x.IEnumerableNavigation)
                .ThenInclude(x => x.InnerNavigation2)
            .Include(x => x.OuterNavigation)
                .ThenInclude(x => x.IReadOnlyCollectionNavigation)
                .ThenInclude(x => x.InnerNavigation2)
            .Include(x => x.OuterNavigation)
                .ThenInclude(x => x.IReadOnlyListNavigation)
                .ThenInclude(x => x.InnerNavigation2)
            .Include(x => x.ListNavigation)
                .ThenInclude(x => x.ListNavigation2)
            .Include(x => x.IEnumerableNavigation)
                .ThenInclude(x => x.ListNavigation2)
            .Include(x => x.IReadOnlyCollectionNavigation)
                .ThenInclude(x => x.ListNavigation2)
            .Include(x => x.IReadOnlyListNavigation)
                .ThenInclude(x => x.ListNavigation2);

        var actual = _evaluator
            .GetQuery(DbContext.Foos, spec)
            .ToQueryString();

        var expected = DbContext.Foos
            .Include(x => x.OuterNavigation)
                .ThenInclude(x => x.InnerNavigation)
                .ThenInclude(x => x.InnerNavigation2)
            .Include(x => x.OuterNavigation)
                .ThenInclude(x => x.ListNavigation)
                .ThenInclude(x => x.InnerNavigation2)
            .Include(x => x.OuterNavigation)
                .ThenInclude(x => x.IEnumerableNavigation)
                .ThenInclude(x => x.InnerNavigation2)
            .Include(x => x.OuterNavigation)
                .ThenInclude(x => x.IReadOnlyCollectionNavigation)
                .ThenInclude(x => x.InnerNavigation2)
            .Include(x => x.OuterNavigation)
                .ThenInclude(x => x.IReadOnlyListNavigation)
                .ThenInclude(x => x.InnerNavigation2)
            .Include(x => x.ListNavigation)
                .ThenInclude(x => x.ListNavigation2)
            .Include(x => x.IEnumerableNavigation)
                .ThenInclude(x => x.ListNavigation2)
            .Include(x => x.IReadOnlyCollectionNavigation)
                .ThenInclude(x => x.ListNavigation2)
            .Include(x => x.IReadOnlyListNavigation)
                .ThenInclude(x => x.ListNavigation2)
            .ToQueryString();

        actual.Should().Be(expected);
    }
}
