using Ardalis.Specification.IntegrationTests.SampleClient;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class BlogWithPostsSpec : BaseSpecification<Blog>
    {
        public BlogWithPostsSpec(int id) : base(b => b.Id == id)
        {
            base.Includes.Add(b => b.Posts);
        }
    }
}
