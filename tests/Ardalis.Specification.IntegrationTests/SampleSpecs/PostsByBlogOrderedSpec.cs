using Ardalis.Specification.IntegrationTests.SampleClient;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class PostsByBlogOrderedSpec : BaseSpecification<Post>
    {
        public PostsByBlogOrderedSpec(int blogId, bool isAscending = true)
            : base(p => p.BlogId == blogId)
        {
            if(isAscending)
            {
                ApplyOrderBy(p => p.Id);
            }
            else
            {
                ApplyOrderByDescending(p => p.Id);
            }
        }
    }

}
