using Ardalis.Specification.EF.IntegrationTests.Data.Seeds;
using Ardalis.Specification.EF.IntegrationTests.Specs;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ardalis.Specification.EF.IntegrationTests
{
    public class SpecificationTests : SpecificationTestBase
    {

        [Fact]
        public async Task GetStoreUsingRepositoryAnd_StoreWithProductsSpec_ShouldIncludeProducts()
        {
            var result = (await storeRepository.ListAsync(new StoreWithProductsSpec(StoreSeed.VALID_STORE_ID))).SingleOrDefault();

            result.Should().NotBeNull();
            result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
            result.Products.Count.Should().BeGreaterThan(1);
        }

        [Fact]
        public async Task GetStoreUsingRepositoryAnd_StoreWithAddressSpec_ShouldIncludeAddress()
        {
            var result = (await storeRepository.ListAsync(new StoreWithAddressSpec(StoreSeed.VALID_STORE_ID))).SingleOrDefault();

            result.Should().NotBeNull();
            result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
            result.Address?.Street.Should().Be(AddressSeed.VALID_STREET_FOR_STOREID1);
        }

        [Fact]
        public async Task GetStoreUsingRepositoryAnd_StoreWithAddressAndProductsSpec_ShouldIncludeAddressAndProducts()
        {
            var result = (await storeRepository.ListAsync(new StoreWithAddressAndProductsSpec(StoreSeed.VALID_STORE_ID))).SingleOrDefault();

            result.Should().NotBeNull();
            result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
            result.Products.Count.Should().BeGreaterThan(1);
            result.Address?.Street.Should().Be(AddressSeed.VALID_STREET_FOR_STOREID1);
        }

        [Fact]
        public async Task GetStoreUsingRepositoryAnd_StoreWithProductsUsingStringSpec_ShouldIncludeProducts()
        {
            var result = (await storeRepository.ListAsync(new StoreWithProductsUsingStringSpec(StoreSeed.VALID_STORE_ID))).SingleOrDefault();

            result.Should().NotBeNull();
            result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
            result.Products.Count.Should().BeGreaterThan(1);
        }

        [Fact]
        public async Task GetCompanyUsingRepositoryAnd_CompanyWithStoresThenIncludeAddressSpec_ShouldIncludeStoresAndAddress()
        {
            var result = (await companyRepository.ListAsync(new CompanyWithStoresThenIncludeAddressSpec(CompanySeed.VALID_COMPANY_ID))).SingleOrDefault();

            result.Should().NotBeNull();
            result.Name.Should().Be(CompanySeed.VALID_COMPANY_NAME);
            result.Stores.Count.Should().BeGreaterThan(49);
            result.Stores.Select(x => x.Address).Count().Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetCompanyUsingRepositoryAnd_CompanyWithStoresThenIncludeProductsSpec_ShouldIncludeStoresAndProducts()
        {
            var result = (await companyRepository.ListAsync(new CompanyWithStoresThenIncludeProductsSpec(CompanySeed.VALID_COMPANY_ID))).SingleOrDefault();

            result.Should().NotBeNull();
            result.Name.Should().Be(CompanySeed.VALID_COMPANY_NAME);
            result.Stores.Count.Should().BeGreaterThan(49);
            result.Stores.Select(x => x.Products).Count().Should().BeGreaterThan(1);
        }
    }
}
