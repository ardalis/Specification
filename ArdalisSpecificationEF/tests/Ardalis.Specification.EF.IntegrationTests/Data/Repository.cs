namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Data
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
