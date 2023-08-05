using Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture;
using Ardalis.Specification.UnitTests.Fixture.Entities;
using Ardalis.Specification.UnitTests.Fixture.Specs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests;

public class ContextFactoryRepositoryBaseOfTTests : IClassFixture<DatabaseFixture>
{
    private readonly DbContextOptions<TestDbContext> _dbContextOptions;
    private readonly ContextFactoryRepository<Company, TestDbContext> _repository;
    private readonly IServiceProvider _serviceProvider;
    private readonly Random _random = new Random();

    public ContextFactoryRepositoryBaseOfTTests(DatabaseFixture fixture)
    {
        _dbContextOptions = fixture.DbContextOptions;

        _serviceProvider = new ServiceCollection()
          .AddDbContextFactory<TestDbContext>((builder => builder.UseSqlServer(fixture.ConnectionString)), ServiceLifetime.Transient)
          .BuildServiceProvider();

        var contextFactory = _serviceProvider.GetService<IDbContextFactory<TestDbContext>>();
        _repository = new ContextFactoryRepository<Company, TestDbContext>(contextFactory);
    }

    [Fact]
    public async Task Saves_new_entity()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var country = await dbContext.Countries.FirstOrDefaultAsync();

        var company = new Company
        {
            Id = _random.Next(1000, 9999),
            Name = "Test save new company name",
            CountryId = country.Id
        };

        await _repository.AddAsync(company);
        Assert.NotEqual(0, company.Id);
    }

    [Fact]
    public async Task Updates_existing_entity()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var country = await dbContext.Countries.FirstOrDefaultAsync();

        var company = new Company
        {
            Id = _random.Next(1000, 9999),
            Name = "Test update existing company name",
            CountryId = country.Id
        };
        await _repository.AddAsync(company);

        var existingCompany = await _repository.GetByIdAsync(company.Id);
        existingCompany.Name = "Updated company name";
        await _repository.UpdateAsync(existingCompany);

        var validationCompany = await dbContext.Companies.FirstOrDefaultAsync(x => x.Id == company.Id);
        Assert.Equal(validationCompany.Name, existingCompany.Name);
    }

    [Fact]
    public async Task Updates_existing_entity_across_context_instances()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var country = await dbContext.Countries.FirstOrDefaultAsync();

        var contextFactory = _serviceProvider.GetService<IDbContextFactory<TestDbContext>>();
        var companyRetrievalRepository = new ContextFactoryRepository<Company, TestDbContext>(contextFactory);
        var companySaveRepository = new ContextFactoryRepository<Company, TestDbContext>(contextFactory);

        var company = new Company
        {
            Id = _random.Next(1000, 9999),
            Name = "Test update existing company name",
            CountryId = country.Id
        };
        await _repository.AddAsync(company);

        var existingCompany = await companyRetrievalRepository.GetByIdAsync(company.Id);
        existingCompany.Name = "Updated company name";
        await companySaveRepository.UpdateAsync(existingCompany);

        var validationCompany = await dbContext.Companies.FirstOrDefaultAsync(x => x.Id == company.Id);
        Assert.Equal(validationCompany.Name, existingCompany.Name);
    }

    [Fact]
    public async Task Updates_graph()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var country = await dbContext.Countries.FirstOrDefaultAsync();

        var company = new Company
        {
            Id = _random.Next(1000, 9999),
            Name = "Test update graph",
            CountryId = country.Id
        };
        var store = new Store
        {
            Id = _random.Next(1000, 9999),
            Name = "Store Number 1"
        };
        company.Stores.Add(store);

        await _repository.AddAsync(company);

        var spec = new CompanyByIdIncludeStoresSpec(company.Id);
        var existingCompany = await _repository.FirstOrDefaultAsync(spec);
        existingCompany.Name = "Updated company name";
        var existingStore = existingCompany.Stores.FirstOrDefault();
        existingStore.Name = "Updated Store Name";

        await _repository.UpdateAsync(existingCompany);

        var validationCompany = await dbContext.Companies.FirstOrDefaultAsync(x => x.Id == company.Id);
        Assert.Equal(validationCompany.Name, existingCompany.Name);

        var validationStore = await dbContext.Stores.FirstOrDefaultAsync(x => x.CompanyId == company.Id);
        Assert.Equal(validationStore.Name, existingStore.Name);
    }

    [Fact]
    public async Task Updates_graph_across_context_instances()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var country = await dbContext.Countries.FirstOrDefaultAsync();

        var contextFactory = _serviceProvider.GetService<IDbContextFactory<TestDbContext>>();
        var companyRetrievalRepository = new ContextFactoryRepository<Company, TestDbContext>(contextFactory);
        var companySaveRepository = new ContextFactoryRepository<Company, TestDbContext>(contextFactory);

        var company = new Company
        {
            Id = _random.Next(1000, 9999),
            Name = "Test update graph",
            CountryId = country.Id
        };
        var store = new Store
        {
            Id = _random.Next(1000, 9999),
            Name = "Store Number 1"
        };
        company.Stores.Add(store);

        await _repository.AddAsync(company);

        var spec = new CompanyByIdIncludeStoresSpec(company.Id);
        var existingCompany = await companyRetrievalRepository.FirstOrDefaultAsync(spec);
        existingCompany.Name = "Updated company name";
        var existingStore = existingCompany.Stores.FirstOrDefault();
        existingStore.Name = "Updated Store Name";

        await companySaveRepository.UpdateAsync(existingCompany);

        var validationCompany = await dbContext.Companies.FirstOrDefaultAsync(x => x.Id == company.Id);
        Assert.Equal(validationCompany.Name, existingCompany.Name);

        var validationStore = await dbContext.Stores.FirstOrDefaultAsync(x => x.CompanyId == company.Id);
        Assert.Equal(validationStore.Name, existingStore.Name);
    }

    [Fact]
    public async Task Deletes_entity()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var country = await dbContext.Countries.FirstOrDefaultAsync();

        var company = new Company
        {
            Id = _random.Next(1000, 9999),
            Name = "Test update graph",
            CountryId = country.Id
        };
        await _repository.AddAsync(company);

        var companyId = company.Id;
        await _repository.DeleteAsync(company);

        var validationCompany = await _repository.GetByIdAsync(companyId);
        Assert.Null(validationCompany);
    }
}
