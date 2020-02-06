using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Ardalis.Specification.IntegrationTests.SampleClient
{
    public class SampleDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        public SampleDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            optionsBuilder.UseLoggerFactory(loggerFactory);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Author author = SeedAuthor(modelBuilder);
            Blog testBlog = SeedBlogs(modelBuilder);

            SeedPosts(modelBuilder, author, testBlog);
        }

        private static Author SeedAuthor(ModelBuilder modelBuilder)
        {
            var author = new Author
            {
                Id = 123,
                Name = "Steve Smith", 
                Email = "steve@ardalis.com"
            };

            modelBuilder.Entity<Author>().HasData(author);

            return author;
        }
        private static Blog SeedBlogs(ModelBuilder modelBuilder)
        {
            var blogBuilder = new BlogBuilder();

            Blog testBlog = blogBuilder.WithTestValues().Build();
            modelBuilder.Entity<Blog>().HasData(testBlog);

            return testBlog;
        }

        private static void SeedPosts(ModelBuilder modelBuilder, Author author, Blog testBlog)
        {
            var postList = new List<Post>
            {
                new Post
                {
                    Id = 234,
                    Title = "First post!",
                    AuthorId = author.Id,
                    BlogId = testBlog.Id,
                    Content = "Lorem ipsum"
                }
            };

            for (var i = 0; i < 100; i++)
            {
                postList.Add(new Post
                {
                    Id = 300 + i,
                    Title = $"Extra post {i}",
                    AuthorId = author.Id,
                    BlogId = testBlog.Id
                });
            }

            modelBuilder.Entity<Post>().HasData(postList);
        }
    }
}
