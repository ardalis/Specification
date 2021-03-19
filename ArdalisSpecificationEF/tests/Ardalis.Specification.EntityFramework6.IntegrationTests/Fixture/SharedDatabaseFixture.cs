using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture
{
    public class SharedDatabaseFixture : IDisposable
    {
        // Docker
        public const string ConnectionString = "Data Source=database;Initial Catalog=SampleDatabase;PersistSecurityInfo=True;User ID=sa;Password=P@ssW0rd!";

        // (localdb)
        //public const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Integrated Security=SSPI;Initial Catalog=SpecificationEFTestsDB;ConnectRetryCount=0";

        public SharedDatabaseFixture()
        {
            Connection = new SqlConnection(ConnectionString);
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
}