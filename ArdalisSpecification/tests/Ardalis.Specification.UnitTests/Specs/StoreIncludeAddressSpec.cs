using Ardalis.Specification.UnitTests.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Specs
{
    public class StoreIncludeAddressSpec : Specification<Store>
    {
        public StoreIncludeAddressSpec()
        {
            Query.Include(x => x.Address);
        }
    }
}