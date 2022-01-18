using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.Specification.UnitTests.Fixture.Entities;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
  public class StoreIncludeCompanyThenCountryAsStringSpec : Specification<Store>
  {
    public StoreIncludeCompanyThenCountryAsStringSpec()
    {
      Query.Include($"{nameof(Company)}.{nameof(Company.Country)}");
    }
  }
}
