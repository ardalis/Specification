using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.SampleApp.Core.Entities.CustomerAggregate;
using Ardalis.SampleApp.Core.Specifications.Filters;
using Ardalis.SampleApp.Web.Models;
using AutoMapper;


namespace Ardalis.SampleApp.Web;

public class AutomapperMaps : Profile
{
  public AutomapperMaps()
  {
    CreateMap<BaseFilterDto, BaseFilter>().IncludeAllDerived().ReverseMap();
    CreateMap<CustomerFilterDto, CustomerFilter>().ReverseMap();

    CreateMap<Customer, CustomerDto>();
    CreateMap<Store, StoreDto>();
  }
}
