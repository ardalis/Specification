using Ardalis.Specification.IntegrationTests.SampleClient;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    // We are selecting/returning b.Name (string property), so we define the TResult generic parameter as string.
    // It's the only place where this generic parameter is defined, everywhere else will be implicitly inferred.
    public class BlogNamesSpecification : BaseSpecification<Blog, string>
    {
        public BlogNamesSpecification() : base(b => true)
        {
            Selector = b => b.Name;
        }
    }
}
