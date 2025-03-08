namespace Tests.Evaluators;

[Collection("SharedCollection")]
public class SpecificationEvaluatorTests(TestFactory factory) : IntegrationTest(factory)
{
    private static readonly SpecificationEvaluator _evaluator = SpecificationEvaluator.Default;

    public record Customer(int Id, string FirstName, string LastName, List<string>? Emails = null);

    [Fact]
    public void ThrowsArgumentNullException_GivenNullSpec()
    {
        var sut = () => _evaluator.GetQuery(DbContext.Countries, (Specification<Country>)null!);

        sut.Should().Throw<ArgumentNullException>().WithParameterName("specification");
    }

    [Fact]
    public void ThrowsArgumentNullException_GivenNullSpecificationWithSelector()
    {
        var sut = () => _evaluator.GetQuery(DbContext.Countries, (Specification<Country, string>)null!);

        sut.Should().Throw<ArgumentNullException>().WithParameterName("specification");
    }

    [Fact]
    public void ThrowsSelectorNotFoundException_GivenNoSelector()
    {
        var spec = new Specification<Country, string>();

        var sut = () => _evaluator.GetQuery(DbContext.Countries, spec);

        sut.Should().Throw<SelectorNotFoundException>();
    }

    [Fact]
    public void GivenEmptySpec()
    {
        var spec = new Specification<Store>();

        var actual = _evaluator.GetQuery(DbContext.Stores, spec)
            .ToQueryString();

        var expected = DbContext.Stores
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void GivenFullQuery()
    {
        var id = 2;
        var name = "Store1";
        var storeTerm = "ab1";
        var companyTerm = "ab2";
        var streetTerm = "ab3";

        var spec = new Specification<Store>();
        spec.Query
            .Where(x => x.Id > id)
            .Where(x => x.Name == name)
            .Search(x => x.Name, $"%{storeTerm}%")
            .Search(x => x.Company.Name, $"%{companyTerm}%")
            .Search(x => x.Address.Street, $"%{streetTerm}%", 2)
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .OrderBy(x => x.Id)
                .ThenByDescending(x => x.Name)
            .Skip(1)
            .Take(10)
            .IgnoreQueryFilters();

        var actual = _evaluator.GetQuery(DbContext.Stores, spec)
            .ToQueryString();

        // The expression in the spec are applied in a predefined order.
        var expected = DbContext.Stores
            .Where(x => x.Id > id)
            .Where(x => x.Name == name)
            .Where(x => EF.Functions.Like(x.Name, $"%{storeTerm}%")
                     || EF.Functions.Like(x.Company.Name, $"%{companyTerm}%"))
            .Where(x => EF.Functions.Like(x.Address.Street, $"%{streetTerm}%"))
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .OrderBy(x => x.Id)
                .ThenByDescending(x => x.Name)
            .IgnoreQueryFilters()
            // Pagination always applied in the end
            .Skip(1)
            .Take(10)
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void GivenExpressionsInRandomOrder()
    {
        var id = 2;
        var name = "Store1";
        var storeTerm = "ab1";
        var companyTerm = "ab2";
        var streetTerm = "ab3";

        var spec = new Specification<Store>();
        spec.Query
            .Where(x => x.Id > id)
            .Search(x => x.Name, $"%{storeTerm}%")
            .Search(x => x.Company.Name, $"%{companyTerm}%")
            .Where(x => x.Name == name)
            .OrderBy(x => x.Id)
                .ThenByDescending(x => x.Name)
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .Search(x => x.Address.Street, $"%{streetTerm}%", 2)
            .Skip(1)
            .Take(10)
            .IgnoreQueryFilters();

        var actual = _evaluator.GetQuery(DbContext.Stores, spec)
            .ToQueryString();

        // The expression in the spec are applied in a predefined order.
        var expected = DbContext.Stores
            .Where(x => x.Id > id)
            .Where(x => x.Name == name)
            .Where(x => EF.Functions.Like(x.Name, $"%{storeTerm}%")
                     || EF.Functions.Like(x.Company.Name, $"%{companyTerm}%"))
            .Where(x => EF.Functions.Like(x.Address.Street, $"%{streetTerm}%"))
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .OrderBy(x => x.Id)
                .ThenByDescending(x => x.Name)
            .IgnoreQueryFilters()
            // Pagination always applied in the end
            .Skip(1)
            .Take(10)
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void GivenFullQueryWithSelect()
    {
        var id = 2;
        var name = "Store1";
        var storeTerm = "ab1";
        var companyTerm = "ab2";
        var streetTerm = "ab3";

        var spec = new Specification<Store, string?>();
        spec.Query
            .Where(x => x.Id > id)
            .Where(x => x.Name == name)
            .Search(x => x.Name, $"%{storeTerm}%")
            .Search(x => x.Company.Name, $"%{companyTerm}%")
            .Search(x => x.Address.Street, $"%{streetTerm}%", 2)
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .OrderBy(x => x.Id)
                .ThenByDescending(x => x.Name)
            .Skip(1)
            .Take(10)
            .IgnoreQueryFilters();
        spec.Query.Select(x => x.Name);

        var actual = _evaluator.GetQuery(DbContext.Stores, spec)
            .ToQueryString();

        // The expression in the spec are applied in a predefined order.
        var expected = DbContext.Stores
            .Where(x => x.Id > id)
            .Where(x => x.Name == name)
            .Where(x => EF.Functions.Like(x.Name, $"%{storeTerm}%")
                    || EF.Functions.Like(x.Company.Name, $"%{companyTerm}%"))
            .Where(x => EF.Functions.Like(x.Address.Street, $"%{streetTerm}%"))
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Id > 10))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .OrderBy(x => x.Id)
                .ThenByDescending(x => x.Name)
            .IgnoreQueryFilters()
            .Select(x => x.Name)
            // Pagination always applied in the end
            .Skip(1)
            .Take(10)
            .ToQueryString();

        actual.Should().Be(expected);
    }

    // TODO: Fix SelectMany query  [fatii, 11/02/2025]
    //[Fact]
    //public void GivenFullQueryWithSelectMany()
    //{
    //    var id = 2;
    //    var name = "Store1";
    //    var storeTerm = "ab1";
    //    var companyTerm = "ab2";
    //    var streetTerm = "ab3";

    //    var spec = new Specification<Store, string?>();
    //    spec.Query
    //        .Where(x => x.Id > id)
    //        .Where(x => x.Name == name)
    //        .Search(x => x.Name, $"%{storeTerm}%")
    //        .Search(x => x.Company.Name, $"%{companyTerm}%")
    //        .Search(x => x.Address.Street, $"%{streetTerm}%", 2)
    //        .Include(nameof(Address))
    //        .Include(x => x.Products.Where(x => x.Id > 10))
    //            .ThenInclude(x => x.Images)
    //        .Include(x => x.Company)
    //            .ThenInclude(x => x.Country)
    //        .OrderBy(x => x.Id)
    //            .ThenByDescending(x => x.Name)
    //        .Skip(1)
    //        .Take(10)
    //        .IgnoreQueryFilters();
    //    spec.Query.SelectMany(x => x.Products.Select(x => x.Name));

    //    var actual = _evaluator.GetQuery(DbContext.Stores, spec)
    //        .ToQueryString();

    //    // The expression in the spec are applied in a predefined order.
    //    var expected = DbContext.Stores
    //        .Where(x => x.Id > id)
    //        .Where(x => x.Name == name)
    //        .Where(x => EF.Functions.Like(x.Name, $"%{storeTerm}%")
    //                || EF.Functions.Like(x.Company.Name, $"%{companyTerm}%"))
    //        .Where(x => EF.Functions.Like(x.Address.Street, $"%{streetTerm}%"))
    //        .Include(nameof(Address))
    //        .Include(x => x.Products.Where(x => x.Id > 10))
    //            .ThenInclude(x => x.Images)
    //        .Include(x => x.Company)
    //            .ThenInclude(x => x.Country)
    //        .OrderBy(x => x.Id)
    //            .ThenByDescending(x => x.Name)
    //        .IgnoreQueryFilters()
    //        .SelectMany(x => x.Products.Select(x => x.Name))
    //        // Pagination always applied in the end
    //        .Skip(1)
    //        .Take(10)
    //        .ToQueryString();

    //    actual.Should().Be(expected);
    //}

    [Fact]
    public void GivenSpecAndIgnorePagination()
    {
        var id = 2;

        var spec = new Specification<Store>();
        spec.Query
            .Where(x => x.Id > id)
            .Skip(1)
            .Take(10);

        var actual = _evaluator.GetQuery(DbContext.Stores, spec, true)
            .ToQueryString();

        var expected = DbContext.Stores
            .Where(x => x.Id > id)
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void GivenSpecWithMultipleFlags()
    {
        var spec = new Specification<Country>();
        spec.Query
            .IgnoreQueryFilters()
            .AsNoTracking()
            .AsSplitQuery();

        var actual = _evaluator.GetQuery(DbContext.Countries, spec)
            .Expression
            .ToString();

        var expected = DbContext.Countries
            .AsNoTracking()
            .IgnoreQueryFilters()
            .AsSplitQuery()
            .Expression
            .ToString();

        actual.Should().Be(expected);
    }
}
