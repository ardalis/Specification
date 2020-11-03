using Ardalis.SampleApp.Core.Entitites.CustomerAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ardalis.SampleApp.Core.Interfaces
{
    public interface ICustomerRepository
    {
        // This is just to demonstrate that at anytime you can create custom repositories, and use to create some complex queries working directly with EF or your ORM.
        Task<List<Customer>> GetCustomers(string addressSearchTerm);
    }
}
