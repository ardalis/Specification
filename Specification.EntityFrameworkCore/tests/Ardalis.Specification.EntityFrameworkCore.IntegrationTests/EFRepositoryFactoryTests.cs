using Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture;
using Ardalis.Specification.UnitTests.Fixture.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests;

public class EFRepositoryFactoryTests : IClassFixture<SharedDatabaseFixture>
{
  protected TestDbContext dbContext;
  protected IServiceProvider serviceProvider;
  protected IRepositoryFactory<IRepositoryBase<Company>> repositoryFactory;
  protected IDbContextFactory<TestDbContext> contextFactory;

  public EFRepositoryFactoryTests(SharedDatabaseFixture fixture)
  {
    dbContext = fixture.CreateContext();

    serviceProvider = new ServiceCollection()
      .AddDbContextFactory<TestDbContext>((builder => builder.UseSqlServer(fixture.Connection)),
        ServiceLifetime.Transient).BuildServiceProvider();

    contextFactory = serviceProvider.GetService<IDbContextFactory<TestDbContext>>();
    repositoryFactory =
      new EFRepositoryFactory<IRepositoryBase<Company>, Repository<Company>, TestDbContext>(contextFactory);
  }

  [Fact]
  public void CorrectlyInstantiatesRepository()
  {
    var mockContextFactory = new Mock<IDbContextFactory<SampleDbContext>>();
    mockContextFactory.Setup(x => x.CreateDbContext())
      .Returns(() => new SampleDbContext(new DbContextOptions<SampleDbContext>()));

    var repositoryFactory =
      new EFRepositoryFactory<IRepository<Customer>, MyRepository<Customer>, SampleDbContext>(mockContextFactory
        .Object);

    var repository = repositoryFactory.CreateRepository();
    Assert.IsType<MyRepository<Customer>>(repository);
  }

  [Fact]
  public async Task Saves_new_entity()
  {
    var repository = repositoryFactory.CreateRepository();
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
    var repository = repositoryFactory.CreateRepository();
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
  public async Task Updates_graph()
  {
    var repository = repositoryFactory.CreateRepository();
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

  public record Customer(int Id, string Name);

  public class SampleDbContext : DbContext
  {
    public DbSet<Customer> Customers { get; set; }

    public SampleDbContext(DbContextOptions<SampleDbContext> options)
        : base(options)
    {
    }
  }

  public interface IRepository<T> : IRepositoryBase<T> where T : class
  {
  }

  public class MyRepository<T> : RepositoryBase<T>, IRepository<T> where T : class
  {
    public MyRepository(SampleDbContext dbContext) : base(dbContext)
    {
    }
  }
}
