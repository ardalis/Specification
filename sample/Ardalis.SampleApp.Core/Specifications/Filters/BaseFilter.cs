using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.SampleApp.Core.Specifications.Filters
{
  public class BaseFilter
  {
    public bool LoadChildren { get; set; }
    public bool IsPagingEnabled { get; set; }

    public int Page { get; set; }
    public int PageSize { get; set; }
  }
}
