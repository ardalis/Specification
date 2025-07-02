using System.Data.Entity;

namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class IncludeEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly IncludeEvaluator _evaluator = IncludeEvaluator.Instance;

    [Fact]
    public void QueriesMatch_GivenNoIncludeExpression()
    {
        var spec = new Specification<Company>();

        var actual = _evaluator.GetQuery(DbContext.Companies, spec);
        var actualSql = GetQueryString(DbContext, actual);

        var expected = DbContext.Companies.AsQueryable();
        var expectedSql = GetQueryString(DbContext, expected);

        actualSql.Should().Be(expectedSql);
    }

    [Fact]
    public void QueriesMatch_GivenNoIncludeExpression_WithAutoOneToOne()
    {
        var spec = new Specification<Store>();

        var actual = _evaluator.GetQuery(DbContext.Stores, spec);
        var actualSql = GetQueryString(DbContext, actual);

        var expected = DbContext.Stores.AsQueryable();
        var expectedSql = GetQueryString(DbContext, expected);

        actualSql.Should().Be(expectedSql);
    }

    [Fact]
    public void QueriesMatch_GivenSingleIncludeExpression_WithReferenceNavigation()
    {
        var spec = new Specification<Company>();
        spec.Query
            .Include(x => x.Country);

        var actual = _evaluator.GetQuery(DbContext.Companies, spec);
        var actualSql = GetQueryString(DbContext, actual);

        var expected = DbContext.Companies
            .Include(x => x.Country);
        var expectedSql = GetQueryString(DbContext, expected);

        actualSql.Should().Be(expectedSql);
    }

    [Fact]
    public void QueriesMatch_GivenSingleIncludeExpression_WithCollectionNavigation()
    {
        var spec = new Specification<Company>();
        spec.Query
            .Include(x => x.Stores);

        var actual = _evaluator.GetQuery(DbContext.Companies, spec);
        var actualSql = GetQueryString(DbContext, actual);

        var expected = DbContext.Companies
            .Include(x => x.Stores);
        var expectedSql = GetQueryString(DbContext, expected);

        actualSql.Should().Be(expectedSql);
    }

    [Fact]
    public void QueriesMatch_GivenMultipleIncludeExpression()
    {
        var spec = new Specification<Company>();
        spec.Query
            .Include(x => x.Country)
            .Include(x => x.Stores);

        var actual = _evaluator.GetQuery(DbContext.Companies, spec);
        var actualSql = GetQueryString(DbContext, actual);

        var expected = DbContext.Companies
            .Include(x => x.Country)
            .Include(x => x.Stores);
        var expectedSql = GetQueryString(DbContext, expected);

        actualSql.Should().Be(expectedSql);
    }


    [Fact(Skip = "EF6 include evaluator fails for multiple include chains [Fati Iseni, 15/06/2025]")]
    public void QueriesMatch_GivenThenIncludeExpression()
    {
        var spec = new Specification<Store>();
        spec.Query
            .Include(x => x.Products)
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country);

        var actual = _evaluator.GetQuery(DbContext.Stores, spec);
        var actualSql = GetQueryString(DbContext, actual);

        // EF6 doesn't support ThenInclude, it uses string-based includes
        var expected = DbContext.Stores
            .Include($"{nameof(Store.Products)}.{nameof(Product.Images)}")
            .Include($"{nameof(Store.Company)}.{nameof(Company.Country)}");
        var expectedSql = GetQueryString(DbContext, expected);

        actualSql.Should().Be(expectedSql);
    }
}
