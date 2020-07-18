using Ardalis.Specification.IntegrationTests.SampleClient;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class BlogWithPostsAndAuthorSpec : Specification<Blog>
    {
        public BlogWithPostsAndAuthorSpec(int id)
        {
            Query.Where(b => b.Id == id)
                .Include(b => b.Posts)
                .ThenInclude(p => p.Author);
        }
    }
}
