using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture
{
    public class Repository<T> : RepositoryBase<T> where T : class
    {
        protected readonly TestDbContext dbContext;

        public Repository(TestDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
