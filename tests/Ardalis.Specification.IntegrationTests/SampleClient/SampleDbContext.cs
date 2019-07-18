using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.IntegrationTests.SampleClient
{
    public class SampleDbContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        public SampleDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var blogBuilder = new BlogBuilder();
            modelBuilder.Entity<Blog>().HasData(blogBuilder.WithTestValues().Build());
        }
    }
}
