//namespace Tests.Extensions;

//[Collection("SharedCollection")]
//public class Extensions_ToPagedResult(TestFactory factory) : IntegrationTest(factory)
//{
//    public record CountryDto(int No, string? Name);

//    [Fact]
//    public async Task ReturnsPaginatedItems_GivenPagingFilter()
//    {
//        var expected = new List<CountryDto>
//        {
//            new(4, "b"),
//        };
//        await SeedRangeAsync<Country>(
//        [
//            new() { No = 9, Name = "a" },
//            new() { No = 9, Name = "c" },
//            new() { No = 1, Name = "b" },
//            new() { No = 2, Name = "b" },
//            new() { No = 3, Name = "b" },
//            new() { No = 4, Name = "b" },
//            new() { No = 9, Name = "d" },
//        ]);

//        var filter = new PagingFilter() { Page = 2, PageSize = 3 };

//        var result = await DbContext.Countries
//            .Where(x => x.Name == "b")
//            .OrderBy(x => x.No)
//            .Select(x => new CountryDto(x.No, x.Name))
//            .ToPagedResultAsync(filter);

//        result.Should().BeOfType<PagedResult<CountryDto>>();
//        result.Pagination.Page.Should().Be(filter.Page);
//        result.Pagination.PageSize.Should().Be(filter.PageSize);
//        result.Data.Should().Equal(expected);
//    }

//    [Fact]
//    public async Task ReturnsPaginatedItems_GivenPagingFilterAndPaginationSettings()
//    {
//        var expected = new List<CountryDto>
//        {
//            new(3, "b"),
//            new(4, "b"),
//        };
//        await SeedRangeAsync<Country>(
//        [
//            new() { No = 9, Name = "a" },
//            new() { No = 9, Name = "c" },
//            new() { No = 1, Name = "b" },
//            new() { No = 2, Name = "b" },
//            new() { No = 3, Name = "b" },
//            new() { No = 4, Name = "b" },
//            new() { No = 9, Name = "d" },
//        ]);

//        var paginationSettings = new PaginationSettings(2, 2);
//        var filter = new PagingFilter() { Page = 2, PageSize = 3 };

//        var result = await DbContext.Countries
//            .Where(x => x.Name == "b")
//            .OrderBy(x => x.No)
//            .Select(x => new CountryDto(x.No, x.Name))
//            .ToPagedResultAsync(filter, paginationSettings);

//        result.Should().BeOfType<PagedResult<CountryDto>>();
//        result.Pagination.Page.Should().Be(filter.Page);
//        result.Pagination.PageSize.Should().Be(paginationSettings.DefaultPageSize);
//        result.Data.Should().Equal(expected);
//    }

//    [Fact]
//    public async Task ReturnsPaginatedItemsWithAggregatedTakeSkip_GivenPagingFilterAndTakeSkip()
//    {
//        var expected = new List<CountryDto>
//        {
//            new(2, "b"),
//            new(3, "b"),
//        };
//        await SeedRangeAsync<Country>(
//        [
//            new() { No = 9, Name = "a" },
//            new() { No = 9, Name = "c" },
//            new() { No = 1, Name = "b" },
//            new() { No = 2, Name = "b" },
//            new() { No = 3, Name = "b" },
//            new() { No = 4, Name = "b" },
//            new() { No = 9, Name = "d" },
//        ]);

//        var filter = new PagingFilter() { Page = 2, PageSize = 3 };

//        var result = await DbContext.Countries
//            .Where(x => x.Name == "b")
//            .OrderBy(x => x.No)
//            .Select(x => new CountryDto(x.No, x.Name))
//            .Skip(1)
//            .Take(2)
//            .ToPagedResultAsync(filter);

//        result.Should().BeOfType<PagedResult<CountryDto>>();
//        result.Pagination.Page.Should().Be(PaginationSettings.Default.DefaultPage);
//        result.Pagination.PageSize.Should().Be(filter.PageSize);
//        result.Data.Should().Equal(expected);
//    }
//}
