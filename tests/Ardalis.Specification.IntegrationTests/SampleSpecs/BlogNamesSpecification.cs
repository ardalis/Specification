using Ardalis.Specification.IntegrationTests.SampleClient;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    // [fiseni, 11/22/2019] All the complexity of this infrastructure is hidden from the users.
    // This is the only place where they will face the second generic parameter of the specifications.
    // And, probably this is the right place for that. 
    // This is the place where they define the expression for the selector, and it's the same place where they define the type of the output as well.
    // You do this once, and u never worry anymore while consuming the repo. Strongly typed, no casting involved.
    public class BlogNamesSpecification : BaseSpecification<Blog, string>
    {
        public BlogNamesSpecification() : base(b => true)
        {
            Selector = b => b.Name;
        }
    }
}
