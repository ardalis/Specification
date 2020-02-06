using System;
using Ardalis.Specification.IntegrationTests.SampleClient;
using Ardalis.Specification.IntegrationTests.SampleSpecs;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Ardalis.Specification.IntegrationTests
{
    public class DatabaseCommunicationTests : DatabaseCommunicationTestBase
    {
        [Fact]
        public async Task GetBlogUsingEF()
        {
            var result = await DbContext.Blogs.FirstOrDefaultAsync();

            Assert.Equal(BlogBuilder.VALID_BLOG_ID, result.Id);
        }

        [Fact]
        public async Task GetBlogUsingEFRepository()
        {
            var result = await BlogRepository.GetByIdAsync(BlogBuilder.VALID_BLOG_ID);

            result.Should().NotBeNull();
            result.Name.Should().Be(BlogBuilder.VALID_BLOG_NAME);
            result.Posts.Count.Should().Be(0);
        }

        [Fact]
        public async Task GetBlogUsingEFRepositoryAndSpecShouldIncludePosts()
        {
            var result = (await BlogRepository.ListAsync(new BlogWithPostsSpec(BlogBuilder.VALID_BLOG_ID))).SingleOrDefault();

            result.Should().NotBeNull();
            result.Name.Should().Be(BlogBuilder.VALID_BLOG_NAME);
            result.Posts.Count.Should().BeGreaterThan(100);
        }

        [Fact]
        public async Task GetBlogUsingEFRepositoryAndSpecWithStringIncludeShouldIncludePosts()
        {
            var result = (await BlogRepository.ListAsync(new BlogWithPostsUsingStringSpec(BlogBuilder.VALID_BLOG_ID))).SingleOrDefault();

            result.Should().NotBeNull();
            result.Name.Should().Be(BlogBuilder.VALID_BLOG_NAME);
            result.Posts.Count.Should().BeGreaterThan(100);
        }

        [Fact]
        public async Task GetBlogWithPostsAndAuthorsWithChainableIncludeSpecShouldIncludePostsAndAuthors()
        {
            var result = (await BlogRepository.ListAsync(new BlogWithPostsAndAuthorSpec(BlogBuilder.VALID_BLOG_ID))).SingleOrDefault();

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
            var result = (await PostRepository.ListAsync(new PostsByBlogPaginatedSpec(pageIndex * pageSize, pageSize, BlogBuilder.VALID_BLOG_ID))).ToList();

            result.Count.Should().Be(pageSize);
            result.First().Id.Should().Be(309);
            result.Last().Id.Should().Be(318);
        }

        [Fact]
        public async Task GetPostsWithOrderedSpec()
        {
            var result = (await PostRepository.ListAsync(new PostsByBlogOrderedSpec(BlogBuilder.VALID_BLOG_ID))).ToList();

            result.First().Id.Should().Be(234);
            result.Last().Id.Should().Be(399);
        }

        [Fact]
        public async Task GetPostsWithOrderedSpecDescending()
        {
            var result = (await PostRepository.ListAsync(new PostsByBlogOrderedSpec(BlogBuilder.VALID_BLOG_ID, false))).ToList();

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

        [Fact]
        public void GroupByShouldNotWorkProperlyInEf3()
        {
            var spec = new PostsGroupedByIdSpec();

            Func<Task> groupPostsByIdAction = 
                async () => (await PostRepository.ListAsync(spec)).ToList();

            groupPostsByIdAction
                .Should()
                .Throw<InvalidOperationException>("EF Core +3.0 no longer supports Client-Side evaluation of queries.")
                .WithMessage("*This may indicate either a bug or a limitation in EF Core.*");
        }
    }
}
