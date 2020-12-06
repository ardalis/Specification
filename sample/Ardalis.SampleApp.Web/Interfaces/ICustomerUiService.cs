using Ardalis.SampleApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ardalis.SampleApp.Web.Interfaces
{
    public interface ICustomerUiService
    {
        Task<CustomerDto> GetCustomer(int customerId);
        Task<CustomerDto> GetCustomer(string customerName);
        Task<CustomerDto> GetCustomerWithStores(string customerName);

        Task<List<CustomerDto>> GetCustomers(CustomerFilterDto filterDto);
    }
}
