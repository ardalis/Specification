using Ardalis.Specification.IntegrationTests.SampleClient;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class PostsByBlogOrderedSpec : BaseSpecification<Post>
    {
        public PostsByBlogOrderedSpec(int blogId, bool isAscending = true)
            : base(post => post.BlogId == blogId)
        {
            if(isAscending)
            {
                ApplyOrderBy(post => post.Id);
            }
            else
            {
                ApplyOrderByDescending(post => post.Id);
            }
        }
    }
}
