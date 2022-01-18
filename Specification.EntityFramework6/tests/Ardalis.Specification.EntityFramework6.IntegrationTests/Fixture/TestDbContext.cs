using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using Ardalis.Specification.UnitTests.Fixture.Entities;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture
{
  public class TestDbContext : DbContext
  {
    public TestDbContext(DbConnection connection) : base(connection, false)
    {
      Database.SetInitializer(new DbInitializer());
    }

    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<Company> Companies { get; set; }
    public virtual DbSet<Store> Stores { get; set; }
    public virtual DbSet<Address> Addresses { get; set; }
    public virtual DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
        .Where(type => !string.IsNullOrEmpty(type.Namespace))
        .Where(type => type.BaseType != null && type.BaseType.IsGenericType
             && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
      foreach (var type in typesToRegister)
      {
        dynamic configurationInstance = Activator.CreateInstance(type);
        modelBuilder.Configurations.Add(configurationInstance);
      }

      base.OnModelCreating(modelBuilder);
    }
  }
}
