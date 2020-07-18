using Ardalis.Specification.UnitTests.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Specs
{
    public class StoreIncludeMethodOfNavigationSpec : Specification<Store>
    {
        public StoreIncludeMethodOfNavigationSpec()
        {
            Query.Include(x => x.Address.GetSomethingFromAddress());
        }
    }
}