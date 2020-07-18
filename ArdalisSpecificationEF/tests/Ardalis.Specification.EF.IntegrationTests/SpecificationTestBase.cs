using Ardalis.Specification.EF.IntegrationTests.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.EF.IntegrationTests
{
    public class SpecificationTestBase
    {
        public const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Integrated Security=SSPI;Initial Catalog=SpecificationEFTestsDB;";
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
