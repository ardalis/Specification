using Ardalis.Specification.UnitTests.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Specs
{
    public class StoreIncludeNameSpec : Specification<Store>
    {
        public StoreIncludeNameSpec()
        {
            Query.Include(x => x.Name);
        }
    }
}
