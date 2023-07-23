using MartinCostello.SqlLocalDb;
using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture;

public class SharedDatabaseFixture : IDisposable
{
    public const string _connectionStringDocker =
        "Data Source=databaseEF;Initial Catalog=SpecificationEF6TestsDB;PersistSecurityInfo=True;User ID=sa;Password=P@ssW0rd!;ConnectRetryCount=0;";

    public const string _connectionStringLocalDb = 
        "Data Source=(localdb)\\mssqllocaldb;Integrated Security=SSPI;Initial Catalog=SpecificationEF6TestsDB;ConnectRetryCount=0;";

    public SharedDatabaseFixture()
    {
        var isLocalDBInstalled = false;

        using (var localDB = new SqlLocalDbApi())
        {
            isLocalDBInstalled = localDB.IsLocalDBInstalled();
        }

        Connection = isLocalDBInstalled
                    ? new SqlConnection(_connectionStringLocalDb)
                    : new SqlConnection(_connectionStringDocker);
    }

    public DbConnection Connection { get; }

    public TestDbContext CreateContext(DbTransaction transaction = null)
    {
        var context = new TestDbContext(Connection);

        if (transaction != null)
        {
            context.Database.UseTransaction(transaction);
        }

        return context;
    }

    public void Dispose() => Connection.Dispose();
}
