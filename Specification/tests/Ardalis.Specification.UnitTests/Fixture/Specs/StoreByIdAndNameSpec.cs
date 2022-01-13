using Ardalis.Specification.UnitTests.Fixture.Entities;

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
