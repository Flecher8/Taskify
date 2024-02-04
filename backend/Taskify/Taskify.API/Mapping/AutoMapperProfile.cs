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
            CreateMap<SectionWithoutTasksDto, Section>();
            CreateMap<Section, SectionWithoutTasksDto>();
            CreateMap<CreateSectionDto,  Section>()
                .ForMember(dest => dest.Project, opt => opt.MapFrom(src => new Project { Id = src.ProjectId }));
            CreateMap<UpdateSectionDto, Section>();
            // Custom tasks
            CreateMap<CustomTask, CustomTaskDto>();
            CreateMap<CustomTaskDto, CustomTask>();
            CreateMap<CreateCustomTaskDto, CustomTask>()
                .ForMember(dest => dest.Section, opt => opt.MapFrom(src => new Section { Id = src.SectionId }));
            CreateMap<UpdateCustomTaskDto, CustomTask>()
                .ForMember(dest => dest.ResponsibleUser, opt => opt.MapFrom(src => new User { Id = src.ResponsibleUserId }));
            // Project roles
            CreateMap<ProjectRoleDto, ProjectRole>();
            CreateMap<ProjectRole, ProjectRoleDto>();
            CreateMap<CreateProjectRoleDto, ProjectRole>()
                .ForMember(dest => dest.Project, opt => opt.MapFrom(src => new Project { Id = src.ProjectId }));
            CreateMap<UpdateProjectRoleDto, ProjectRole>()
                .ForMember(dest => dest.Project, opt => opt.MapFrom(src => new Project { Id = src.ProjectId }));
            // Project members
            CreateMap<ProjectMemberDto, ProjectMember>();
            CreateMap<ProjectMember, ProjectMemberDto>();
            CreateMap<CreateProjectMemberDto, ProjectMember>()
                .ForMember(dest => dest.Project, opt => opt.MapFrom(src => new Project { Id = src.ProjectId }))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new User { Id = src.UserId }));
            CreateMap<UpdateProjectMemberDto, ProjectMember>()
                .ForMember(dest => dest.ProjectRole, opt => opt.MapFrom(src => new ProjectRole { Id = src.ProjectRoleId }));
            // Notifications
            CreateMap<Notification, NotificationDto>();
            CreateMap<NotificationDto, Notification>();
            CreateMap<CreateNotificationDto, Notification>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new User { Id = src.UserId }));
            // Project Invitations
            CreateMap<ProjectInvitation, ProjectInvitationDto>();
            CreateMap<ProjectInvitationDto, ProjectInvitation>();
            CreateMap<CreateProjectInvitationDto, ProjectInvitation>()
                .ForMember(dest => dest.Project, opt => opt.MapFrom(src => new Project { Id = src.ProjectId }));
            // Companies
            CreateMap<Company,  CompanyDto>();
            CreateMap<CompanyDto, Company>();
            CreateMap<CreateCompanyDto, Company>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new User { Id = src.UserId }));
            CreateMap<UpdateCompanyDto, Company>();
        }
    }
}
