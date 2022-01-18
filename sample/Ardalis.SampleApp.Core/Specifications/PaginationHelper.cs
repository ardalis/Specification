using Ardalis.SampleApp.Core.Specifications.Filters;

namespace Ardalis.SampleApp.Core.Specifications
{
  public static class PaginationHelper
  {
    public static int DefaultPage => 1;
    public static int DefaultPageSize => 10;

    public static int CalculateTake(int pageSize)
    {
      return pageSize <= 0 ? DefaultPageSize : pageSize;
    }
    public static int CalculateSkip(int pageSize, int page)
    {
      page = page <= 0 ? DefaultPage : page;

      return CalculateTake(pageSize) * (page - 1);
    }

    public static int CalculateTake(BaseFilter baseFilter)
    {
      return CalculateTake(baseFilter.PageSize);
    }
    public static int CalculateSkip(BaseFilter baseFilter)
    {
      return CalculateSkip(baseFilter.PageSize, baseFilter.Page);
    }
  }
}
