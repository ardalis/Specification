using Ardalis.SampleApp.Infrastructure.DataAccess;
using Ardalis.SampleApp.Core.Interfaces;
using Ardalis.Specification.EntityFrameworkCore;

namespace Ardalis.SampleApp.Infrastructure.Data
{
    /// <inheritdoc/>
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
