namespace Ardalis.SampleApp.Web.Models;

public class CustomerFilterDto : BaseFilterDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
}
