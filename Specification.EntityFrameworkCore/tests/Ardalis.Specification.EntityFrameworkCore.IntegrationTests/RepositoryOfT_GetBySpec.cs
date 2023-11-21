using Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture;
using Ardalis.Specification.UnitTests.Fixture.Entities;
using Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;
using Ardalis.Specification.UnitTests.Fixture.Specs;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests;

[Collection("ReadCollection")]
public class RepositoryOfT_GetBySpec : RepositoryOfT_GetBySpec_TestKit
{
    public RepositoryOfT_GetBySpec(DatabaseFixture fixture) : base(fixture, SpecificationEvaluator.Default)
    {
    }
}

[Collection("ReadCollection")]
public class RepositoryOfT_GetBySpec_Cached : RepositoryOfT_GetBySpec_TestKit
{
    public RepositoryOfT_GetBySpec_Cached(DatabaseFixture fixture) : base(fixture, SpecificationEvaluator.Cached)
    {
    }
}

public abstract class RepositoryOfT_GetBySpec_TestKit
{
    private readonly DbContextOptions<TestDbContext> _dbContextOptions;
    private readonly ISpecificationEvaluator _specificationEvaluator;

    protected RepositoryOfT_GetBySpec_TestKit(DatabaseFixture fixture, ISpecificationEvaluator specificationEvaluator)
    {
        _dbContextOptions = fixture.DbContextOptions;
        _specificationEvaluator = specificationEvaluator;
    }

    [Fact]
    public virtual async Task ReturnsStoreWithProducts_GivenStoreByIdIncludeProductsSpec()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var repo = new Repository<Store>(dbContext, _specificationEvaluator);

        var result = await repo.GetBySpecAsync(new StoreByIdIncludeProductsSpec(StoreSeed.VALID_STORE_ID));

        result.Should().NotBeNull();
        result!.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
        result.Products.Count.Should().BeGreaterThan(1);
    }

    [Fact]
    public virtual async Task ReturnsStoreWithAddress_GivenStoreByIdIncludeAddressSpec()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var repo = new Repository<Store>(dbContext, _specificationEvaluator);

        var result = await repo.GetBySpecAsync(new StoreByIdIncludeAddressSpec(StoreSeed.VALID_STORE_ID));

        result.Should().NotBeNull();
        result!.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
        result.Address?.Street.Should().Be(AddressSeed.VALID_STREET_FOR_STOREID1);
    }

    [Fact]
    public virtual async Task ReturnsStoreWithAddressAndProduct_GivenStoreByIdIncludeAddressAndProductsSpec()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var repo = new Repository<Store>(dbContext, _specificationEvaluator);

        var result = await repo.GetBySpecAsync(new StoreByIdIncludeAddressAndProductsSpec(StoreSeed.VALID_STORE_ID));

        result.Should().NotBeNull();
        result!.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
        result.Products.Count.Should().BeGreaterThan(1);
        result.Address?.Street.Should().Be(AddressSeed.VALID_STREET_FOR_STOREID1);
    }

    [Fact]
    public virtual async Task ReturnsStoreWithProducts_GivenStoreByIdIncludeProductsUsingStringSpec()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var repo = new Repository<Store>(dbContext, _specificationEvaluator);

        var result = await repo.GetBySpecAsync(new StoreByIdIncludeProductsUsingStringSpec(StoreSeed.VALID_STORE_ID));

        result.Should().NotBeNull();
        result!.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
        result.Products.Count.Should().BeGreaterThan(1);
    }

    [Fact]
    public virtual async Task ReturnsCompanyWithStoresAndAddress_GivenCompanyByIdIncludeStoresThenIncludeAddressSpec()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var repo = new Repository<Company>(dbContext, _specificationEvaluator);

        var result = await repo.GetBySpecAsync(new CompanyByIdIncludeStoresThenIncludeAddressSpec(CompanySeed.VALID_COMPANY_ID));

        result.Should().NotBeNull();
        result!.Name.Should().Be(CompanySeed.VALID_COMPANY_NAME);
        result.Stores.Count.Should().BeGreaterThan(49);
        result.Stores.Select(x => x.Address).Count().Should().BeGreaterThan(0);
    }

    [Fact]
    public virtual async Task ReturnsCompanyWithStoresAndProducts_GivenCompanyByIdIncludeStoresThenIncludeProductsSpec()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var repo = new Repository<Company>(dbContext, _specificationEvaluator);

        var result = await repo.GetBySpecAsync(new CompanyByIdIncludeStoresThenIncludeProductsSpec(CompanySeed.VALID_COMPANY_ID));

        result.Should().NotBeNull();
        result!.Name.Should().Be(CompanySeed.VALID_COMPANY_NAME);
        result.Stores.Count.Should().BeGreaterThan(49);
        result.Stores.Select(x => x.Products).Count().Should().BeGreaterThan(1);
    }

    [Fact]
    public virtual async Task ReturnsUntrackedCompany_GivenCompanyByIdAsUntrackedSpec()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var repo = new Repository<Company>(dbContext, _specificationEvaluator);

        dbContext.ChangeTracker.Clear();

        var result = await repo.GetBySpecAsync(new CompanyByIdAsUntrackedSpec(CompanySeed.VALID_COMPANY_ID));

        result.Should().NotBeNull();
        result?.Name.Should().Be(CompanySeed.VALID_COMPANY_NAME);
        dbContext.Entry(result!).State.Should().Be(Microsoft.EntityFrameworkCore.EntityState.Detached);
    }

    [Fact]
    public virtual async Task ReturnsStoreWithCompanyAndCountryAndStoresForCompany_GivenStoreByIdIncludeCompanyAndCountryAndStoresForCompanySpec()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var repo = new Repository<Store>(dbContext, _specificationEvaluator);

        var result = await repo.GetBySpecAsync(new StoreByIdIncludeCompanyAndCountryAndStoresForCompanySpec(StoreSeed.VALID_STORE_ID));

        result.Should().NotBeNull();
        result!.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
        result.Company.Should().NotBeNull();
        result.Company!.Country.Should().NotBeNull();
        result.Company!.Stores.Should().HaveCountGreaterOrEqualTo(2);
        result.Company?.Stores?.Should().Match(x => x.Any(z => z.Products.Count > 0));
    }
}
