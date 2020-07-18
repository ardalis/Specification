using Ardalis.Specification.IntegrationTests.SampleClient;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class PostSelectSpecification : Specification<Post, object>
    {
        public PostSelectSpecification(Expression<Func<Post, object>> selector)
        {
            Query.Select(selector);
        }
    }
}
