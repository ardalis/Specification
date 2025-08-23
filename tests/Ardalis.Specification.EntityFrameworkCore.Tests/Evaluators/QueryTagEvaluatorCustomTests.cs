using Azure;

namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class QueryTagEvaluatorCustomTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly QueryTagEvaluator _evaluator = QueryTagEvaluator.Instance;

    [Fact]
    public void QueriesMatch_GivenTag()
    {
        var tag = "asd";

        var spec = Substitute.For<ISpecification<Country>>();
        spec.QueryTags.Returns([tag]);

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

        var spec = Substitute.For<ISpecification<Country>>();
        spec.QueryTags.Returns([tag1, tag2]);

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
        var spec = Substitute.For<ISpecification<Country>>();

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

        var spec = Substitute.For<ISpecification<Country>>();
        spec.QueryTags.Returns([tag]);

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

        var spec = Substitute.For<ISpecification<Country>>();
        spec.QueryTags.Returns([tag1, tag2]);

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

        var spec = Substitute.For<ISpecification<Country>>();
        spec.QueryTags.Returns([tag1, tag2, tag3]);

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
}
