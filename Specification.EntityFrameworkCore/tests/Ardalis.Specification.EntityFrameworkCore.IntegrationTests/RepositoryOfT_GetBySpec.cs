using System.Linq;
using System.Threading.Tasks;
using Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Fixture;
using Ardalis.Specification.UnitTests.Fixture.Entities.Seeds;
using Ardalis.Specification.UnitTests.Fixture.Specs;
using FluentAssertions;
using Xunit;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests
{
    public class RepositoryOfT_GetBySpec : RepositoryOfT_GetBySpec_TestKit
    {
        public RepositoryOfT_GetBySpec(SharedDatabaseFixture fixture) : base(fixture, SpecificationEvaluator.Default)
        {
        }
    }

    public class RepositoryOfT_GetBySpec_Cached : RepositoryOfT_GetBySpec_TestKit
    {
        public RepositoryOfT_GetBySpec_Cached(SharedDatabaseFixture fixture) : base(fixture, SpecificationEvaluator.Cached)
        {
        }
    }

    public abstract class RepositoryOfT_GetBySpec_TestKit : IntegrationTestBase
    {
        protected RepositoryOfT_GetBySpec_TestKit(SharedDatabaseFixture fixture, ISpecificationEvaluator specificationEvaluator ) : base(fixture, specificationEvaluator) { }

        [Fact]
        public virtual async Task ReturnsStoreWithProducts_GivenStoreByIdIncludeProductsSpec()
        {
            var result = await storeRepository.GetBySpecAsync(new StoreByIdIncludeProductsSpec(StoreSeed.VALID_STORE_ID));

            result.Should().NotBeNull();
            result!.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
            result.Products.Count.Should().BeGreaterThan(1);
        }

        [Fact]
        public virtual async Task ReturnsStoreWithAddress_GivenStoreByIdIncludeAddressSpec()
        {
            var result = await storeRepository.GetBySpecAsync(new StoreByIdIncludeAddressSpec(StoreSeed.VALID_STORE_ID));

            result.Should().NotBeNull();
            result!.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
            result.Address?.Street.Should().Be(AddressSeed.VALID_STREET_FOR_STOREID1);
        }

        [Fact]
        public virtual async Task ReturnsStoreWithAddressAndProduct_GivenStoreByIdIncludeAddressAndProductsSpec()
        {
            var result = await storeRepository.GetBySpecAsync(new StoreByIdIncludeAddressAndProductsSpec(StoreSeed.VALID_STORE_ID));

            result.Should().NotBeNull();
            result!.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
            result.Products.Count.Should().BeGreaterThan(1);
            result.Address?.Street.Should().Be(AddressSeed.VALID_STREET_FOR_STOREID1);
        }

        [Fact]
        public virtual async Task ReturnsStoreWithProducts_GivenStoreByIdIncludeProductsUsingStringSpec()
        {
            var result = await storeRepository.GetBySpecAsync(new StoreByIdIncludeProductsUsingStringSpec(StoreSeed.VALID_STORE_ID));

            result.Should().NotBeNull();
            result!.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
            result.Products.Count.Should().BeGreaterThan(1);
        }

        [Fact]
        public virtual async Task ReturnsCompanyWithStoresAndAddress_GivenCompanyByIdIncludeStoresThenIncludeAddressSpec()
        {
            var result = await companyRepository.GetBySpecAsync(new CompanyByIdIncludeStoresThenIncludeAddressSpec(CompanySeed.VALID_COMPANY_ID));

            result.Should().NotBeNull();
            result!.Name.Should().Be(CompanySeed.VALID_COMPANY_NAME);
            result.Stores.Count.Should().BeGreaterThan(49);
            result.Stores.Select(x => x.Address).Count().Should().BeGreaterThan(0);
        }

        [Fact]
        public virtual async Task ReturnsCompanyWithStoresAndProducts_GivenCompanyByIdIncludeStoresThenIncludeProductsSpec()
        {
            var result = await companyRepository.GetBySpecAsync(new CompanyByIdIncludeStoresThenIncludeProductsSpec(CompanySeed.VALID_COMPANY_ID));

            result.Should().NotBeNull();
            result!.Name.Should().Be(CompanySeed.VALID_COMPANY_NAME);
            result.Stores.Count.Should().BeGreaterThan(49);
            result.Stores.Select(x => x.Products).Count().Should().BeGreaterThan(1);
        }

        [Fact]
        public virtual async Task ReturnsUntrackedCompany_GivenCompanyByIdAsUntrackedSpec()
        {
            dbContext.ChangeTracker.Clear();

            var result = await companyRepository.GetBySpecAsync(new CompanyByIdAsUntrackedSpec(CompanySeed.VALID_COMPANY_ID));

            result.Should().NotBeNull();
            result?.Name.Should().Be(CompanySeed.VALID_COMPANY_NAME);
            dbContext.Entry(result).State.Should().Be(Microsoft.EntityFrameworkCore.EntityState.Detached);
        }

        [Fact]
        public virtual async Task ReturnsStoreWithCompanyAndCountryAndStoresForCompany_GivenStoreByIdIncludeCompanyAndCountryAndStoresForCompanySpec()
        {
            var result = await storeRepository.GetBySpecAsync(new StoreByIdIncludeCompanyAndCountryAndStoresForCompanySpec(StoreSeed.VALID_STORE_ID));

            result.Should().NotBeNull();
            result!.Name.Should().Be(StoreSeed.VALID_STORE_NAME);
            result.Company.Should().NotBeNull();
            result.Company!.Country.Should().NotBeNull();
            result.Company!.Stores.Should().HaveCountGreaterOrEqualTo(2);
            result.Company?.Stores?.Should().Match(x => x.Any(z => z.Products.Count > 0));
        }
    }
}
