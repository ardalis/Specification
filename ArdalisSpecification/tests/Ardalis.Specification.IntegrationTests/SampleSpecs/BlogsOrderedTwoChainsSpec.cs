using Ardalis.Specification.IntegrationTests.SampleClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.IntegrationTests.SampleSpecs
{
    public class BlogsOrderedTwoChainsSpec : Specification<Blog>
    {
        public BlogsOrderedTwoChainsSpec()
        {
            Query.OrderBy(x => x.Id)
                    .ThenBy(x => x.Name!);
            Query.OrderBy(x => x.Url!);
        }
    }
}
