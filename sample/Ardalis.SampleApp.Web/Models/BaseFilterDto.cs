using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Ardalis.SampleApp.Web.Models;

public class BaseFilterDto
{
    [Browsable(false)]
    [JsonIgnore]
    public bool LoadChildren { get; set; } = false;

    [Browsable(false)]
    [JsonIgnore]
    public bool IsPagingEnabled { get; set; } = true;

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
