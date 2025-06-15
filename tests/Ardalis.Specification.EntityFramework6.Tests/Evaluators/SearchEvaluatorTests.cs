using System.Data.Entity;
using Tests.FixtureNew;

namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class SearchEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly SearchEvaluator _evaluator = SearchEvaluator.Instance;

    [Fact]
    public void QueriesMatch_GivenNoSearch()
    {
        var spec = new Specification<Company>();
        spec.Query
            .Where(x => x.Id > 0);

        var actual = _evaluator.GetQuery(DbContext.Companies, spec);
        var actualSql = GetQueryString(DbContext, actual);

        var expected = DbContext.Companies.AsQueryable();
        var expectedSql = GetQueryString(DbContext, expected);

        actualSql.Should().Be(expectedSql);
    }

    //// TODO: Not producing the same query, it's not parameterized and no null check [Fati Iseni, 15/06/2025]
    //[Fact]
    //public void QueriesMatch_GivenSingleSearch()
    //{
    //    var storeTerm = "ab1";

    //    var spec = new Specification<Company>();
    //    spec.Query
    //        .Where(x => x.Id > 0)
    //        .Search(x => x.Name, $"%{storeTerm}%");

    //    var actual = _evaluator.GetQuery(DbContext.Companies, spec);
    //    var actualSql = GetQueryString(DbContext, actual);

    //    var expected = DbContext.Companies
    //        .Where(x => DbFunctions.Like(x.Name, "%" + storeTerm + "%"));
    //    var expectedSql = GetQueryString(DbContext, expected);

    //    actualSql.Should().Be(expectedSql);
    //}

    //// TODO: Not producing the same query, it's not parameterized and no null check [Fati Iseni, 15/06/2025]
    //[Fact]
    //public void QueriesMatch_GivenMultipleSearch()
    //{
    //    var storeTerm = "ab1";
    //    var companyTerm = "ab2";
    //    var countryTerm = "ab3";
    //    var streetTerm = "ab4";

    //    var spec = new Specification<Store>();
    //    spec.Query
    //        .Where(x => x.Id > 0)
    //        .Search(x => x.Name, $"%{storeTerm}%")
    //        .Search(x => x.Company.Name, $"%{companyTerm}%")
    //        .Search(x => x.Company.Country.Name, $"%{countryTerm}%", 3)
    //        .Search(x => x.Address.Street, $"%{streetTerm}%", 2);

    //    var actual = _evaluator.GetQuery(DbContext.Stores, spec);
    //    var actualSql = GetQueryString(DbContext, actual);

    //    var expected = DbContext.Stores
    //        .Where(x => DbFunctions.Like(x.Name, "%" + storeTerm + "%")
    //                || DbFunctions.Like(x.Company.Name, "%" + storeTerm + "%"))
    //        .Where(x => DbFunctions.Like(x.Address.Street, "%" + storeTerm + "%"))
    //        .Where(x => DbFunctions.Like(x.Company.Country.Name, "%" + storeTerm + "%"));
    //    var expectedSql = GetQueryString(DbContext, expected);

    //    actualSql.Should().Be(expectedSql);
    //}
}
