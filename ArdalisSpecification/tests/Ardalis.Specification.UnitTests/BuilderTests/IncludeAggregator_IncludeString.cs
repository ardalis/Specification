using FluentAssertions;
using Ardalis.Specification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ardalis.Specification.UnitTests
{
    public class IncludeAggregator_IncludeString
    {
        [Fact]
        public void ReturnsCorrectPath_GivenClassNameInConstructor()
        {
            var expectedString = nameof(Company);
            var includeAggregator = new IncludeAggregator(expectedString);

            includeAggregator.IncludeString.Should().Be(expectedString);
        }

        [Fact]
        public void ReturnsEmptyString_GivenNullInConstructor()
        {
            var expectedString = string.Empty;
            var includeAggregator = new IncludeAggregator(null);

            includeAggregator.IncludeString.Should().Be(expectedString);
        }

        [Fact]
        public void ReturnsEmptyEmpty_GivenEmptyStringInConstructor()
        {
            var expectedString = string.Empty;
            var includeAggregator = new IncludeAggregator(null);

            includeAggregator.IncludeString.Should().Be(expectedString);
        }

        [Fact]
        public void ReturnsCorrectPath_GivenPropertyNamesAndClassNameInConstructor()
        {
            var expectedString = $"{nameof(Company)}.{nameof(Company.Stores)}.{nameof(Store.Products)}";

            var includeAggregator = new IncludeAggregator(nameof(Company));
            includeAggregator.AddNavigationPropertyName(nameof(Company.Stores));
            includeAggregator.AddNavigationPropertyName(nameof(Store.Products));

            includeAggregator.IncludeString.Should().Be(expectedString);
        }

        [Fact]
        public void ReturnsCorrectPath_GivenPropertyNamesAndNullInConstructor()
        {
            var expectedString = $"{nameof(Company)}.{nameof(Company.Stores)}.{nameof(Store.Products)}";

            var includeAggregator = new IncludeAggregator(null);
            includeAggregator.AddNavigationPropertyName(nameof(Company));
            includeAggregator.AddNavigationPropertyName(nameof(Company.Stores));
            includeAggregator.AddNavigationPropertyName(nameof(Store.Products));

            includeAggregator.IncludeString.Should().Be(expectedString);
        }

        [Fact]
        public void ReturnsCorrectPath_GivenPropertyNamesAndEmptyStringInConstructor()
        {
            var expectedString = $"{nameof(Company)}.{nameof(Company.Stores)}.{nameof(Store.Products)}";

            var includeAggregator = new IncludeAggregator(string.Empty);
            includeAggregator.AddNavigationPropertyName(nameof(Company));
            includeAggregator.AddNavigationPropertyName(nameof(Company.Stores));
            includeAggregator.AddNavigationPropertyName(nameof(Store.Products));

            includeAggregator.IncludeString.Should().Be(expectedString);
        }

        [Fact]
        public void ReturnsCorrectPath_GivenNullAndEmptyStringInConstructor()
        {
            var expectedString = $"{nameof(Company)}.{nameof(Company.Stores)}.{nameof(Store.Products)}";

            var includeAggregator = new IncludeAggregator(string.Empty);
            includeAggregator.AddNavigationPropertyName(nameof(Company));
            includeAggregator.AddNavigationPropertyName(null);
            includeAggregator.AddNavigationPropertyName(nameof(Company.Stores));
            includeAggregator.AddNavigationPropertyName(nameof(Store.Products));

            includeAggregator.IncludeString.Should().Be(expectedString);
        }
    }
}
