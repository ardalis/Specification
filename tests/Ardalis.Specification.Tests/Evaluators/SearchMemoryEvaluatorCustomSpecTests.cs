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

        var spec = Substitute.For<ISpecification<Customer>>();
        spec.SearchCriterias.Returns(
        [
            new SearchExpressionInfo<Customer>(x => x.LastName, "%aa%")
        ]);

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

        var spec = Substitute.For<ISpecification<Customer>>();
        spec.SearchCriterias.Returns(
        [
            new SearchExpressionInfo<Customer>(x => x.FirstName, "%xx%"),
            new SearchExpressionInfo<Customer>(x => x.LastName, "%xy%")
        ]);

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

        var spec = Substitute.For<ISpecification<Customer>>();
        spec.SearchCriterias.Returns(
        [
            new SearchExpressionInfo<Customer>(x => x.FirstName, "%xx%", 1),
            new SearchExpressionInfo<Customer>(x => x.LastName, "%xy%", 2)
        ]);

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

        var spec = Substitute.For<ISpecification<Customer>>();
        spec.SearchCriterias.Returns(
        [
            new SearchExpressionInfo<Customer>(x => x.FirstName, "%xx%", 1),
            new SearchExpressionInfo<Customer>(x => x.LastName, "%xy%", 2),        
            new SearchExpressionInfo<Customer>(x => x.LastName, "%xz%", 2)
        ]);

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

        var spec = Substitute.For<ISpecification<Customer>>();
        spec.WhereExpressions.Returns(
        [
            new WhereExpressionInfo<Customer>(x => x.Id > 0)
        ]);

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

        var spec = Substitute.For<ISpecification<Customer>>();

        var actual = _evaluator.Evaluate(input, spec);

        actual.Should().Equal(expected);
    }
}
