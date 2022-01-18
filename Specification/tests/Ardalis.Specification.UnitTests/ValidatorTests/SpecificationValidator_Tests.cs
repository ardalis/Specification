using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification.UnitTests.Fixture.Entities;
using Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;
using Ardalis.Specification.UnitTests.Fixture.Specs;
using FluentAssertions;

namespace Ardalis.Specification.UnitTests.ValidatorTests
{
  public class SpecificationValidator_Tests
  {
    Store store = StoreSeed.Get().Single(x => x.Id == StoreSeed.VALID_Search_ID);

    public void ReturnsTrue_GivenStoreByIdSearchByNameAndCitySpec_WithValidValues()
    {
      var spec = new StoreByIdSearchByNameAndCitySpec(StoreSeed.VALID_Search_ID, StoreSeed.VALID_Search_City_Name_Key);

      var result = spec.IsSatisfiedBy(store);

      result.Should().BeTrue();
    }

    public void ReturnsFalse_GivenStoreByIdSearchByNameAndCitySpec_WithInvalidId()
    {
      var spec = new StoreByIdSearchByNameAndCitySpec(1, StoreSeed.VALID_Search_City_Name_Key);

      var result = spec.IsSatisfiedBy(store);

      result.Should().BeFalse();
    }

    public void ReturnsFalse_GivenStoreByIdSearchByNameAndCitySpec_WithInvalidNameSearchString()
    {
      var spec = new StoreByIdSearchByNameAndCitySpec(StoreSeed.VALID_Search_ID, StoreSeed.VALID_Search_City_Key);

      var result = spec.IsSatisfiedBy(store);

      result.Should().BeFalse();
    }

    public void ReturnsFalse_GivenStoreByIdSearchByNameAndCitySpec_WithInvalidCitySearchString()
    {
      var spec = new StoreByIdSearchByNameAndCitySpec(StoreSeed.VALID_Search_ID, StoreSeed.VALID_Search_Name_Key);

      var result = spec.IsSatisfiedBy(store);

      result.Should().BeFalse();
    }

    public void ReturnsFalse_StoreByIdSearchByNameAndCitySpecSpec_WithInvalidCityAndNameSearchString()
    {
      var spec = new StoreByIdSearchByNameAndCitySpec(StoreSeed.VALID_Search_ID, "random");

      var result = spec.IsSatisfiedBy(store);

      result.Should().BeFalse();
    }

    public void ReturnsTrue_StoreByIdSearchByNameOrCitySpecSpec_WithValidValues()
    {
      var spec = new StoreByIdSearchByNameOrCitySpec(StoreSeed.VALID_Search_ID, StoreSeed.VALID_Search_City_Name_Key);

      var result = spec.IsSatisfiedBy(store);

      result.Should().BeTrue();
    }

    public void ReturnsTrue_StoreByIdSearchByNameOrCitySpecSpec_WithInvalidNameSearchString()
    {
      var spec = new StoreByIdSearchByNameOrCitySpec(StoreSeed.VALID_Search_ID, StoreSeed.VALID_Search_City_Key);

      var result = spec.IsSatisfiedBy(store);

      result.Should().BeTrue();
    }

    public void ReturnsTrue_StoreByIdSearchByNameOrCitySpecSpec_WithInvalidCitySearchString()
    {
      var spec = new StoreByIdSearchByNameOrCitySpec(StoreSeed.VALID_Search_ID, StoreSeed.VALID_Search_Name_Key);

      var result = spec.IsSatisfiedBy(store);

      result.Should().BeTrue();
    }

    public void ReturnsFalse_StoreByIdSearchByNameOrCitySpecSpec_WithInvalidCityAndNameSearchString()
    {
      var spec = new StoreByIdSearchByNameOrCitySpec(StoreSeed.VALID_Search_ID, "random");

      var result = spec.IsSatisfiedBy(store);

      result.Should().BeFalse();
    }
  }
}
