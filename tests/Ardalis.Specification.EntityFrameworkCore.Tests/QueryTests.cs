namespace Tests;

[Collection("SharedCollection")]
public class QueryTests(TestFactory factory) : IntegrationTest(factory)
{
    public record CountryDto(string? Name);
    public record ProductImageDto(string? ImageUrl);

    [Fact]
    public async Task ComplexQuery()
    {
        var validCountryName = "CountryX";
        var validStoreName = "StoreX";
        var validProductName = "ProductX";
        var validProductImageUrl = "ImageUrlX";
        var validCity = "CityX";
        var validCompanyName = "CompanyX";
        var validStreet = "StreetX";

        var validCityTerm = "ityX";
        var validCompanyTerm = "ompanyX";
        var validStreetTerm = "treetX";

        var validCountry = new Country { Name = validCountryName, IsDeleted = true };
        var validCompany = new Company { Name = validCompanyName, Country = validCountry };
        var validAddress = new Address { Street = validStreet };
        var invalidCompany = new Company { Name = "Fails", Country = validCountry };
        var invalidAddress = new Address { Street = "Fails" };

        List<Product> GetProducts() =>
        [
            new() { Name = validProductName, Images = [new() { ImageUrl = validProductImageUrl }] },
            new() { Name = validProductName, Images = null },
            new() { Name = "Fails", Images = [new() { ImageUrl = "Fails" }] },
        ];

        // The second item is expected based on descending city order.
        var stores = new List<Store>
        {
            new() // this passes, city-company same SEARCH group
            {
                Name = validStoreName,
                City = validCity,
                Company = invalidCompany,
                Address = validAddress with { },
                Products = GetProducts()
            },
            new() // this passes, city-company same SEARCH group
            {
                Name = validStoreName,
                City = "WWW",
                Company = validCompany,
                Address = validAddress with { },
                Products = GetProducts()
            },
            new() // fails, city and company
            {
                Name = validStoreName,
                City = "Fails",
                Company = invalidCompany,
                Address = validAddress with { },
                Products = GetProducts()
            },
            new() // fails, address
            {
                Name = validStoreName,
                City = validCity,
                Company = validCompany,
                Address = invalidAddress with { },
                Products = GetProducts()
            },
            new() // fails name
            {
                Name = "Fails",
                City = validCity,
                Company = validCompany,
                Address = validAddress with { },
                Products = GetProducts()
            },
            new() // this passes
            {
                Name = validStoreName,
                City = validCity,
                Company = validCompany,
                Address = validAddress with { },
                Products = GetProducts()
            },
        };

        await SeedRangeAsync(stores);

        var spec = new Specification<Store>();
        spec.Query
            .Where(x => x.Name == validStoreName)
            .Search(x => x.City, $"%{validCityTerm}%")
            .Search(x => x.Company.Name, $"%{validCompanyTerm}%")
            .Search(x => x.Address.Street, $"%{validStreetTerm}%", 2)
            .Include(nameof(Address))
            .Include(x => x.Products.Where(x => x.Name == validProductName))
                .ThenInclude(x => x.Images)
            .Include(x => x.Company)
                .ThenInclude(x => x.Country)
            .OrderBy(x => x.Id)
                .ThenByDescending(x => x.City)
            .Skip(1)
            .Take(1)
            .IgnoreQueryFilters();

        var result = await DbContext.Stores
            .WithSpecification(spec)
            .ToListAsync();

        result.Should().ContainSingle();
        result[0].Name.Should().Be(validStoreName);
        result[0].City.Should().Be("WWW");
        result[0].Address.Street.Should().Be(validStreet);
        result[0].Company.Name.Should().Be(validCompanyName);
        result[0].Company.Country.Name.Should().Be(validCountryName);
        result[0].Products.Should().HaveCount(2);
        result[0].Products[0].Name.Should().Be(validProductName);
        result[0].Products[0].Images.Should().HaveCount(1);
        result[0].Products[0].Images![0].ImageUrl.Should().Be(validProductImageUrl);
        result[0].Products[1].Name.Should().Be(validProductName);
        result[0].Products[1].Images.Should().BeEmpty();
    }

    [Fact]
    public async Task QueryWithSelect()
    {
        var expected = new List<CountryDto>
        {
            new("b"),
            new("b"),
            new("b"),
        };
        await SeedRangeAsync<Country>(
        [
            new() { Name = "a" },
            new() { Name = "c" },
            new() { Name = "b" },
            new() { Name = "b" },
            new() { Name = "b" },
            new() { Name = "d" },
        ]);

        var spec = new Specification<Country, CountryDto>();
        spec.Query
            .Where(x => x.Name == "b")
            .Select(x => new CountryDto(x.Name));

        var result = await DbContext.Countries
            .WithSpecification(spec)
            .ToListAsync();

        result.Should().HaveSameCount(expected);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task QueryWithSelectMany()
    {
        var expected = new List<ProductImageDto>
        {
            new("b"),
            new("b"),
            new("b"),
        };
        var store = new Store
        {
            Name = "Store1",
            Company = new Company
            {
                Name = "Company1",
                Country = new Country { Name = "b" }
            },
        };
        var products = new List<Product>()
        {
            new()
            {
                Store = store,
                Images =
                [
                    new() { ImageUrl = "a" },
                    new() { ImageUrl = null },
                    new() { ImageUrl = "b" },
                    new() { ImageUrl = "b" },
                    new() { ImageUrl = "b" },
                    new() { ImageUrl = "d" },
                ]
            },
            new()
            {
                Store = store,
                Images = null
            }
        };
        await SeedRangeAsync(products);

        var spec = new Specification<Product, ProductImageDto>();
        spec.Query
            .SelectMany(x => x.Images!
                .Where(x => x.ImageUrl == "b")
                .Select(x => new ProductImageDto(x.ImageUrl)));

        var result = await DbContext.Products
            .WithSpecification(spec)
            .ToListAsync();

        result.Should().HaveSameCount(expected);
        result.Should().BeEquivalentTo(expected);
    }
}
