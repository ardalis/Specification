using Ardalis.Specification.IntegrationTests.SampleClient;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class PostsByBlogOrderedByThenBySpec : BaseSpecification<Post>
    {
        public PostsByBlogOrderedByThenBySpec(int blogId, bool isAscending = true)
            : base(p => p.BlogId == blogId)
        {
            if(isAscending)
            {
                ApplyOrderByWithThenBy(p => p.AuthorId, p => p.Title);
            }
            else
            {
                ApplyOrderByDescendingWithThenBy(p => p.Id, p => p.Title);
            }
        }
    }

}
