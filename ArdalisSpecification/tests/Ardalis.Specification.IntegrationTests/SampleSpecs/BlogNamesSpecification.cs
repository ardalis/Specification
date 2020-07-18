using Ardalis.Specification.IntegrationTests.SampleClient;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class BlogNamesSpecification : Specification<Blog, string>
    {
        public BlogNamesSpecification()
        {
            Query.Select(b => b.Name);
        }
    }
}
