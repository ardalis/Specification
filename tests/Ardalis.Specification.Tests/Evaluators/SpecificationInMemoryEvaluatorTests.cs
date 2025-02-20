namespace Tests.Evaluators;

public class SpecificationInMemoryEvaluatorTests
{
    private static readonly InMemorySpecificationEvaluator _evaluator = InMemorySpecificationEvaluator.Default;

    public record Customer(int Id, string FirstName, string LastName);
    public record CustomerWithMails(int Id, string FirstName, string LastName, List<string> Emails);

    [Fact]
    public void Evaluate_ThrowsSelectorNotFoundException_GivenNoSelector()
    {
        var spec = new Specification<Customer, string>();

        var sut = () => _evaluator.Evaluate([], spec);

        sut.Should().Throw<SelectorNotFoundException>();
    }

    [Fact]
    public void Evaluate_Filters_GivenSpec()
    {
        List<Customer> input =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "axya"),
            new(3, "aaaa", "axya"),
            new(4, "aaaa", "axya")
        ];

        List<Customer> expected =
        [
            new(3, "aaaa", "axya")
        ];

        var spec = new Specification<Customer>();
        spec.Query
            .Where(x => x.Id > 1)
            .Search(x => x.LastName, "%xy%")
            .OrderBy(x => x.Id)
            .Skip(1)
            .Take(1);

        var actual = _evaluator.Evaluate(input, spec).ToList();
        var actualFromSpec = spec.Evaluate(input).ToList();

        actual.Should().Equal(actualFromSpec);
        actual.Should().Equal(expected);
    }

    [Fact]
    public void Evaluate_Filters_GivenSpecWithSelect()
    {
        List<Customer> input =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "axya"),
            new(3, "vvvv", "axya"),
            new(4, "aaaa", "axya")
        ];

        List<string> expected = ["vvvv"];

        var spec = new Specification<Customer, string>();
        spec.Query
            .Where(x => x.Id > 1)
            .Search(x => x.LastName, "%xy%")
            .OrderBy(x => x.Id)
            .Skip(1)
            .Take(1);
        spec.Query.Select(x => x.FirstName);

        var actual = _evaluator.Evaluate(input, spec).ToList();
        var actualFromSpec = spec.Evaluate(input).ToList();

        actual.Should().Equal(actualFromSpec);
        actual.Should().Equal(expected);
    }

    [Fact]
    public void Evaluate_DoesNotFilter_GivenEmptySpec()
    {
        List<Customer> input =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "axya"),
            new(3, "aaaa", "axya"),
            new(4, "aaaa", "axya")
        ];

        List<Customer> expected =
        [
            new(1, "axxa", "axya"),
            new(2, "aaaa", "axya"),
            new(3, "aaaa", "axya"),
            new(4, "aaaa", "axya")
        ];

        var spec = new Specification<Customer>();

        var actual = _evaluator.Evaluate(input, spec).ToList();
        var actualFromSpec = spec.Evaluate(input).ToList();

        actual.Should().Equal(actualFromSpec);
        actual.Should().Equal(expected);
    }
}
