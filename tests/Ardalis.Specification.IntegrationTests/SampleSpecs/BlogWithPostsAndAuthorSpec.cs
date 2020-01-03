using Ardalis.Specification.IntegrationTests.SampleClient;
using Ardalis.Specification.QueryExtensions.Include;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class BlogWithPostsAndAuthorSpec : BaseSpecification<Blog>
    {
        public BlogWithPostsAndAuthorSpec(int id) : base(b => b.Id == id)
        {
            AddIncludes(x =>
             x.Include(b => b.Posts)
                .ThenInclude(p => p.Author)
            );
        }
    }
}
