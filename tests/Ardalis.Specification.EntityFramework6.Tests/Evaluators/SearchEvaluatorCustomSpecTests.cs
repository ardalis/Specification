using System.Data.Entity;

namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class SearchEvaluatorCustomSpecTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly SearchEvaluator _evaluator = SearchEvaluator.Instance;

    [Fact]
    public void QueriesMatch_GivenNoSearch()
    {
        var spec = Substitute.For<ISpecification<Store>>();
        spec.WhereExpressions.Returns(
        [
            new WhereExpressionInfo<Store>(x => x.Id > 0)
        ]);

        var actual = _evaluator.GetQuery(DbContext.Stores, spec);
        var actualSql = GetQueryString(DbContext, actual);

        var expected = DbContext.Companies.AsQueryable();
        var expectedSql = GetQueryString(DbContext, expected);

        actualSql.Should().Be(expectedSql);
    }

    [Fact]
    public void QueriesMatch_GivenSingleSearch()
    {
        var storeTerm = "ab1";
        var searchTerm = $"%{storeTerm}%";

        var spec = Substitute.For<ISpecification<Store>>();
        spec.WhereExpressions.Returns(
        [
            new WhereExpressionInfo<Store>(x => x.Id > 0)
        ]);
        spec.SearchCriterias.Returns(
        [
            new SearchExpressionInfo<Store>(x => x.Name, searchTerm)
        ]);

        var actual = _evaluator.GetQuery(DbContext.Stores, spec);
        var actualSql = GetQueryString(DbContext, actual);

        var expected = DbContext.Stores
            .Where(x => DbFunctions.Like(x.Name, searchTerm));
        var expectedSql = GetQueryString(DbContext, expected);

        actualSql.Should().Be(expectedSql);
    }

    [Fact]
    public void QueriesMatch_GivenMultipleSearch()
    {
        var storeTerm = "%ab1%";
        var companyTerm = "%ab2%";
        var countryTerm = "%ab3%";
        var streetTerm = "%ab4%";

        var spec = Substitute.For<ISpecification<Store>>();
        spec.WhereExpressions.Returns(
        [
            new WhereExpressionInfo<Store>(x => x.Id > 0)
        ]);
        spec.SearchCriterias.Returns(
        [
            new SearchExpressionInfo<Store>(x => x.Name, storeTerm),
            new SearchExpressionInfo<Store>(x => x.Company.Name, companyTerm),
            new SearchExpressionInfo<Store>(x => x.Address.Street, streetTerm, 2),
            new SearchExpressionInfo<Store>(x => x.Company.Country.Name, countryTerm, 3)
        ]);

        var actual = _evaluator.GetQuery(DbContext.Stores, spec);
        var actualSql = GetQueryString(DbContext, actual);

        var expected = DbContext.Stores
            .Where(x => DbFunctions.Like(x.Name, storeTerm)
                    || DbFunctions.Like(x.Company.Name, companyTerm))
            .Where(x => DbFunctions.Like(x.Address.Street, streetTerm))
            .Where(x => DbFunctions.Like(x.Company.Country.Name, countryTerm));
        var expectedSql = GetQueryString(DbContext, expected);

        actualSql.Should().Be(expectedSql);
    }
}
