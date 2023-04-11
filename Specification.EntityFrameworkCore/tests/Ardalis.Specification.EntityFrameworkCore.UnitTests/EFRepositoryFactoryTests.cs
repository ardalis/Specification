using Ardalis.SampleApp.Core.Entities.CustomerAggregate;
using Ardalis.SampleApp.Core.Interfaces;
using Ardalis.SampleApp.Infrastructure.Data;
using Ardalis.SampleApp.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Ardalis.Specification.EntityFrameworkCore.UnitTests;

public class EFRepositoryFactoryTests
{
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
}
