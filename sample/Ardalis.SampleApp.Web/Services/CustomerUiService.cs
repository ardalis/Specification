using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Ardalis.SampleApp.Core.Entities.CustomerAggregate;
using Ardalis.SampleApp.Core.Interfaces;
using Ardalis.SampleApp.Core.Specifications;
using Ardalis.SampleApp.Core.Specifications.Filters;
using Ardalis.SampleApp.Web.Interfaces;
using Ardalis.SampleApp.Web.Models;
using AutoMapper;

namespace Ardalis.SampleApp.Web.Services;

public class CustomerUiService : ICustomerUiService
{
  private readonly IMapper mapper;
  private readonly IReadRepository<Customer> customerRepository;

  public CustomerUiService(IMapper mapper,
                           IReadRepository<Customer> customerRepository)
  {
    this.mapper = mapper;
    this.customerRepository = customerRepository;
  }


  // Here I'm just writing various usages, not necessarily you'll need all of them.

  public async Task<CustomerDto> GetCustomer(int customerId)
  {
    var customer = await customerRepository.GetByIdAsync(customerId);

    Guard.Against.Null(customer, nameof(customer));

    return mapper.Map<CustomerDto>(customer);
  }

  public async Task<CustomerDto> GetCustomer(string customerName)
  {
    var customer = await customerRepository.GetBySpecAsync(new CustomerByNameSpec(customerName));

    Guard.Against.Null(customer, nameof(customer));

    return mapper.Map<CustomerDto>(customer);
  }

  public async Task<CustomerDto> GetCustomerWithStores(string customerName)
  {
    var customer = await customerRepository.GetBySpecAsync(new CustomerByNameWithStoresSpec(customerName));

    Guard.Against.Null(customer, nameof(customer));

    return mapper.Map<CustomerDto>(customer);
  }

  public async Task<List<CustomerDto>> GetCustomers(CustomerFilterDto filterDto)
  {
    var spec = new CustomerSpec(mapper.Map<CustomerFilter>(filterDto));
    var customers = await customerRepository.ListAsync(spec);

    return mapper.Map<List<CustomerDto>>(customers);
  }
}
