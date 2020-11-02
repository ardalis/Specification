using Ardalis.SampleApp.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SampleApp.Core.Interfaces;
using Ardalis.Specification.EntityFrameworkCore;

namespace Ardalis.SampleApp.Infrastructure.Data
{
    public class MyRepository<T> : RepositoryBase<T>, IRepository<T> where T : class, IAggregateRoot
    {
        private readonly SampleDbContext dbContext;

        public MyRepository(SampleDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        // Not required to implement anything. Add additional functionalities if required.
    }
}
