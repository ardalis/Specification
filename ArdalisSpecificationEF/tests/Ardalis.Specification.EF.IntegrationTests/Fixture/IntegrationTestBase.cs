using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture
{
    public class IntegrationTestBase : IClassFixture<SharedDatabaseFixture>
    {
        protected TestDbContext dbContext;
        protected Repository<Company> companyRepository;
        protected Repository<Store> storeRepository;

        public IntegrationTestBase(SharedDatabaseFixture fixture)
        {
            dbContext = fixture.CreateContext();

            companyRepository = new Repository<Company>(dbContext);
            storeRepository = new Repository<Store>(dbContext);
        }
    }
}
