using AutoMapper;
using Ardalis.SampleApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.SampleApp.Core.Specifications.Filters;
using Ardalis.SampleApp.Core.Entitites.CustomerAggregate;

namespace Ardalis.SampleApp.Web
{
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
}
