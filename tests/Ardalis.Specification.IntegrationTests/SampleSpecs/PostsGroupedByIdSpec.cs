using Ardalis.Specification.IntegrationTests.SampleClient;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class PostsGroupedByIdSpec : Specification<Post>
    {
        public PostsGroupedByIdSpec()
        {
            // Removed in version 3.0
            //ApplyGroupBy(p => p.Id % 2 == 0);
        }
    }

}
