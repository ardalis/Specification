using Ardalis.GuardClauses;
using Ardalis.SampleApp.Core.Entities.CustomerAggregate;
using Ardalis.SampleApp.Core.Interfaces;
using Ardalis.SampleApp.Core.Specifications;
using Ardalis.SampleApp.Core.Specifications.Filters;
using Ardalis.SampleApp.Web.Interfaces;
using Ardalis.SampleApp.Web.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ardalis.SampleApp.Web.Services;

public class CustomerUiService : ICustomerUiService
{
    private readonly IMapper _mapper;
    private readonly IReadRepository<Customer> _customerRepository;

    public CustomerUiService(IMapper mapper,
                             IReadRepository<Customer> customerRepository)
    {
        _mapper = mapper;
        _customerRepository = customerRepository;
    }


    // Here I'm just writing various usages, not necessarily you'll need all of them.

    public async Task<CustomerDto> GetCustomer(int customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);

        Guard.Against.Null(customer, nameof(customer));

        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<CustomerDto> GetCustomer(string customerName)
    {
        var customer = await _customerRepository.GetBySpecAsync(new CustomerByNameSpec(customerName));

        Guard.Against.Null(customer, nameof(customer));

        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<CustomerDto> GetCustomerWithStores(string customerName)
    {
        var customer = await _customerRepository.GetBySpecAsync(new CustomerByNameWithStoresSpec(customerName));

        Guard.Against.Null(customer, nameof(customer));

        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<List<CustomerDto>> GetCustomers(CustomerFilterDto filterDto)
    {
        var spec = new CustomerSpec(_mapper.Map<CustomerFilter>(filterDto));
        var customers = await _customerRepository.ListAsync(spec);

        return _mapper.Map<List<CustomerDto>>(customers);
    }
}
