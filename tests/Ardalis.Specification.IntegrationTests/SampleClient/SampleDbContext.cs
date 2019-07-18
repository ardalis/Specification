using Microsoft.EntityFrameworkCore;

namespace Ardalis.Specification.IntegrationTests.SampleClient
{
    public class SampleDbContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Author> Authors { get; set; }

        public SampleDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var author = new Author { Name = "Steve Smith", Email = "steve@ardalis.com", Id=123 };
            modelBuilder.Entity<Author>().HasData(author);

            var blogBuilder = new BlogBuilder();
            var testBlog = blogBuilder.WithTestValues().Build();
            modelBuilder.Entity<Blog>().HasData(testBlog);

            var post = new Post { AuthorId = author.Id, BlogId = testBlog.Id, Title = "First post!", Content = "Lorem ipsum" };
            modelBuilder.Entity<Post>().HasData(post);

        }
    }
}
