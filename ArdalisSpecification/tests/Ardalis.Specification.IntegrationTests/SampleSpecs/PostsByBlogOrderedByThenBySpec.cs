using Ardalis.Specification.IntegrationTests.SampleClient;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class PostsByBlogOrderedByThenBySpec : Specification<Post>
    {
        public PostsByBlogOrderedByThenBySpec(int blogId, bool isAscending = true)
        {
            Query.Where(p => p.BlogId == blogId);

            if(isAscending)
            {
                Query.OrderBy(p => p.AuthorId)
                    .ThenBy(p => p.Title!);
            }
            else
            {
                Query.OrderByDescending(p => p.Id)
                    .ThenBy(p => p.Title!);
            }
        }
    }

}
