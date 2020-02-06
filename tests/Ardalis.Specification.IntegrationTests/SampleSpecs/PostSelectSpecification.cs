using Ardalis.Specification.IntegrationTests.SampleClient;
using System;
using System.Linq.Expressions;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    // Just an example where selector expression is passed as an argument.
    // Since it's generic solution, we should be cautious in its usage.
    public class PostSelectSpecification : BaseSpecification<Post, object>
    {
        public PostSelectSpecification(Expression<Func<Post, object>> selector) : base(b => true)
        {
            Selector = selector;
        }
    }
}
