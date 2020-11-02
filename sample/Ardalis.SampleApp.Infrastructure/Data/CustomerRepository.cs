using Ardalis.SampleApp.Core.Entitites.CustomerAggregate;
using Ardalis.SampleApp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Ardalis.SampleApp.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardalis.SampleApp.Infrastructure.Data
{
    // This is just to demonstrate that at anytime you can create custom repositories, and use to create some complex queries working directly with EF or your ORM.
    public class CustomerRepository : ICustomerRepository
    {
        private readonly SampleDbContext dbContext;

        public CustomerRepository(SampleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Customer>> GetCustomers(string addressSearchTerm)
        {
            return await dbContext.Customers
                                .Take(10)
                                .Where(x => EF.Functions.Like(x.Address, "%" + addressSearchTerm + "%"))
                                .ToListAsync();
        }
    }
}
