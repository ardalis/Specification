using Ardalis.Specification.UnitTests.Fixture.Entities;

namespace Ardalis.Specification.UnitTests.Fixture.Specs
{
  public class StoreByIdSpec : Specification<Store>
  {
    public StoreByIdSpec(int Id)
    {
      Query.Where(x => x.Id == Id);
    }
  }
}
