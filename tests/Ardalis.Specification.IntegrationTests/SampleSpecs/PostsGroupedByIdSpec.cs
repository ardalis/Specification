using Ardalis.Specification.IntegrationTests.SampleClient;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class PostsGroupedByIdSpec : BaseSpecification<Post>
    {
        public PostsGroupedByIdSpec() : base(post => true)
        {
            ApplyGroupBy(post => post.Id % 2 == 0);
        }
    }
}
