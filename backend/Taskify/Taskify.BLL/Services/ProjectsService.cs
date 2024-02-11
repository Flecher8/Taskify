using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Interfaces;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;
using Taskify.Core.Enums;
using Taskify.Core.Result;
using Taskify.DAL.Interfaces;
using Taskify.DAL.Repositories;

namespace Taskify.BLL.Services
{
    public class ProjectsService : IProjectsService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUsersService _userService;
        private readonly ISectionRepository _sectionRepository;
        private readonly IProjectInvitationRepository _projectInvitationRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IProjectRoleRepository _projectRoleRepository;
        private readonly IValidator<Project> _validator;
        private readonly ILogger<ProjectsService> _logger;

        public ProjectsService(IProjectRepository projectRepository,
            IUsersService userService,
            ISectionRepository sectionRepository,
            IProjectInvitationRepository projectInvitationRepository,
            INotificationRepository notificationRepository,
            IProjectRoleRepository projectRoleRepository,
            IValidator<Project> validator,
            ILogger<ProjectsService> logger
        )
        {
            _projectRepository = projectRepository;
            _userService = userService;
            _sectionRepository = sectionRepository;
            _projectInvitationRepository = projectInvitationRepository;
            _notificationRepository = notificationRepository;
            _projectRoleRepository = projectRoleRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Project>> CreateProjectAsync(Project project)
        {
            try
            {
                if (project.User == null)
                {
                    return ResultFactory.Failure<Project>("Can not find such user id.");
                }

                var userResult = await _userService.GetUserByIdAsync(project.User.Id);

                if (!userResult.IsSuccess) 
                { 
                    return ResultFactory.Failure<Project>(userResult.Errors);
                }

                // Validation
                var validation = await _validator.ValidateAsync(project);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<Project>(validation.ErrorMessages);
                }

                project.User = userResult.Data;
                project.CreatedAt = DateTime.UtcNow;

                var result = await _projectRepository.AddAsync(project);

                // Template for standart sections in project
                // TO DO
                Section todo = new Section();
                todo.Name = "To do";
                todo.Project = result;
                todo.CreatedAt = DateTime.UtcNow;
                todo.SectionType = SectionType.ToDo;
                todo.SequenceNumber = 0;
                todo.IsArchived = false;
                // Doing
                Section doing = new Section();
                doing.Name = "Doing";
                doing.Project = result;
                doing.CreatedAt = DateTime.UtcNow;
                doing.SectionType = SectionType.Doing;
                doing.SequenceNumber = 1;
                doing.IsArchived = false;
                // Done
                Section done = new Section();
                done.Name = "Done";
                done.Project = result;
                done.CreatedAt = DateTime.UtcNow;
                done.SectionType = SectionType.Done;
                done.SequenceNumber = 2;
                done.IsArchived = false;

                await _sectionRepository.AddAsync(todo);
                await _sectionRepository.AddAsync(doing);
                await _sectionRepository.AddAsync(done);


                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<Project>("Can not create a new project.");
            }
        }

        public async Task<Result<bool>> DeleteProjectAsync(string id)
        {
            try
            {
                // Delete ProjectRoles related to the project
                var projectRolesToDelete = await _projectRoleRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeProjectEntity()
                        .WithFilter(pr => pr.Project.Id == id)
                );

                foreach (var projectRole in projectRolesToDelete)
                {
                    await _projectRoleRepository.DeleteAsync(projectRole.Id);
                }

                // Find ProjectInvitations related to the project
                var projectInvitationsToDelete = await _projectInvitationRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeNotificationEntity()
                        .IncludeProjectEntity()
                        .WithFilter(pi => pi.Project.Id == id)
                );

                foreach (var projectInvitation in projectInvitationsToDelete)
                {
                    // Delete associated Notification
                    await _notificationRepository.DeleteAsync(projectInvitation.Notification.Id);

                    // Delete ProjectInvitation
                    await _projectInvitationRepository.DeleteAsync(projectInvitation.Id);
                }

                // Delete project
                await _projectRepository.DeleteAsync(id);

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not delete the project.");
            }
        }

        public async Task<Result<Project>> GetProjectByIdAsync(string id)
        {
            try
            {
                var result = await _projectRepository.GetByIdAsync(id);

                if (result == null)
                {
                    return ResultFactory.Failure<Project>("Project with such id does not exist.");
                }

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<Project>("Can not get the project by id.");
            }
        }

        public async Task<Result<bool>> UpdateProjectAsync(Project project)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(project);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<bool>(validation.ErrorMessages);
                }

                var projectToUpdate = await _projectRepository.GetByIdAsync(project.Id);
                if(projectToUpdate == null)
                {
                    return ResultFactory.Failure<bool>("Can not find project with such id.");
                }

                projectToUpdate.Name = project.Name;

                await _projectRepository.UpdateAsync(projectToUpdate);
                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not update the project.");
            }
        }

        public async Task<Result<List<Project>>> GetProjectsByUserIdAsync(string userId)
        {
            try
            {
                var result = await _projectRepository.GetFilteredItemsAsync(
                    builder => builder
                                    .IncludeUserEntity()
                                    .WithFilter(p => p.User.Id == userId)
                    );
                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<Project>>("Can not get projects by user id.");
            }
        }
    }
}
