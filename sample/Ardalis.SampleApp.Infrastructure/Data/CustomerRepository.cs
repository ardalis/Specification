using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.SampleApp.Core.Entities.CustomerAggregate;
using Ardalis.SampleApp.Core.Interfaces;
using Ardalis.SampleApp.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Ardalis.SampleApp.Infrastructure.Data;

// This is just to demonstrate that at anytime you can create custom repositories, and use to create some complex queries working directly with EF or your ORM.
public class CustomerRepository : ICustomerRepository
{
  private readonly SampleDbContext dbContext;

  public CustomerRepository(SampleDbContext dbContext)
  {
    this.dbContext = dbContext;
  }

  public Task<List<Customer>> GetCustomers(string addressSearchTerm)
  {
    return dbContext.Customers
        .Take(10)
        .Where(x => EF.Functions.Like(x.Address, "%" + addressSearchTerm + "%"))
        .ToListAsync();
  }
}
