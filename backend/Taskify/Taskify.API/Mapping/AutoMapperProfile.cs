using AutoMapper;
using Taskify.Core.DbModels;
using Taskify.Core.Dtos;

namespace Taskify.API.Mapping
{
    public class AutoMapperProfile : Profile    
    {
        public AutoMapperProfile() 
        {
            CreateMap<Subscription, SubscriptionDto>();
            CreateMap<SubscriptionDto, Subscription>();
            CreateMap<CreateSubscriptionDto, Subscription>();

            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
        }
    }
}
