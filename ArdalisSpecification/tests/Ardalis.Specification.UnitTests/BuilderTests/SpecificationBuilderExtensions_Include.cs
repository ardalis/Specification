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
    public class SpecificationBuilderExtensions_Include
    {
        [Fact]
        public void AddsNothingToList_GivenNoIncludeExpression()
        {
            var spec = new StoreEmptySpec();

            spec.IncludeAggregators.Should().BeEmpty();
        }

        [Fact]
        public void AddsAggregatorToList_GivenExpression()
        {
            var spec = new StoreIncludeNameSpec();

            spec.IncludeAggregators.Should().ContainSingle();
        }

        [Fact]
        public void AddsNavigationName_GivenSimpleType()
        {
            var spec = new StoreIncludeNameSpec();

            string expected = nameof(Store.Name);
            string actual = spec.IncludeAggregators.FirstOrDefault().IncludeString;

            actual.Should().Be(expected);
        }

        [Fact]
        public void AddsNavigationName_GivenReferenceType()
        {
            var spec = new StoreIncludeAddressSpec();

            string expected = nameof(Store.Address);
            string actual = spec.IncludeAggregators.FirstOrDefault().IncludeString;

            actual.Should().Be(expected);
        }

        [Fact]
        public void AddsNavigationName_GivenCollectionType()
        {
            var spec = new StoreIncludeProductsSpec();

            string expected = nameof(Store.Products);
            string actual = spec.IncludeAggregators.FirstOrDefault().IncludeString;

            actual.Should().Be(expected);
        }

        [Fact]
        public void DoesNotAddNavigationName_GivenExpressionWithDotAppendedNavigations()
        {
            var spec = new StoreIncludeCompanyCountryDotSeparatedSpec();

            string expected = nameof(Company.Country);
            string actual = spec.IncludeAggregators.FirstOrDefault().IncludeString;

            actual.Should().Be(expected);
        }

        [Fact]
        public void DoesNotAddNavigationName_GivenExpressionWithMethod()
        {
            var spec = new StoreIncludeMethodSpec();

            string expected = string.Empty;
            string actual = spec.IncludeAggregators.FirstOrDefault().IncludeString;

            actual.Should().Be(expected);
        }

        [Fact]
        public void DoesNotAddNavigationName_GivenExpressionWithMethodOfNavigation()
        {
            var spec = new StoreIncludeMethodOfNavigationSpec();

            string expected = string.Empty;
            string actual = spec.IncludeAggregators.FirstOrDefault().IncludeString;

            actual.Should().Be(expected);
        }

        [Fact]
        public void DoesNotAddNavigationName_GivenFaultyIncludeExpressions()
        {
            var spec = new StoreWithFaultyIncludeSpec();

            string expected = string.Empty;
            string actual = spec.IncludeAggregators.FirstOrDefault().IncludeString;

            actual.Should().Be(expected);
        }
    }
}
