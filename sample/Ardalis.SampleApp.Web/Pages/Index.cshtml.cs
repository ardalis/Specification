using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Ardalis.SampleApp.Core.Entities.CustomerAggregate;
using Ardalis.SampleApp.Core.Interfaces;
using Ardalis.SampleApp.Core.Specifications;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Ardalis.SampleApp.Web.Pages;

public class IndexModel : PageModel
{
  private readonly IReadRepository<Customer> _customerRepository;
  private readonly ILogger<IndexModel> _logger;

  public IndexModel(IReadRepository<Customer> customerRepository,
      ILogger<IndexModel> logger)
  {
    _customerRepository = customerRepository;
    _logger = logger;
  }

  public List<Customer> Customers { get; set; }
  public long ElapsedTimeMilliseconds { get; set; }

  public async Task OnGet()
  {
    var timer = Stopwatch.StartNew();
    var spec = new CustomerByNameWithStoresSpec(name: "Customer66");
    Customers = await _customerRepository.ListAsync(spec);
    timer.Stop();
    ElapsedTimeMilliseconds = timer.ElapsedMilliseconds;
  }
}
