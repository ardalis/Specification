using System.Runtime.InteropServices;

namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class SearchExtensionTests(TestFactory factory) : IntegrationTest(factory)
{
    [Fact]
    public void QueriesMatch_GivenSpecWithMultipleSearch()
    {
        var storeTerm = "ab1";
        var companyTerm = "ab2";

        var spec = new Specification<Store>();
        spec.Query
            .Search(x11 => x11.Name, $"%{storeTerm}%")
            .Search(x22 => x22.Company.Name, $"%{companyTerm}%");

        var list = spec.SearchCriterias as List<SearchExpressionInfo<Store>>;
        var span = CollectionsMarshal.AsSpan(list);

        var actual = DbContext.Stores
            .ApplyLikesAsOrGroup(span)
            .ToQueryString();

        var expected = DbContext.Stores
            .Where(x => EF.Functions.Like(x.Name, $"%{storeTerm}%")
                    || EF.Functions.Like(x.Company.Name, $"%{companyTerm}%"))
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenEmptySpec()
    {
        var spec = new Specification<Store>();

        var list = spec.SearchCriterias as List<SearchExpressionInfo<Store>>;
        var span = CollectionsMarshal.AsSpan(list);

        var actual = DbContext.Stores
            .ApplyLikesAsOrGroup(span)
            .ToQueryString();

        var expected = DbContext.Stores
            .ToQueryString();

        actual.Should().Be(expected);
    }
}
