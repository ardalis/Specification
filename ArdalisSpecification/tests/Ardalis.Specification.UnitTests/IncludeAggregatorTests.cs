using Ardalis.Specification.UnitTests.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ardalis.Specification.UnitTests
{
    public class IncludeAggregatorTests
    {
        [Fact]
        public void IncludeAggregator_AddPropertyNameInConstructor_ReturnsCorrectIncludeString()
        {
            var expectedString = nameof(Company);
            var includeAggregator = new IncludeAggregator(expectedString);

            Assert.Equal(expectedString, includeAggregator.IncludeString);
        }

        [Fact]
        public void IncludeAggregator_AddNullInConstructor_ReturnsStringEmpty()
        {
            var expectedString = string.Empty;
            var includeAggregator = new IncludeAggregator(null);

            Assert.Equal(expectedString, includeAggregator.IncludeString);
        }

        [Fact]
        public void IncludeAggregator_AddEmptyStringInConstructor_ReturnsStringEmpty()
        {
            var expectedString = string.Empty;
            var includeAggregator = new IncludeAggregator(null);

            Assert.Equal(expectedString, includeAggregator.IncludeString);
        }

        [Fact]
        public void IncludeAggregator_AddNavigationPropertyName_ReturnsCorrectIncludeString()
        {
            var expectedString = $"{nameof(Company)}.{nameof(Company.Stores)}.{nameof(Store.Products)}";

            var includeAggregator = new IncludeAggregator(nameof(Company));
            includeAggregator.AddNavigationPropertyName(nameof(Company.Stores));
            includeAggregator.AddNavigationPropertyName(nameof(Store.Products));

            Assert.Equal(expectedString, includeAggregator.IncludeString);
        }

        [Fact]
        public void IncludeAggregator_AddNavigationPropertyNameAndNullInConstructor_ReturnsCorrectIncludeString()
        {
            var expectedString = $"{nameof(Company)}.{nameof(Company.Stores)}.{nameof(Store.Products)}";

            var includeAggregator = new IncludeAggregator(null);
            includeAggregator.AddNavigationPropertyName(nameof(Company));
            includeAggregator.AddNavigationPropertyName(nameof(Company.Stores));
            includeAggregator.AddNavigationPropertyName(nameof(Store.Products));

            Assert.Equal(expectedString, includeAggregator.IncludeString);
        }

        [Fact]
        public void IncludeAggregator_AddNavigationPropertyNameAndEmptyStringInConstructor_ReturnsCorrectIncludeString()
        {
            var expectedString = $"{nameof(Company)}.{nameof(Company.Stores)}.{nameof(Store.Products)}";

            var includeAggregator = new IncludeAggregator(string.Empty);
            includeAggregator.AddNavigationPropertyName(nameof(Company));
            includeAggregator.AddNavigationPropertyName(nameof(Company.Stores));
            includeAggregator.AddNavigationPropertyName(nameof(Store.Products));

            Assert.Equal(expectedString, includeAggregator.IncludeString);
        }

        [Fact]
        public void IncludeAggregator_AddNullNavigationPropertyName_ReturnsCorrectIncludeString()
        {
            var expectedString = $"{nameof(Company)}.{nameof(Company.Stores)}.{nameof(Store.Products)}";

            var includeAggregator = new IncludeAggregator(string.Empty);
            includeAggregator.AddNavigationPropertyName(nameof(Company));
            includeAggregator.AddNavigationPropertyName(null);
            includeAggregator.AddNavigationPropertyName(nameof(Company.Stores));
            includeAggregator.AddNavigationPropertyName(nameof(Store.Products));

            Assert.Equal(expectedString, includeAggregator.IncludeString);
        }
    }
}
