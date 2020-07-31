using Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Data;
using Microsoft.EntityFrameworkCore;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests
{
    public class SpecificationTestBase
    {
        // Docker
        public const string ConnectionString = "Data Source=databaseEF;Initial Catalog=SampleDatabase;PersistSecurityInfo=True;User ID=sa;Password=P@ssW0rd!";

        // (localdb)
        //public const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Integrated Security=SSPI;Initial Catalog=SpecificationEFTestsDB;";

        protected TestDbContext dbContext;
        protected Repository<Company> companyRepository;
        protected Repository<Store> storeRepository;

        public SpecificationTestBase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.UseSqlServer(ConnectionString);
            dbContext = new TestDbContext(optionsBuilder.Options);

            dbContext.Database.EnsureCreated();

            companyRepository = new Repository<Company>(dbContext);
            storeRepository = new Repository<Store>(dbContext);
        }
    }
}
