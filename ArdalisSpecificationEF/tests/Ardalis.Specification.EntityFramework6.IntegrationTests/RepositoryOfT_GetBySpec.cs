using FluentAssertions;
using Ardalis.Specification.EntityFramework6.IntegrationTests.Fixture;
using Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Ardalis.Specification.UnitTests.Fixture.Specs;
using System.Data.Entity;

namespace Ardalis.Specification.EntityFramework6.IntegrationTests
{
    public class RepositoryOfT_GetBySpec : IntegrationTestBase
    {
        public RepositoryOfT_GetBySpec(SharedDatabaseFixture fixture) : base(fixture) { }

        [Fact]
        public async Task ReturnsStoreWithProducts_GivenStoreByIdIncludeProductsSpec()
        {
            var result = await storeRepository.GetBySpecAsync(new StoreByIdIncludeProductsSpec(StoreSeed.VALID_STORE_ID));

            result.Should().NotBeNull();
            result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
            result.Products.Count.Should().BeGreaterThan(1);
        }

        [Fact]
        public async Task ReturnsStoreWithAddress_GivenStoreByIdIncludeAddressSpec()
        {
            var result = await storeRepository.GetBySpecAsync(new StoreByIdIncludeAddressSpec(StoreSeed.VALID_STORE_ID));

            result.Should().NotBeNull();
            result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
            result.Address?.Street.Should().Be(AddressSeed.VALID_STREET_FOR_STOREID1);
        }

        [Fact]
        public async Task ReturnsStoreWithAddressAndProduct_GivenStoreByIdIncludeAddressAndProductsSpec()
        {
            var result = await storeRepository.GetBySpecAsync(new StoreByIdIncludeAddressAndProductsSpec(StoreSeed.VALID_STORE_ID));

            result.Should().NotBeNull();
            result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
            result.Products.Count.Should().BeGreaterThan(1);
            result.Address?.Street.Should().Be(AddressSeed.VALID_STREET_FOR_STOREID1);
        }

        [Fact]
        public async Task ReturnsStoreWithProducts_GivenStoreByIdIncludeProductsUsingStringSpec()
        {
            var result = await storeRepository.GetBySpecAsync(new StoreByIdIncludeProductsUsingStringSpec(StoreSeed.VALID_STORE_ID));

            result.Should().NotBeNull();
            result.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
            result.Products.Count.Should().BeGreaterThan(1);
        }

        [Fact]
        public async Task ReturnsCompanyWithStoresAndAddress_GivenCompanyByIdIncludeStoresThenIncludeAddressSpec()
        {
            var result = await companyRepository.GetBySpecAsync(new CompanyByIdIncludeStoresThenIncludeAddressSpec(CompanySeed.VALID_COMPANY_ID));

            result.Should().NotBeNull();
            result.Name.Should().Be(CompanySeed.VALID_COMPANY_NAME);
            result.Stores.Count.Should().BeGreaterThan(49);
            result.Stores.Select(x => x.Address).Count().Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task ReturnsCompanyWithStoresAndProducts_GivenCompanyByIdIncludeStoresThenIncludeProductsSpec()
        {
            var result = await companyRepository.GetBySpecAsync(new CompanyByIdIncludeStoresThenIncludeProductsSpec(CompanySeed.VALID_COMPANY_ID));

            result.Should().NotBeNull();
            result.Name.Should().Be(CompanySeed.VALID_COMPANY_NAME);
            result.Stores.Count.Should().BeGreaterThan(49);
            result.Stores.Select(x => x.Products).Count().Should().BeGreaterThan(1);
        }

        [Fact]
        public async Task ReturnsUntrackedCompany_GivenCompanyByIdAsUntrackedSpec()
        {
            //dbContext.ChangeTracker.Clear();

            var result = await companyRepository.GetBySpecAsync(new CompanyByIdAsUntrackedSpec(CompanySeed.VALID_COMPANY_ID));

            result.Should().NotBeNull();
            result?.Name.Should().Be(CompanySeed.VALID_COMPANY_NAME);
            dbContext.Entry(result).State.Should().Be(EntityState.Detached);
        }
    }
}