using Ardalis.Specification.IntegrationTests.SampleClient;
using Microsoft.EntityFrameworkCore;


namespace Ardalis.Specification.IntegrationTests
{
    public class DatabaseCommunicationTestBase
    {
        // Run EF Migrations\DBUp script to prepare database before running your tests.
        public const string ConnectionString = "Data Source=database;Initial Catalog=SampleDatabase;PersistSecurityInfo=True;User ID=sa;Password=P@ssW0rd!";
        public SampleDbContext _dbContext;
        public EfRepository<Blog> _blogRepository;
        public EfRepository<Post> _postRepository;

        public DatabaseCommunicationTestBase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SampleDbContext>();
            optionsBuilder.UseSqlServer(ConnectionString);
            _dbContext = new SampleDbContext(optionsBuilder.Options);

            // Run this if you've made seed data or schema changes to force the container to rebuild the db
            // _dbContext.Database.EnsureDeleted();

            // Note: If the database exists, this will do nothing, so it only creates it once.
            // This is fine since these tests all perform read-only operations
            _dbContext.Database.EnsureCreated();

            _blogRepository = new EfRepository<Blog>(_dbContext);
            _postRepository = new EfRepository<Post>(_dbContext);
        }

    }
}