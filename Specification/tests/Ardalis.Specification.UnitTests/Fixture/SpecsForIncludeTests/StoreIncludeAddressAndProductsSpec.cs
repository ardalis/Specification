using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.Specification.UnitTests.Fixture.Entities;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
  public class StoreIncludeAddressAndProductsSpec : Specification<Store>
  {
    public StoreIncludeAddressAndProductsSpec()
    {
      Query.Include(x => x.Products)
           .Include(x => x!.Address);
    }
  }
}
