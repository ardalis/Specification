using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoresByCompanyOrderedDescByNameThenByIdSpec : Specification<Store>
    {
        public StoresByCompanyOrderedDescByNameThenByIdSpec(int companyId)
        {
            Query.Where(x => x.CompanyId == companyId)
                 .OrderByDescending(x => x.Name)
                 .ThenBy(x => x.Id);
        }
    }
}