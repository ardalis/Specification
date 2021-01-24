using Microsoft.AspNetCore.Mvc;
using Ardalis.SampleApp.Web.Interfaces;
using Ardalis.SampleApp.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ardalis.SampleApp.Web.Controllers
{
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
        public async Task<CustomerDto> Get(int Id)
        {
            return await customerUiService.GetCustomer(Id);
        }

        [HttpGet("{name}")]
        public async Task<CustomerDto> Get(string name)
        {
            return await customerUiService.GetCustomer(name);
        }

        [HttpGet]
        public async Task<List<CustomerDto>> Get([FromQuery] CustomerFilterDto filter)
        {
            filter = filter ?? new CustomerFilterDto();

            // Here you can decide if you want the collections as well

            filter.LoadChildren = true;
            filter.IsPagingEnabled = true;

            return await customerUiService.GetCustomers(filter);
        }
    }
}
