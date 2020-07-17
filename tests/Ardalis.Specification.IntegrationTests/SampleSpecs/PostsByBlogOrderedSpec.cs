using Ardalis.Specification.IntegrationTests.SampleClient;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class PostsByBlogOrderedSpec : Specification<Post>
    {
        public PostsByBlogOrderedSpec(int blogId, bool isAscending = true)
        {
            Query.Where(p => p.BlogId == blogId);

            if(isAscending)
            {
                Query.OrderBy(p => p.Id);
            }
            else
            {
                Query.OrderByDescending(p => p.Id);
            }
        }
    }

}
