using Ardalis.Specification.IntegrationTests.SampleClient;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class BlogNamesSpecification : BaseSpecification<Blog>
    {
        public BlogNamesSpecification() : base(b => true)
        {
            Selector = b => b.Name;
        }
    }
}
