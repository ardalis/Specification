using Ardalis.Specification.IntegrationTests.SampleClient;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class BlogWithPostsSpec : BaseSpecification<Blog>
    {
        public BlogWithPostsSpec(int id) : base(blog => blog.Id == id)
        {
            AddInclude(blog => blog.Posts);
            EnableCache(nameof(BlogWithPostsSpec), id);
        }
    }
}
