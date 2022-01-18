using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ardalis.SampleApp.Web.Models;

public class CustomerDto
{
  public int Id { get; set; }
  public string Name { get; set; }
  public string Email { get; set; }
  public string Address { get; set; }

  public List<StoreDto> Stores { get; set; } = new List<StoreDto>();
}
