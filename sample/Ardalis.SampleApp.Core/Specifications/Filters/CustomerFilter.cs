using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.SampleApp.Core.Specifications.Filters
{
  public class CustomerFilter : BaseFilter
  {
    public string Name { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
  }
}
