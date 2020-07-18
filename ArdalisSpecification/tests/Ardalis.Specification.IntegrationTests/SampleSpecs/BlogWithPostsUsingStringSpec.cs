using Ardalis.Specification.IntegrationTests.SampleClient;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class BlogWithPostsUsingStringSpec : Specification<Blog>
    {
        public BlogWithPostsUsingStringSpec(int id)
        {
            Query.Where(b => b.Id == id)
                .Include("Posts");
        }
    }
}
