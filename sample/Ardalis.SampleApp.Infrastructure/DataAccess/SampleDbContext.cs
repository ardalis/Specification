using Microsoft.EntityFrameworkCore;
using Ardalis.SampleApp.Infrastructure.DataAccess.Configurations;
using Ardalis.SampleApp.Core.Entities.CustomerAggregate;

namespace Ardalis.SampleApp.Infrastructure.DataAccess
{
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
}
