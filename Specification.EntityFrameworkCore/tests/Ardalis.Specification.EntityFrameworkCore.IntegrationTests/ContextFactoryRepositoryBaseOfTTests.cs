using System;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture;
using Ardalis.Specification.UnitTests.Fixture.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests
{
  public class ContextFactoryRepositoryBaseOfTTests : IClassFixture<SharedDatabaseFixture>
  {
    protected TestDbContext dbContext;
    protected IServiceProvider serviceProvider;
    protected ContextFactoryRepository<Company, TestDbContext> repository;

    public ContextFactoryRepositoryBaseOfTTests(SharedDatabaseFixture fixture)
    {
      dbContext = fixture.CreateContext();

      serviceProvider = new ServiceCollection()
        .AddDbContextFactory<TestDbContext>((builder => builder.UseSqlServer(fixture.Connection)),
          ServiceLifetime.Transient).BuildServiceProvider();

      var contextFactory = serviceProvider.GetService<IDbContextFactory<TestDbContext>>();
      repository = new ContextFactoryRepository<Company, TestDbContext>(contextFactory);
    }

    [Fact]
    public async Task Saves_new_entity()
    {
      var country = await dbContext.Countries.FirstOrDefaultAsync();

      var company = new Company();
      company.Name = "Test save new company name";
      company.CountryId = country.Id;

      await repository.AddAsync(company);
      Assert.NotEqual(0, company.Id);
    }

    [Fact]
    public async Task Updates_existing_entity()
    {
      var country = await dbContext.Countries.FirstOrDefaultAsync();

      var company = new Company { Name = "Test update existing company name", CountryId = country.Id };
      await repository.AddAsync(company);

      var existingCompany = await repository.GetByIdAsync(company.Id);
      existingCompany.Name = "Updated company name";
      await repository.UpdateAsync(existingCompany);

      var validationCompany = await dbContext.Companies.FirstOrDefaultAsync(x => x.Id == company.Id);
      Assert.Equal(validationCompany.Name, existingCompany.Name);
    }

    [Fact]
    public async Task Updates_existing_entity_across_context_instances()
    {
      var contextFactory = serviceProvider.GetService<IDbContextFactory<TestDbContext>>();
      var companyRetrievalRepository = new ContextFactoryRepository<Company, TestDbContext>(contextFactory);
      var companySaveRepository = new ContextFactoryRepository<Company, TestDbContext>(contextFactory);

      var country = await dbContext.Countries.FirstOrDefaultAsync();

      var company = new Company { Name = "Test update existing company name", CountryId = country.Id };
      await repository.AddAsync(company);

      var existingCompany = await companyRetrievalRepository.GetByIdAsync(company.Id);
      existingCompany.Name = "Updated company name";
      await companySaveRepository.UpdateAsync(existingCompany);

      var validationCompany = await dbContext.Companies.FirstOrDefaultAsync(x => x.Id == company.Id);
      Assert.Equal(validationCompany.Name, existingCompany.Name);
    }

    [Fact]
    public async Task Updates_graph()
    {
      var country = await dbContext.Countries.FirstOrDefaultAsync();

      var company = new Company { Name = "Test update graph", CountryId = country.Id };
      var store = new Store { Name = "Store Number 1" };
      company.Stores.Add(store);

      await repository.AddAsync(company);

      var spec = new GetCompanyWithStoresSpec(company.Id);
      var existingCompany = await repository.FirstOrDefaultAsync(spec);
      existingCompany.Name = "Updated company name";
      var existingStore = existingCompany.Stores.FirstOrDefault();
      existingStore.Name = "Updated Store Name";

      await repository.UpdateAsync(existingCompany);

      var validationCompany = await dbContext.Companies.FirstOrDefaultAsync(x => x.Id == company.Id);
      Assert.Equal(validationCompany.Name, existingCompany.Name);

      var validationStore = await dbContext.Stores.FirstOrDefaultAsync(x => x.CompanyId == company.Id);
      Assert.Equal(validationStore.Name, existingStore.Name);
    }

    [Fact]
    public async Task Updates_graph_across_context_instances()
    {
      var contextFactory = serviceProvider.GetService<IDbContextFactory<TestDbContext>>();
      var companyRetrievalRepository = new ContextFactoryRepository<Company, TestDbContext>(contextFactory);
      var companySaveRepository = new ContextFactoryRepository<Company, TestDbContext>(contextFactory);

      var country = await dbContext.Countries.FirstOrDefaultAsync();

      var company = new Company { Name = "Test update graph", CountryId = country.Id };
      var store = new Store { Name = "Store Number 1" };
      company.Stores.Add(store);

      await repository.AddAsync(company);

      var spec = new GetCompanyWithStoresSpec(company.Id);
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
      var country = await dbContext.Countries.FirstOrDefaultAsync();

      var company = new Company { Name = "Test update graph", CountryId = country.Id };
      await repository.AddAsync(company);

      var companyId = company.Id;
      await repository.DeleteAsync(company);

      var validationCompany = await repository.GetByIdAsync(companyId);
      Assert.Null(validationCompany);
    }
  }
}
