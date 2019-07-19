using Ardalis.Specification.IntegrationTests.SampleClient;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class PostsGroupedByIdSpec : BaseSpecification<Post>
    {
        public PostsGroupedByIdSpec() : base(p => true)
        {
            ApplyGroupBy(p => p.Id % 2 == 0);
        }
    }

}
