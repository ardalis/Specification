using Ardalis.Specification.IntegrationTests.SampleClient;
using Ardalis.Specification.IntegrationTests.SampleSpecs;
using Dapper;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace Ardalis.Specification.IntegrationTests
{
    [Collection("Sequential")]
    public class DatabaseCommunicationTests : DatabaseCommunicationTestBase
    {
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
            var result = await _dbContext.Blogs.FirstOrDefaultAsync();

            Assert.Equal(BlogBuilder.VALID_BLOG_ID, result.Id);
        }

        [Fact]
        public async Task GetBlogUsingEFRepository()
        {
            var result = await _blogRepository.GetByIdAsync(BlogBuilder.VALID_BLOG_ID);

            result.Should().NotBeNull();
            result.Name.Should().Be(BlogBuilder.VALID_BLOG_NAME);
            result.Posts.Count.Should().Be(0);
        }

        [Fact]
        public async Task GetBlogUsingEFRepositoryAndSpecShouldIncludePosts()
        {
            var result = (await _blogRepository.ListAsync(new BlogWithPostsSpec(BlogBuilder.VALID_BLOG_ID))).SingleOrDefault();

            result.Should().NotBeNull();
            result.Name.Should().Be(BlogBuilder.VALID_BLOG_NAME);
            result.Posts.Count.Should().BeGreaterThan(100);
        }

        [Fact]
        public async Task GetBlogUsingEFRepositoryAndSpecWithStringIncludeShouldIncludePosts()
        {
            var result = (await _blogRepository.ListAsync(new BlogWithPostsUsingStringSpec(BlogBuilder.VALID_BLOG_ID))).SingleOrDefault();

            result.Should().NotBeNull();
            result.Name.Should().Be(BlogBuilder.VALID_BLOG_NAME);
            result.Posts.Count.Should().BeGreaterThan(100);
        }

        [Fact]
        public async Task GetBlogWithPostsAndAuthorsWithChainableIncludeSpecShouldIncludePostsAndAuthors()
        {
            var result = (await _blogRepository.ListAsync(new BlogWithPostsAndAuthorSpec(BlogBuilder.VALID_BLOG_ID))).SingleOrDefault();

            result.Should().NotBeNull();
            result.Name.Should().Be(BlogBuilder.VALID_BLOG_NAME);
            result.Posts.Count.Should().BeGreaterThan(100);
            result.Posts.Select(p => p.Author).ToList().Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetSecondPageOfPostsUsingPostsByBlogPaginatedSpec()
        {
            int pageSize = 10;
            int pageIndex = 1; // page 2
            var result = (await _postRepository.ListAsync(new PostsByBlogPaginatedSpec(pageIndex * pageSize, pageSize, BlogBuilder.VALID_BLOG_ID))).ToList();

            result.Count.Should().Be(pageSize);
            result.First().Id.Should().Be(309);
            result.Last().Id.Should().Be(318);
        }

        [Fact]
        public async Task GetPostsWithOrderedSpec()
        {
            var result = (await _postRepository.ListAsync(new PostsByBlogOrderedSpec(BlogBuilder.VALID_BLOG_ID))).ToList();
            
            result.First().Id.Should().Be(234);
            result.Last().Id.Should().Be(399);
        }

        [Fact]
        public async Task GetPostsOrderedByThenBySpec()
        {
            var result = (await _postRepository.ListAsync(new PostsByBlogOrderedByThenBySpec(BlogBuilder.VALID_BLOG_ID))).ToList();

            result.First().Id.Should().Be(300);
            result.Last().Id.Should().Be(234);
        }

        [Fact]
        public async Task GetPostsOrderedByThenBySpecDescending()
        {
            var result = (await _postRepository.ListAsync(new PostsByBlogOrderedByThenBySpec(BlogBuilder.VALID_BLOG_ID, false))).ToList();

            result.First().Id.Should().Be(234);
            result.Last().Id.Should().Be(399);
        }

        [Fact]
        public async Task GetPostsWithOrderedSpecDescending()
        {
            var result = (await _postRepository.ListAsync(new PostsByBlogOrderedSpec(BlogBuilder.VALID_BLOG_ID, false))).ToList();

            result.First().Id.Should().Be(399);
            result.Last().Id.Should().Be(234);
        }

        // TODO: This could move to the Unit Tests project if specs were in separate project
        [Fact]
        public void EnableCacheShouldSetCacheKeyProperly()
        {
            var spec = new BlogWithPostsSpec(BlogBuilder.VALID_BLOG_ID);

            spec.CacheKey.Should().Be($"BlogWithPostsSpec-{BlogBuilder.VALID_BLOG_ID}");
        }

        //[Fact]
        //public async Task GroupByShouldWorkProperly()
        //{
        //    var spec = new PostsGroupedByIdSpec();
        //    var result = (await _postRepository.ListAsync(spec)).ToList();

        //    result.First().Id.Should().Be(301);
        //    result.Skip(1).Take(1).First().Id.Should().Be(303);
        //}
    }
}
