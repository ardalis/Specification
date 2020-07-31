using Ardalis.Specification.IntegrationTests.SampleClient;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class BlogWithPostsSpec : Specification<Blog>
    {
        public BlogWithPostsSpec(int id)
        {
            Query.Where(b => b.Id == id)
                .EnableCache(nameof(BlogWithPostsSpec), id)
                .Include(b => b.Posts);
        }
    }
}
