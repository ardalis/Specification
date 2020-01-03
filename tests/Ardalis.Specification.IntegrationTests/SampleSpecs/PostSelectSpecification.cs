using Ardalis.Specification.IntegrationTests.SampleClient;
using System;
using System.Linq.Expressions;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class PostSelectSpecification : BaseSpecification<Post, object>
    {
        public PostSelectSpecification(Expression<Func<Post, object>> selector) : base(b => true)
        {
            Selector = selector;
        }
    }
}
