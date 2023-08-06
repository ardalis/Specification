using Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture;
using Ardalis.Specification.UnitTests.Fixture.Entities;
using Ardalis.Specification.UnitTests.Fixture.Specs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests;

public class EFRepositoryFactoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DbContextOptions<TestDbContext> _dbContextOptions;
    private readonly IRepositoryFactory<IRepositoryBase<Company>> _repositoryFactory;
    private readonly IDbContextFactory<TestDbContext> _contextFactory;
    private readonly IServiceProvider _serviceProvider;
    private readonly Random _random = new Random();

    public EFRepositoryFactoryTests(DatabaseFixture fixture)
    {
        _dbContextOptions = fixture.DbContextOptions;

        _serviceProvider = new ServiceCollection()
          .AddDbContextFactory<TestDbContext>((builder => builder.UseSqlServer(fixture.ConnectionString)), ServiceLifetime.Transient)
          .BuildServiceProvider();

        _contextFactory = _serviceProvider.GetService<IDbContextFactory<TestDbContext>>();
        _repositoryFactory = new EFRepositoryFactory<IRepositoryBase<Company>, Repository<Company>, TestDbContext>(_contextFactory);
    }

    [Fact]
    public void CorrectlyInstantiatesRepository()
    {
        var mockContextFactory = new Mock<IDbContextFactory<SampleDbContext>>();
        mockContextFactory.Setup(x => x.CreateDbContext())
          .Returns(() => new SampleDbContext(new DbContextOptions<SampleDbContext>()));

        var repositoryFactory = new EFRepositoryFactory<IRepository<Customer>, MyRepository<Customer>, SampleDbContext>(mockContextFactory.Object);

        var repository = repositoryFactory.CreateRepository();
        Assert.IsType<MyRepository<Customer>>(repository);
    }

    [Fact]
    public async Task Saves_new_entity()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var country = await dbContext.Countries.FirstOrDefaultAsync();
        var repository = _repositoryFactory.CreateRepository();

        var company = new Company
        {
            Id = _random.Next(1000, 9999),
            Name = "Test save new company name",
            CountryId = country.Id
        };

        await repository.AddAsync(company);
        Assert.NotEqual(0, company.Id);
    }

    [Fact]
    public async Task Updates_existing_entity()
    {
        using var dbContext = new TestDbContext(_dbContextOptions);
        var country = await dbContext.Countries.FirstOrDefaultAsync();
        var repository = _repositoryFactory.CreateRepository();

        var company = new Company
        {
            Id = _random.Next(1000, 9999),
            Name = "Test update existing company name",
            CountryId = country.Id
        };
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
        using var dbContext = new TestDbContext(_dbContextOptions);
        var country = await dbContext.Countries.FirstOrDefaultAsync();
        var repository = _repositoryFactory.CreateRepository();

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

        await repository.AddAsync(company);

        var spec = new CompanyByIdIncludeStoresSpec(company.Id);
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
