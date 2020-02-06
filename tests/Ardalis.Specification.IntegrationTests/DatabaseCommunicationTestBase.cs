using Ardalis.Specification.IntegrationTests.SampleClient;
using Microsoft.EntityFrameworkCore;

namespace Ardalis.Specification.IntegrationTests
{
    public class DatabaseCommunicationTestBase
    {
        protected readonly SampleDbContext DbContext;
        protected readonly EfRepository<Blog> BlogRepository;
        protected readonly EfRepository<Post> PostRepository;

        protected DatabaseCommunicationTestBase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SampleDbContext>();
            optionsBuilder.UseInMemoryDatabase("SampleDatabase");
            DbContext = new SampleDbContext(optionsBuilder.Options);

            DbContext.Database.EnsureCreated();

            BlogRepository = new EfRepository<Blog>(DbContext);
            PostRepository = new EfRepository<Post>(DbContext);
        }

    }
}