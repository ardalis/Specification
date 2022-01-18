using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.Specification.UnitTests.Fixture.Entities;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
  public class StoreIncludeCompanyCountryDotSeparatedSpec : Specification<Store>
  {
    public StoreIncludeCompanyCountryDotSeparatedSpec()
    {
      Query.Include(x => x.Company!.Country);
    }
  }
}
