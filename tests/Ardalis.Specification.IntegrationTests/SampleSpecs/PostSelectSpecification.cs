using Ardalis.Specification.IntegrationTests.SampleClient;
using System;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class PostSelectSpecification : BaseSpecification<Post>
    {
        public PostSelectSpecification(Func<Post,object> selector) : base(b => true)
        {
            Selector = selector;
        }
    }
}
