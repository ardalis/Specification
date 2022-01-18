using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ardalis.SampleApp.Web.Models;

public class CustomerFilterDto : BaseFilterDto
{
  public string Name { get; set; }
  public string Email { get; set; }
  public string Address { get; set; }
}
