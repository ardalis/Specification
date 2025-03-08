namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class SearchEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly SearchEvaluator _evaluator = SearchEvaluator.Instance;

    [Fact]
    public void QueriesMatch_GivenNoSearch()
    {
        var spec = new Specification<Store>();
        spec.Query
            .Where(x => x.Id > 0);

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

        var spec = new Specification<Store>();
        spec.Query
            .Where(x => x.Id > 0)
            .Search(x => x.Name, $"%{storeTerm}%");

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

        var spec = new Specification<Store>();
        spec.Query
            .Where(x => x.Id > 0)
            .Search(x => x.Name, $"%{storeTerm}%")
            .Search(x => x.Company.Name, $"%{companyTerm}%")
            .Search(x => x.Company.Country.Name, $"%{countryTerm}%", 3)
            .Search(x => x.Address.Street, $"%{streetTerm}%", 2);

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
}
