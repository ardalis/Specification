using Ardalis.Specification.IntegrationTests.SampleClient;
using Ardalis.Specification.QueryExtensions.Include;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class BlogWithPostsAndAuthorSpec : BaseSpecification<Blog>
    {
        public BlogWithPostsAndAuthorSpec(int id) : base(blog => blog.Id == id)
        {
            AddIncludes(agg =>
                agg.Include(blog => blog.Posts)
                    .ThenInclude(post => post.Author));
        }
    }
}
