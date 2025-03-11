namespace Tests.Evaluators;

public class InMemorySpecificationEvaluatorTests
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
    public void Evaluate_ThrowsConcurrentSelectorsException_GivenSelectAndSelectMany()
    {
        var spec = new Specification<CustomerWithMails, string>();
        spec.Query.Select(x => x.FirstName);
        spec.Query.SelectMany(x => x.Emails);

        var sut = () => _evaluator.Evaluate([], spec);

        sut.Should().Throw<ConcurrentSelectorsException>();
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
            .Take(1)
            .Select(x => x.FirstName);

        var actual = _evaluator.Evaluate(input, spec).ToList();
        var actualFromSpec = spec.Evaluate(input).ToList();

        actual.Should().Equal(actualFromSpec);
        actual.Should().Equal(expected);
    }

    [Fact]
    public void Evaluate_Filters_GivenSpecWithSelectMany()
    {
        List<CustomerWithMails> input =
        [
            new(1, "axxa", "axya", []),
            new(2, "aaaa", "axya", []),
            new(3, "aaaa", "axya", ["zzz", "www"]),
            new(4, "aaaa", "axya", ["yyy"])
        ];

        // TODO: This is a known flaw. Pagination should be applied after SelectMany [Fati, 11/03/2025]
        // It's a major breaking change, so it will be postponed to v10.
        List<string> expected = ["zzz", "www", "yyy"];

        var spec = new Specification<CustomerWithMails, string>();
        spec.Query
            .Where(x => x.Id > 1)
            .Search(x => x.LastName, "%xy%")
            .OrderBy(x => x.Id)
            .Skip(1)
            .Take(2)
            .SelectMany(x => x.Emails);

        var actual = _evaluator.Evaluate(input, spec).ToList();
        var actualFromSpec = spec.Evaluate(input).ToList();

        actual.Should().Equal(actualFromSpec);
        actual.Should().Equal(expected);
    }

    [Fact]
    public void Evaluate_Filters_GivenSpecAndPostProcessingAction()
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
            .PostProcessingAction(x => x.Where(c => c.Id == 3));

        var actual = _evaluator.Evaluate(input, spec).ToList();
        var actualFromSpec = spec.Evaluate(input).ToList();

        actual.Should().Equal(actualFromSpec);
        actual.Should().Equal(expected);
    }

    [Fact]
    public void Evaluate_Filters_GivenSpecWithSelectAndPostProcessingAction()
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
            .PostProcessingAction(x => x.Where(c => c == "vvvv"))
            .Select(x => x.FirstName);

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

#if NET8_0_OR_GREATER
    [Fact]
    public void Constructor_SetsProvidedEvaluators()
    {
        var evaluators = new List<IInMemoryEvaluator>
        {
            WhereEvaluator.Instance,
            OrderEvaluator.Instance,
            WhereEvaluator.Instance,
        };

        var evaluator = new InMemorySpecificationEvaluator(evaluators);

        var result = EvaluatorsOf(evaluator);
        result.Should().HaveSameCount(evaluators);
        result.Should().Equal(evaluators);
    }

    [Fact]
    public void DerivedSpecificationEvaluatorCanAlterDefaultEvaluator()
    {
        var evaluator = new SpecificationEvaluatorDerived();

        var result = EvaluatorsOf(evaluator);
        result.Should().HaveCount(6);
        result[0].Should().BeOfType<SearchMemoryEvaluator>();
        result[1].Should().BeOfType<WhereEvaluator>();
        result[2].Should().BeOfType<SearchMemoryEvaluator>();
        result[3].Should().BeOfType<OrderEvaluator>();
        result[4].Should().BeOfType<PaginationEvaluator>();
        result[5].Should().BeOfType<WhereEvaluator>();
    }

    private class SpecificationEvaluatorDerived : InMemorySpecificationEvaluator
    {
        public SpecificationEvaluatorDerived()
        {
            Evaluators.Add(WhereEvaluator.Instance);
            Evaluators.Insert(0, SearchMemoryEvaluator.Instance);
        }
    }

    [System.Runtime.CompilerServices.UnsafeAccessor(System.Runtime.CompilerServices.UnsafeAccessorKind.Field, Name = "<Evaluators>k__BackingField")]
    public static extern ref List<IInMemoryEvaluator> EvaluatorsOf(InMemorySpecificationEvaluator @this);
#endif
}
