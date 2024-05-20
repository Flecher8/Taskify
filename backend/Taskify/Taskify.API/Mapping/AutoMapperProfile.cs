using AutoMapper;
using Taskify.BLL.Helpers;
using Taskify.Core.DbModels;
using Taskify.Core.Dtos;
using Taskify.Core.Dtos.Statistics;

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
            CreateMap<UpdateProjectRoleDto, ProjectRole>();
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
            // Company Expenses
            CreateMap<CompanyExpense,  CompanyExpenseDto>();
            CreateMap<CompanyExpenseDto, CompanyExpense>();
            CreateMap<CreateCompanyExpenseDto, CompanyExpense>()
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => new Company { Id = src.CompanyId }));
            CreateMap<UpdateCompanyExpenseDto, CompanyExpense>();
            // Company Member Roles
            CreateMap<CompanyRole, CompanyRoleDto>();
            CreateMap<CompanyRoleDto, CompanyRole>();
            CreateMap<CreateCompanyRoleDto, CompanyRole>()
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => new Company { Id = src.CompanyId }));
            CreateMap<UpdateCompanyRoleDto, CompanyRole>();
            // Company Members
            CreateMap<CompanyMember, CompanyMemberDto>();
            CreateMap<CompanyMemberDto, CompanyMember>();
            CreateMap<CreateCompanyMemberDto, CompanyMember>()
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => new Company { Id = src.CompanyId }))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new User { Id = src.UserId }));
            CreateMap<UpdateCompanyMemberDto, CompanyMember>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => new CompanyRole { Id = src.RoleId }));
            // Company Invitations
            CreateMap<CompanyInvitation, CompanyInvitationDto>();
            CreateMap<CompanyInvitationDto, CompanyInvitation>();
            CreateMap<CreateCompanyInvitationDto, CompanyInvitation>();
            // Task Time Tracker
            CreateMap<TaskTimeTracker, TaskTimeTrackerDto>();
            CreateMap<TaskTimeTrackerDto, TaskTimeTracker>();
            CreateMap<CreateTaskTimeTrackerDto, TaskTimeTracker>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new User { Id = src.UserId }))
                .ForMember(dest => dest.CustomTask, opt => opt.MapFrom(src => new CustomTask { Id = src.CustomTaskId }));
            CreateMap<UpdateTaskTimeTracker, TaskTimeTracker>();

            // Statistics
            // ProjectRoleTaskCountStatistics
            CreateMap<ProjectRoleTaskCountStatistics, ProjectRoleTaskCountStatisticsDto>();
            CreateMap<ProjectRoleTaskCountStatisticsDto, ProjectRoleTaskCountStatistics>();
            // SectionTaskCountStatistics
            CreateMap<SectionTaskCountStatistics, SectionTaskCountStatisticsDto>();
            CreateMap<SectionTaskCountStatisticsDto, SectionTaskCountStatistics>();
            // SectionTypeTaskCountStatistics
            CreateMap<SectionTypeTaskCountStatistics, SectionTypeTaskCountStatisticsDto>();
            CreateMap<SectionTypeTaskCountStatisticsDto, SectionTypeTaskCountStatistics>();
            // UserStoryPointsCountStatistics
            CreateMap<UserStoryPointsCountStatistics, UserStoryPointsCountStatisticsDto>();
            CreateMap<UserStoryPointsCountStatisticsDto, UserStoryPointsCountStatistics>();
            // UserTaskCountStatistics
            CreateMap<UserTaskCountStatistics, UserTaskCountStatisticsDto>();
            CreateMap<UserTaskCountStatisticsDto, UserTaskCountStatistics>();
            // UserTimeSpendOnDateRequest
            CreateMap<UserTimeSpendOnDateRequest, UserTimeSpendOnDateRequestDto>();
            CreateMap<UserTimeSpendOnDateRequestDto, UserTimeSpendOnDateRequest>();
            // UserTimeSpendOnDateStatistic
            CreateMap<UserTimeSpendOnDateStatistic, UserTimeSpendOnDateStatisticDto>();
            CreateMap<UserTimeSpendOnDateStatisticDto, UserTimeSpendOnDateStatistic>();
        }
    }
}
