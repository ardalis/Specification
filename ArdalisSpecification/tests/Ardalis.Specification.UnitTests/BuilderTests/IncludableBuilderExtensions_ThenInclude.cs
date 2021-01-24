using FluentAssertions;
using Ardalis.Specification.UnitTests.Fixture.Entities;
using Ardalis.Specification.UnitTests.Fixture.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Ardalis.Specification.UnitTests
{
    public class IncludableBuilderExtensions_ThenInclude
    {
        [Fact]
        public void AppendsNavigationName_GivenSimpleType()
        {
            var spec = new StoreIncludeCompanyThenNameSpec();

            string expected = $"{nameof(Store.Company)}.{nameof(Company.Name)}";
            string actual = spec.IncludeAggregators.FirstOrDefault().IncludeString;

            actual.Should().Be(expected);
        }

        [Fact]
        public void AppendsNavigationName_GivenReferenceType()
        {
            var spec = new StoreIncludeCompanyThenCountrySpec();

            string expected = $"{nameof(Store.Company)}.{nameof(Company.Country)}";
            string actual = spec.IncludeAggregators.FirstOrDefault().IncludeString;

            actual.Should().Be(expected);
        }

        [Fact]
        public void AppendsNavigationName_GivenCollectionType()
        {
            var spec = new StoreIncludeCompanyThenStoresSpec();

            string expected = $"{nameof(Store.Company)}.{nameof(Company.Stores)}";
            string actual = spec.IncludeAggregators.FirstOrDefault().IncludeString;

            actual.Should().Be(expected);
        }

        [Fact]
        public void AppendsNavigationName_GivenTypeOfCollection()
        {
            var spec = new StoreIncludeProductsThenStoreSpec();

            string expected = $"{nameof(Store.Products)}.{nameof(Product.Store)}";
            string actual = spec.IncludeAggregators.FirstOrDefault().IncludeString;

            actual.Should().Be(expected);
        }
    }
}
