using Ardalis.Specification.IntegrationTests.SampleClient;
using Dapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace Ardalis.Specification.IntegrationTests
{
    public class DatabaseCommunicationTests
    {
        // Run EF Migrations\DBUp script to prepare database before running your tests.
        const string ConnectionString = "Data Source=database;Initial Catalog=SampleDatabase;PersistSecurityInfo=True;User ID=sa;Password=P@ssW0rd!";
        public SampleDbContext _dbContext;
        public EfRepository<Blog> _blogRepository;

        public DatabaseCommunicationTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SampleDbContext>();
            optionsBuilder.UseSqlServer(ConnectionString);
            _dbContext = new SampleDbContext(optionsBuilder.Options);

            // Note: If the database exists, this will do nothing, so it only creates it once.
            // This is fine since these tests all perform read-only operations
            _dbContext.Database.EnsureCreated();

            _blogRepository = new EfRepository<Blog>(_dbContext);
        }

        [Fact]
        public async Task CanConnectAndRunQuery()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                await conn.OpenAsync();
                const string query = "SELECT 1 AS Data";
                var result = (await conn.QueryAsync<int>(query)).FirstOrDefault();
                result.Should().Be(1);
            }
        }

        [Fact]
        public async Task GetBlogUsingEF()
        {
            var result = _dbContext.Blogs.FirstOrDefault();

            Assert.Equal(BlogBuilder.VALID_BLOG_ID, result.Id);
        }

        [Fact]
        public async Task GetBlogUsingEFRepository()
        {
            var result = await _blogRepository.GetByIdAsync(BlogBuilder.VALID_BLOG_ID);

            result.Should().NotBeNull();
            result.Name.Should().Be(BlogBuilder.VALID_BLOG_NAME);
        }
    }
}
