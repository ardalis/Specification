using Ardalis.Specification.UnitTests.Entities;
using Ardalis.Specification.UnitTests.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Ardalis.Specification.UnitTests
{
    public class IncludeExpressionTests
    {
        [Fact]
        public void ShouldGetCorrectPropertyName_ForExpressionWithSimpleType()
        {
            var spec = new StoreIncludeNameSpec();

            string expeted = nameof(Store.Name);
            string actual = spec.IncludeAggregators.FirstOrDefault().IncludeString;

            Assert.Equal(expeted, actual);
        }

        [Fact]
        public void ShouldGetCorrectPropertyName_ForExpressionWithReferenceType()
        {
            var spec = new StoreIncludeAddressSpec();

            string expeted = nameof(Store.Address);
            string actual = spec.IncludeAggregators.FirstOrDefault().IncludeString;

            Assert.Equal(expeted, actual);
        }

        [Fact]
        public void ShouldGetCorrectPropertyName_ForExpressionWithCollection()
        {
            var spec = new StoreIncludeProductsSpec();

            string expeted = nameof(Store.Products);
            string actual = spec.IncludeAggregators.FirstOrDefault().IncludeString;

            Assert.Equal(expeted, actual);
        }

        [Fact]
        public void ShouldGetEmptyString_ForExpressionWithDotAppendedNavigations()
        {
            var spec = new StoreIncludeMethodSpec();

            string expeted = string.Empty;
            string actual = spec.IncludeAggregators.FirstOrDefault().IncludeString;

            Assert.Equal(expeted, actual);
        }

        [Fact]
        public void ShouldGetEmptyString_ForExpressionWithMethod()
        {
            var spec = new StoreIncludeMethodSpec();

            string expeted = string.Empty;
            string actual = spec.IncludeAggregators.FirstOrDefault().IncludeString;

            Assert.Equal(expeted, actual);
        }

        [Fact]
        public void ShouldGetEmptyString_ForExpressionWithMethodOfNavigation()
        {
            var spec = new StoreIncludeMethodOfNavigationSpec();

            string expeted = string.Empty;
            string actual = spec.IncludeAggregators.FirstOrDefault().IncludeString;

            Assert.Equal(expeted, actual);
        }

        [Fact]
        public void ShouldGetEmptyString_ForFaultyIncludeExpressions()
        {
            var spec = new StoreWithFaultyIncludeSpec();

            string expeted = string.Empty;
            string actual = spec.IncludeAggregators.FirstOrDefault().IncludeString;

            Assert.Equal(expeted, actual);
        }
    }
}
