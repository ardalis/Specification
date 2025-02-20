namespace Tests.Extensions;

[Collection("SharedCollection")]
public class Extensions_WithSpecification(TestFactory factory) : IntegrationTest(factory)
{
    public record CountryDto(string? Name);

    [Fact]
    public void QueriesMatch_GivenFullQuery()
    {
        var id = 1;
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

        var actual = DbContext.Stores
            .WithSpecification(spec)
            .ToQueryString()
            .Replace("__criteria_SearchTerm_", "__Format_");

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
    public void QueriesMatch_GivenFullQueryWithSelect()
    {
        var id = 1;
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

        var actual = DbContext.Stores
            .WithSpecification(spec)
            .ToQueryString()
            .Replace("__criteria_SearchTerm_", "__Format_");

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
    //public void QueriesMatch_GivenFullQueryWithSelectMany()
    //{
    //    var id = 1;
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

    //    var actual = DbContext.Stores
    //        .WithSpecification(spec)
    //        .ToQueryString()
    //        .Replace("__criteria_SearchTerm_", "__Format_");

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
    public void QueriesMatch_GivenCustomEvaluator()
    {
        var id = 1;

        var spec = new Specification<Store>();
        spec.Query
            .Where(x => x.Id > id)
            .Skip(1)
            .Take(10);

        var actual = DbContext.Stores
            .WithSpecification(spec, new MySpecificationEvaluator())
            .ToQueryString();

        var expected = DbContext.Stores
            .Skip(1)
            .Take(10)
            .ToQueryString();

        actual.Should().Be(expected);
    }

    [Fact]
    public void QueriesMatch_GivenSelectorAndCustomEvaluator()
    {
        var id = 1;

        var spec = new Specification<Store, CountryDto>();
        spec.Query
            .Where(x => x.Id > id)
            .Skip(1)
            .Take(10);
        spec.Query.Select(x => new CountryDto(x.Name));

        var actual = DbContext.Stores
            .WithSpecification(spec, new MySpecificationEvaluator())
            .ToQueryString();

        var expected = DbContext.Stores
            .Select(x => new CountryDto(x.Name))
            .Skip(1)
            .Take(10)
            .ToQueryString();

        actual.Should().Be(expected);
    }

    // It will ignore all where expressions.
    public class MySpecificationEvaluator : SpecificationEvaluator
    {
        public MySpecificationEvaluator()
        {
            Evaluators.Remove(WhereEvaluator.Instance);
        }
    }
}
