namespace Tests.Evaluators;

public class SearchMemoryEvaluatorTests
{
    private static readonly SearchMemoryEvaluator _evaluator = SearchMemoryEvaluator.Instance;

    public record Customer(int Id, string FirstName, string? LastName);

    [Fact]
    public void Filters_GivenSearchInSameGroup()
    {
        List<Customer> input =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "aaaa"),
            new(3, "aaaa", "axya"),
            //new(4, "aaaa", null) // TODO: Fix null cases. [fatii, 11/02/2025]
        ];

        List<Customer> expected =
        [
            new(1, "axxa", "axya"),
            new(3, "aaaa", "axya")
        ];

        var spec = new Specification<Customer>();
        spec.Query
            .Search(x => x.FirstName, "%xx%")
            .Search(x => x.LastName, "%xy%");

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
            //new(4, "aaaa", null)
        ];

        List<Customer> expected =
        [
            new(1, "axxa", "axya")
        ];

        var spec = new Specification<Customer>();
        spec.Query
            .Search(x => x.FirstName, "%xx%", 1)
            .Search(x => x.LastName, "%xy%", 2);

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
            //new(4, "aaaa", null),
            //new(5, "axxa", null)
        ];

        List<Customer> expected =
        [
            new(1, "axxa", "axya"),
            new(3, "axxa", "axza"),
        ];

        var spec = new Specification<Customer>();
        spec.Query
            .Search(x => x.FirstName, "%xx%", 1)
            .Search(x => x.LastName, "%xy%", 2)
            .Search(x => x.LastName, "%xz%", 2);

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

        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id > 0);

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

        var spec = new Specification<Customer>();

        var actual = _evaluator.Evaluate(input, spec);

        actual.Should().Equal(expected);
    }
}
