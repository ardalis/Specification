using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using MartinCostello.SqlLocalDb;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture
{
  public class SharedDatabaseFixture : IDisposable
  {
    // Docker
    public const string ConnectionStringDocker = "Data Source=databaseEFCore;Initial Catalog=SpecificationEFCoreTestsDB;PersistSecurityInfo=True;User ID=sa;Password=P@ssW0rd!";

    // (localdb)
    public const string ConnectionStringLocalDb = "Server=(localdb)\\mssqllocaldb;Integrated Security=SSPI;Initial Catalog=SpecificationEFTestsDB;ConnectRetryCount=0";


    private static readonly object _lock = new object();
    private static bool _databaseInitialized;

    public SharedDatabaseFixture()
    {
      var isLocalDBInstalled = false;

      using (var localDB = new SqlLocalDbApi())
      {
        isLocalDBInstalled = localDB.IsLocalDBInstalled();
      }

      Connection = isLocalDBInstalled
                  ? new SqlConnection(ConnectionStringLocalDb)
                  : new SqlConnection(ConnectionStringDocker);

      Seed();

      Connection.Open();
    }

    // This would work only if the DB already exists, otherwise obviously won't be able to open a connection.
    // Therefore, in the ctor we're using a Nuget package for this check, it's more robust.
    private bool IsLocalDbAvailable1()
    {
      try
      {
        using (var connection = new SqlConnection(ConnectionStringLocalDb))
        {
          connection.Open();
          connection.Close();
        }

        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public DbConnection Connection { get; }

    public TestDbContext CreateContext(DbTransaction? transaction = null)
    {
      var context = new TestDbContext(new DbContextOptionsBuilder<TestDbContext>().UseSqlServer(Connection).Options);

      if (transaction != null)
      {
        context.Database.UseTransaction(transaction);
      }

      return context;
    }

    private void Seed()
    {
      lock (_lock)
      {
        if (!_databaseInitialized)
        {
          using (var context = CreateContext())
          {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
          }

          _databaseInitialized = true;
        }
      }
    }

    public void Dispose() => Connection.Dispose();
  }
}
