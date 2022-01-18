using Ardalis.SampleApp.Core.Entities.CustomerAggregate;
using Ardalis.SampleApp.Infrastructure.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Ardalis.SampleApp.Infrastructure.DataAccess;

public class SampleDbContext : DbContext
{
  public DbSet<Customer> Customers { get; set; }

  public SampleDbContext(DbContextOptions<SampleDbContext> options)
      : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.ApplyConfiguration(new CustomerConfiguration());
    builder.ApplyConfiguration(new StoreConfiguration());
  }
}
