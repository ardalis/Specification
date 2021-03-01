using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class CompanyByIdSpec : Specification<Company>, ISingleResultSpecification
    {
        public CompanyByIdSpec(int id)
        {
            Query.Where(company => company.Id == id);
        }
    }
}
