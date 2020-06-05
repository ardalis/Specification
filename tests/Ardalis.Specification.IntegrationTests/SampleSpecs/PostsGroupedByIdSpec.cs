using Ardalis.Specification.IntegrationTests.SampleClient;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class PostsGroupedByIdSpec : BaseSpecification<Post>
    {
        public PostsGroupedByIdSpec() : base(p => true)
        {
            // Removed in version 3.0
            //ApplyGroupBy(p => p.Id % 2 == 0);
        }
    }

}
