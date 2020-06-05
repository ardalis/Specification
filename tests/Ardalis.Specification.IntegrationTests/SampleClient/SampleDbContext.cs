using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Ardalis.Specification.IntegrationTests.SampleClient
{
    public class SampleDbContext : DbContext
    {
        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => 
        {
            builder.AddFilter("Ardalis.Specification", LogLevel.Debug);
            builder.AddConsole();
        });

        public DbSet<Author> Authors { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        public SampleDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(loggerFactory);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var author = new Author { Name = "Steve Smith", Email = "steve@ardalis.com", Id=123 };
            modelBuilder.Entity<Author>().HasData(author);

            var blogBuilder = new BlogBuilder();
            var testBlog = blogBuilder.WithTestValues().Build();
            modelBuilder.Entity<Blog>().HasData(testBlog);

            var postList = new List<Post>();
            postList.Add(new Post { Id=234, AuthorId = author.Id, BlogId = testBlog.Id, Title = "First post!", Content = "Lorem ipsum" });
            for (int i = 0; i < 100; i++)
            {
                postList.Add(new Post { AuthorId = author.Id, BlogId = testBlog.Id, Title = $"Extra post {i}", Id = 300 + i });
            }
            modelBuilder.Entity<Post>().HasData(postList);

        }
    }
}
