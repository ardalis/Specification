using Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture;
using Ardalis.Specification.UnitTests.Fixture.Entities;
using Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;
using Ardalis.Specification.UnitTests.Fixture.Specs;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests;

[Collection("ReadCollection")]
public class RepositoryOfT_AnyAsync : RepositoryOfT_AnyAsync_TestKit
{
    public RepositoryOfT_AnyAsync(DatabaseFixture fixture) : base(fixture, SpecificationEvaluator.Default)
    {
    }
}

[Collection("ReadCollection")]
public class RepositoryOfT_AnyAsync_Cached : RepositoryOfT_AnyAsync_TestKit
{
    public RepositoryOfT_AnyAsync_Cached(DatabaseFixture fixture) : base(fixture, SpecificationEvaluator.Cached)
    {
    }
}

public abstract class RepositoryOfT_AnyAsync_TestKit
{
    private readonly DbContextOptions<TestDbContext> _dbContextOptions;
    private readonly ISpecificationEvaluator _specificationEvaluator;

    protected RepositoryOfT_AnyAsync_TestKit(DatabaseFixture fixture, ISpecificationEvaluator specificationEvaluator)
    {
        _dbContextOptions = fixture.DbContextOptions;
        _specificationEvaluator = specificationEvaluator;
    }

    [Fact]
    public virtual async Task ReturnsTrueOnStoresRecords_WithoutSpec()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var repo = new Repository<Store>(dbContext, _specificationEvaluator);

        var result = await repo.AnyAsync();

        result.Should().BeTrue();
    }

    [Fact]
    public virtual async Task ReturnsTrue_GivenStoreByIdSpecWithValidStore()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var repo = new Repository<Store>(dbContext, _specificationEvaluator);

        var result = await repo.AnyAsync(new StoreByIdSpec(StoreSeed.VALID_STORE_ID));

        result.Should().BeTrue();
    }

    [Fact]
    public virtual async Task ReturnsFalse_GivenStoreByIdSpecWithInvalidStore()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var repo = new Repository<Store>(dbContext, _specificationEvaluator);

        var result = await repo.AnyAsync(new StoreByIdSpec(0));

        result.Should().BeFalse();
    }
}
