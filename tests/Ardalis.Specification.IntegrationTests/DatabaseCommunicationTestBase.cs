using Ardalis.Specification.IntegrationTests.SampleClient;
using Microsoft.EntityFrameworkCore;

namespace Ardalis.Specification.IntegrationTests
{
    public class DatabaseCommunicationTestBase
    {
        protected readonly SampleDbContext _dbContext;
        protected readonly EfRepository<Blog> _blogRepository;
        protected readonly EfRepository<Post> _postRepository;

        protected DatabaseCommunicationTestBase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SampleDbContext>();
            optionsBuilder.UseInMemoryDatabase("SampleDatabase");
            _dbContext = new SampleDbContext(optionsBuilder.Options);

            _dbContext.Database.EnsureCreated();

            _blogRepository = new EfRepository<Blog>(_dbContext);
            _postRepository = new EfRepository<Post>(_dbContext);
        }

    }
}