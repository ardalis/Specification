﻿using Ardalis.Specification.IntegrationTests.SampleClient;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class PostsByBlogPaginatedSpec : BaseSpecification<Post>
    {
        public PostsByBlogPaginatedSpec(int skip, int take, int blogId)
            : base(post => post.BlogId == blogId)
        {
            ApplyPaging(skip, take);
        }
    }

}
