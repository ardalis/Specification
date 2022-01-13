using Ardalis.Specification.UnitTests.Fixture.Entities;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
    public class StoresOrderedDescendingByNameSpec : Specification<Store>
    {
        public StoresOrderedDescendingByNameSpec()
        {
            Query.OrderByDescending(x => x.Name);
        }
    }
}