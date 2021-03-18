using System;
using FluentAssertions;
using Ardalis.Specification.UnitTests.Fixture.Entities;
using Ardalis.Specification.UnitTests.Fixture.Specs;
using Xunit;

namespace Ardalis.Specification.UnitTests
{
    public class SpecificationEntityEvaluatorTests
    {
        [Fact]
        public void ReturnsTrueForEmptySpec_StoreEmptySpec()
        {
            var store = new Store
            {
                Id = 1
            };
            var evaluation = new StoreEmptySpec().Evaluate(store);

            evaluation.Should().BeTrue();
        }

        [Fact]
        public void ReturnsTrueForSameId_StoreByIdSpec()
        {
            var store = new Store
            {
                Id = 1
            };
            var evaluation = new StoreByIdSpec(1).Evaluate(store);

            evaluation.Should().BeTrue();
        }

        [Fact]
        public void ReturnsFalseForDiffrentId_StoreByIdSpec()
        {
            var store = new Store
            {
                Id = 2
            };
            var evaluation = new StoreByIdSpec(1).Evaluate(store);

            evaluation.Should().BeFalse();
        }

        [Fact]
        public void ReturnsTrueForIdAndName_StoreByIdAndNameSpec()
        {
            var store = new Store
            {
                Id = 1, Name = "name"
            };
            var evaluation = new StoreByIdAndNameSpec(1, "name").Evaluate(store);

            evaluation.Should().BeTrue();
        } 
        
        [Fact]
        public void ReturnsFalseForIdAndDifferentName_StoreByIdAndNameSpec()
        {
            var store = new Store
            {
                Id = 1,
                Name = "different"
            };
            var evaluation = new StoreByIdAndNameSpec(1, "name").Evaluate(store);

            evaluation.Should().BeFalse();
        }

        [Fact]
        public void ReturnsTrueForIdAndIncludeStatements_StoreByIdIncludeAddressAndProductsSpec()
        {
            var store = new Store
            {
                Id = 1
            };
            var evaluation = new StoreByIdIncludeAddressAndProductsSpec(1).Evaluate(store);

            evaluation.Should().BeTrue();
        }

        [Fact]
        public void ReturnsFalseForDifferentIdAndIncludeStatements_StoreByIdIncludeAddressAndProductsSpec()
        {
            var store = new Store
            {
                Id = 2
            };
            var evaluation = new StoreByIdIncludeAddressAndProductsSpec(1).Evaluate(store);

            evaluation.Should().BeFalse();
        }

        [Fact]
        public void ReturnsTrueForCompanyIdAndOrderedStatements_StoresByCompanyOrderedDescByNameThenByDescIdSpec()
        {
            var store = new Store
            {
                CompanyId = 1
            };
            var evaluation = new StoresByCompanyOrderedDescByNameThenByDescIdSpec(1).Evaluate(store);

            evaluation.Should().BeTrue();
        }

        [Fact]
        public void ReturnsFalseForDifferentIdAndIncludeStatements_StoresByCompanyOrderedDescByNameThenByDescIdSpec()
        {
            var store = new Store
            {
                CompanyId = 2
            };
            var evaluation = new StoresByCompanyOrderedDescByNameThenByDescIdSpec(1).Evaluate(store);

            evaluation.Should().BeFalse();
        }

        [Fact]
        public void ReturnsTrueForPaginatedSpec_StoreNamesPaginatedSpec()
        {
            var store = new Store
            {
                Id = 1
            };
            var evaluation = new StoreNamesPaginatedSpec(0, 5).Evaluate(store);
            evaluation.Should().BeTrue();
        }

        [Fact]
        public void ReturnsTrueForCompanyIdAndPaginatedSpec_StoresByCompanyPaginatedSpec()
        {
            var store = new Store
            {
                Id = 1,
                CompanyId = 2
            };
            var evaluation = new StoresByCompanyPaginatedSpec(2,0, 5).Evaluate(store);
            evaluation.Should().BeTrue();
        }

        [Fact]
        public void ReturnsFalseForDifferentCompanyIdAndPaginatedSpec_StoresByCompanyPaginatedSpec()
        {
            var store = new Store
            {
                Id = 1,
                CompanyId = 1
            };
            var evaluation = new StoresByCompanyPaginatedSpec(2, 0, 5).Evaluate(store);
            evaluation.Should().BeFalse();
        }

        [Fact]
        public void ReturnsFalseForWhereAndPaginatedSpec_StoresByCompanyPaginatedSpec()
        {
            var store = new Store
            {
                Id = 1,
                CompanyId = 2
            };
            var evaluation = new StoresByCompanyPaginatedSpec(2, 0, 5).Evaluate(store);
            evaluation.Should().BeTrue();
        }

        [Fact]
        public void ThrowsNotSupportedExceptionForSearchSpec_StoreSearchByNameOrCitySpec()
        {
            var store = new Store
            {
                Name = "spec",
                City = "speccity"
            };
            Assert.Throws<NotSupportedException>(() => new StoreSearchByNameOrCitySpec("speccity").Evaluate(store));
        }
    }
}
