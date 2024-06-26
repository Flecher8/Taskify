
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using System.Text.Json.Serialization;
using Taskify.BLL.Interfaces;
using Taskify.BLL.Services;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;
using Taskify.DAL;
using Taskify.DAL.Interfaces;
using Taskify.DAL.Repositories;

namespace Taskify.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Repositories
            builder.Services.AddScoped<ICompanyExpenseRepository, CompanyExpenseRepository>();
            builder.Services.AddScoped<ICompanyInvitationRepository, CompanyInvitationRepository>();
            builder.Services.AddScoped<ICompanyMemberRepository, CompanyMemberRepository>();
            builder.Services.AddScoped<ICompanyRoleRepository, CompanyRoleRepository>();
            builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
            builder.Services.AddScoped<ICustomTaskRepository, CustomTaskRepository>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<IProjectIncomeRepository, ProjectIncomeRepository>();
            builder.Services.AddScoped<IProjectInvitationRepository, ProjectInvitationRepository>();
            builder.Services.AddScoped<IProjectMemberRepository, ProjectMemberRepository>();
            builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
            builder.Services.AddScoped<IProjectRoleRepository, ProjectRoleRepository>();
            builder.Services.AddScoped<ISectionRepository, SectionRepository>();
            builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            builder.Services.AddScoped<ITaskTimeTrackerRepository, TaskTimeTrackerRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserSubscriptionRepository, UserSubscriptionRepository>();

            // Validators
            builder.Services.AddScoped<IValidator<Subscription>, SubscriptionValidator>();
            builder.Services.AddScoped<IValidator<User>, UserValidator>();
            builder.Services.AddScoped<IValidator<Project>, ProjectValidator>();
            builder.Services.AddScoped<IValidator<ProjectIncome>, ProjectIncomeValidator>();
            builder.Services.AddScoped<IValidator<Section>, SectionValidator>();
            builder.Services.AddScoped<IValidator<CustomTask>, CustomTaskValidator>();
            builder.Services.AddScoped<IValidator<ProjectRole>, ProjectRoleValidator>();
            builder.Services.AddScoped<IValidator<ProjectMember>, ProjectMemberValidator>();
            builder.Services.AddScoped<IValidator<Notification>, NotificationValidator>();
            builder.Services.AddScoped<IValidator<ProjectInvitation>, ProjectInvitationValidator>();
            builder.Services.AddScoped<IValidator<Company>, CompanyValidator>();
            builder.Services.AddScoped<IValidator<CompanyExpense>, CompanyExpenseValidator>();
            builder.Services.AddScoped<IValidator<CompanyRole>, CompanyRoleValidator>();
            builder.Services.AddScoped<IValidator<CompanyMember>, CompanyMemberValidator>();
            builder.Services.AddScoped<IValidator<CompanyInvitation>, CompanyInvitationValidator>();
            builder.Services.AddScoped<IValidator<TaskTimeTracker>, TaskTimeTrackerValidator>();

            // Services
            builder.Services.AddScoped<IBudgetService, BudgetService>();
            builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
            builder.Services.AddScoped<IUserSubscriptionService, UserSubscriptionService>();
            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<IProjectsService, ProjectsService>();
            builder.Services.AddScoped<IProjectIncomesService, ProjectIncomesService>();
            builder.Services.AddScoped<ISectionsService, SectionsService>();
            builder.Services.AddScoped<ICustomTasksService, CustomTasksService>();
            builder.Services.AddScoped<IProjectRolesService, ProjectRolesService>();
            builder.Services.AddScoped<IProjectMembersService, ProjectMembersService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IProjectInvitationsService, ProjectInvitationsService>();
            builder.Services.AddScoped<ICompaniesService, CompaniesService>();
            builder.Services.AddScoped<ICompanyExpensesService, CompanyExpensesService>();
            builder.Services.AddScoped<ICompanyRolesService, CompanyRolesService>();
            builder.Services.AddScoped<ICompanyMembersService, CompanyMembersService>();
            builder.Services.AddScoped<ICompanyInvitationsService, CompanyInvitationsService>();
            builder.Services.AddScoped<ITaskStatisticsService, TaskStatisticsService>();
            builder.Services.AddScoped<ITaskTimeTrackersService, TaskTimeTrackersService>();
            builder.Services.AddScoped<ITimeStatisticsService, TimeStatisticsService>();
            builder.Services.AddScoped<IUserPermissionService, UserPermissionService>();
            builder.Services.AddScoped<IProjectPermissionService, ProjectPermissionService>();
            builder.Services.AddScoped<IKpiStatisticsService, KpiStatisticsService>();



            // Add Identity services
            //builder.Services.AddIdentity<User, IdentityRole>()
            //    .AddEntityFrameworkStores<DataContext>()
            //    .AddDefaultTokenProviders();

            builder.Services.AddIdentityApiEndpoints<User>()
                .AddEntityFrameworkStores<DataContext>();
            //builder.Services.AddControllers();
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            }); ;
            // Can be added to remove json cycles
            //.AddJsonOptions(options =>
            // {
            //     options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            // });
            //builder.Services.AddControllers().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            //});
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme (\"Bearer {token}\")",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                option.OperationFilter<SecurityRequirementsOperationFilter>();
            });
            // AutoMapper
            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            // DB
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Taskify.DAL"));

                // Enable sensitive data logging
                options.EnableSensitiveDataLogging();
            });

            // Enable CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapIdentityApi<User>();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            // Cors
            app.UseCors("AllowAllHeaders");

            app.Run();
        }
    }
}
