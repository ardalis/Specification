using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class CompanyByIdAsUntrackedSpec : Specification<Company>, ISingleResultSpecification
    {
        public CompanyByIdAsUntrackedSpec(int id)
        {
            Query.Where(company => company.Id == id).AsNoTracking();
        }
    }
}
