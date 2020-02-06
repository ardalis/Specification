using Ardalis.Specification.IntegrationTests.SampleClient;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class BlogWithPostsUsingStringSpec : BaseSpecification<Blog>
    {
        public BlogWithPostsUsingStringSpec(int id) : base(blog => blog.Id == id)
        {
            AddInclude("Posts");
        }
    }
}
