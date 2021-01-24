using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoreByIdAndNameSpec : Specification<Store>
    {
        public StoreByIdAndNameSpec(int Id, string name)
        {
            Query.Where(x => x.Id == Id)
                .Where(x => x.Name == name);
        }
    }
}
