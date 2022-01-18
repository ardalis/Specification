using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.Specification.UnitTests.Fixture.Entities;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
  public class StoreIncludeAddressSpec : Specification<Store>
  {
    public StoreIncludeAddressSpec()
    {
      Query.Include(x => x.Address);
    }
  }
}
