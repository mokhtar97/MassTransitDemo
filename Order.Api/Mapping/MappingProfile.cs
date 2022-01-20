using AutoMapper;
using MassTransit.Messages.Models.Events;
using Order.Api.Models;

namespace Ownership.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Orders, OrderCreatedEvent>()
                 .ForMember(r => r.OrderId, o => o.MapFrom(y => y.Id))
                 .ReverseMap();
          
         
        }

     
       

       
       

       
        
     
    }
}
