using Ardalis.Specification.UnitTests.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Specs
{
    public class StoreIncludeMethodSpec : Specification<Store>
    {
        public StoreIncludeMethodSpec()
        {
            Query.Include(x => x.GetSomethingFromStore());
        }
    }
}