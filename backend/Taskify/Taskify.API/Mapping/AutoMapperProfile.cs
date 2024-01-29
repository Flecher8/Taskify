using AutoMapper;
using Taskify.Core.DbModels;
using Taskify.Core.Dtos;

namespace Taskify.API.Mapping
{
    public class AutoMapperProfile : Profile    
    {
        public AutoMapperProfile() 
        {
            // Subscrioptions
            CreateMap<Subscription, SubscriptionDto>();
            CreateMap<SubscriptionDto, Subscription>();
            CreateMap<CreateSubscriptionDto, Subscription>();
            // Users
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
            // Projects
            CreateMap<Project,  ProjectDto>();
            CreateMap<ProjectDto, Project>();
            CreateMap<CreateProjectDto, Project>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new User { Id = src.UserId }));
            CreateMap<UpdateProjectDto, Project>();
            // Project Incomes
            CreateMap<ProjectIncome, ProjectIncomeDto>();
            CreateMap<ProjectIncomeDto, ProjectIncome>();
            CreateMap<CreateProjectIncomeDto, ProjectIncome>()
                .ForMember(dest => dest.Project, opt => opt.MapFrom(src => new Project { Id = src.ProjectId }));
            CreateMap<UpdateProjectIncomeDto, ProjectIncome>();
            // Sections
            CreateMap<Section, SectionDto>();
            CreateMap<SectionDto, Section>();
            CreateMap<CreateSectionDto,  Section>()
                .ForMember(dest => dest.Project, opt => opt.MapFrom(src => new Project { Id = src.ProjectId }));
            CreateMap<UpdateSectionDto, Section>();

        }
    }
}
