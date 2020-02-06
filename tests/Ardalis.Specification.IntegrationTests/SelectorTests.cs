﻿using Ardalis.Specification.IntegrationTests.SampleClient;
using Ardalis.Specification.IntegrationTests.SampleSpecs;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Ardalis.Specification.IntegrationTests
{
    public class SelectorTests : DatabaseCommunicationTestBase
    {
        [Fact]
        public async Task GetBlogsSelectNameOnlyReturnsStringWithNameUsingDbContext()
        {
            var result = await DbContext.Blogs
                .Select(new BlogNamesSpecification().Selector)
                .FirstOrDefaultAsync();

            result.Should().NotBeNull();
            result.Should().Be(BlogBuilder.VALID_BLOG_NAME);
        }

        [Fact]
        public async Task GetBlogsSelectNameOnlyReturnsStringWithNameUsingEfRepository()
        {
            var spec = new BlogNamesSpecification();
            var result = (await BlogRepository.ListAsync(spec))
                .FirstOrDefault();

            result.Should().NotBeNull();
            result.Should().Be(BlogBuilder.VALID_BLOG_NAME);
        }

        [Fact]
        public async Task GetPostsSelectNameAndContentOnlyReturnsExpectedResultsUsingEfRepository()
        {
            var spec = new PostSelectSpecification(p => new { p.Title, p.Content });
            dynamic result = (await PostRepository.ListAsync(spec))
                .FirstOrDefault();

            Assert.NotNull(result);
            Assert.Equal("First post!", result.Title);
            Assert.Equal("Lorem ipsum", result.Content);
        }
    }
}