using Ardalis.Specification.IntegrationTests.SampleClient;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class PostsByBlogPaginatedSpec : Specification<Post>
    {
        public PostsByBlogPaginatedSpec(int skip, int take, int blogId)
        {
            Query.Where(p => p.BlogId == blogId)
                .Skip(skip)
                .Take(take);
        }
    }

}
