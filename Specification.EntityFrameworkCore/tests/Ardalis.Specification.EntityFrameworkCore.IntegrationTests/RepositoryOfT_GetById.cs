using Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture;
using Ardalis.Specification.UnitTests.Fixture.Entities;
using Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests;

[Collection("ReadCollection")]
public class RepositoryOfT_GetById : RepositoryOfT_GetById_TestKit
{
    public RepositoryOfT_GetById(DatabaseFixture fixture) : base(fixture, SpecificationEvaluator.Default)
    {
    }
}

[Collection("ReadCollection")]
public class RepositoryOfT_GetById_Cached : RepositoryOfT_GetById_TestKit
{
    public RepositoryOfT_GetById_Cached(DatabaseFixture fixture) : base(fixture, SpecificationEvaluator.Cached)
    {
    }
}

public abstract class RepositoryOfT_GetById_TestKit
{
    private readonly DbContextOptions<TestDbContext> _dbContextOptions;
    private readonly ISpecificationEvaluator _specificationEvaluator;

    protected RepositoryOfT_GetById_TestKit(DatabaseFixture fixture, ISpecificationEvaluator specificationEvaluator)
    {
        _dbContextOptions = fixture.DbContextOptions;
        _specificationEvaluator = specificationEvaluator;
    }

    [Fact]
    public virtual async Task ReturnsStore_GivenId()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var repo = new Repository<Store>(dbContext, _specificationEvaluator);

        var result = await repo.GetByIdAsync(StoreSeed.VALID_STORE_ID);

        result.Should().NotBeNull();
        result!.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
    }

    [Fact]
    public virtual async Task ReturnsStore_GivenGenericId()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var repo = new Repository<Store>(dbContext, _specificationEvaluator);

        var result = await repo.GetByIdAsync<int>(StoreSeed.VALID_STORE_ID);

        result.Should().NotBeNull();
        result!.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
    }
}
