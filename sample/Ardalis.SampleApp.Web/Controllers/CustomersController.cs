using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.SampleApp.Web.Interfaces;
using Ardalis.SampleApp.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ardalis.SampleApp.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
  private readonly ICustomerUiService customerUiService;

  public CustomersController(ICustomerUiService customerUiService)
  {
    this.customerUiService = customerUiService;
  }

  [HttpGet("{Id}")]
  public Task<CustomerDto> Get(int Id)
  {
    return customerUiService.GetCustomer(Id);
  }

  [HttpGet("{name}")]
  public Task<CustomerDto> Get(string name)
  {
    return customerUiService.GetCustomer(name);
  }

  [HttpGet]
  public Task<List<CustomerDto>> Get([FromQuery] CustomerFilterDto filter)
  {
    filter = filter ?? new CustomerFilterDto();

    // Here you can decide if you want the collections as well

    filter.LoadChildren = true;
    filter.IsPagingEnabled = true;

    return customerUiService.GetCustomers(filter);
  }
}
