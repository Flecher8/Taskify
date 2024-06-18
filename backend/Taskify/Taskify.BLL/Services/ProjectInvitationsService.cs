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
    public class ProjectInvitationsService : IProjectInvitationsService
    {
        private readonly IProjectInvitationRepository _projectInvitationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IValidator<ProjectInvitation> _validator;
        private readonly ILogger<ProjectInvitationsService> _logger;
        private readonly INotificationService _notificationService;
        private readonly IProjectMembersService _projectMembersService;

        public ProjectInvitationsService(IProjectInvitationRepository projectInvitationRepository,
            IUserRepository userRepository,
            IProjectRepository projectRepository,
            IValidator<ProjectInvitation> validator,
            ILogger<ProjectInvitationsService> logger,
            INotificationService notificationService,
            IProjectMembersService projectMembersService)
        {
            _projectInvitationRepository = projectInvitationRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _validator = validator;
            _logger = logger;
            _notificationService = notificationService;
            _projectMembersService = projectMembersService;
        }

        public async Task<Result<ProjectInvitation>> CreateProjectInvitationAsync(string userId, ProjectInvitation projectInvitation)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(projectInvitation);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<ProjectInvitation>(validation.ErrorMessages);
                }

                if (projectInvitation.Project == null || string.IsNullOrEmpty(projectInvitation.Project.Id))
                {
                    return ResultFactory.Failure<ProjectInvitation>("Invalid project specified.");
                }

                if (string.IsNullOrEmpty(userId))
                {
                    return ResultFactory.Failure<ProjectInvitation>("Invalid user specified.");
                }

                // Check if the user is already a member of the project using ProjectMembersService
                var isUserAlreadyMemberResult = await _projectMembersService.IsUserAlreadyMemberAsync(userId, projectInvitation.Project.Id);

                if (isUserAlreadyMemberResult.IsSuccess && isUserAlreadyMemberResult.Data)
                {
                    return ResultFactory.Failure<ProjectInvitation>("User is already a member of this project.");
                }

                // Check if an invitation to this user for this project already exists and has not been accepted yet
                var notificationsByUser = await _notificationService.GetNotificationsByUserIdAsync(userId);
                var existingProjectInvitationsForUser = notificationsByUser.Data
                    .FindAll(n => n.NotificationType == NotificationType.ProjectInvitation);

                foreach (var existingNotification in existingProjectInvitationsForUser)
                {
                    var existingProjectInvitation = (await _projectInvitationRepository.GetFilteredItemsAsync(
                        builder => builder
                            .IncludeNotificationEntity()
                            .IncludeProjectEntity()
                            .WithFilter(p => p.Notification.Id == existingNotification.Id && p.Project.Id == projectInvitation.Project.Id)
                        )).FirstOrDefault() ;
                    if (existingProjectInvitation != null && existingProjectInvitation.IsAccepted == null)
                    {
                        return ResultFactory.Failure<ProjectInvitation>("Invitation to this user for this project already exists and has not been accepted yet.");
                    }
                }

                var user = await _userRepository.GetByIdAsync(userId);
                var project = (await _projectRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeUserEntity()
                        .WithFilter(p => p.Id == projectInvitation.Project.Id)
                    )).FirstOrDefault();

                if (user == null)
                {
                    return ResultFactory.Failure<ProjectInvitation>("Can not find user with such id.");
                }

                if (project == null)
                {
                    return ResultFactory.Failure<ProjectInvitation>("Can not find project with such id.");
                }

                // Check if the user is not the creator of the project
                if (userId == project.User.Id)
                {
                    return ResultFactory.Failure<ProjectInvitation>("Project creators cannot send an invitation to themselves.");
                }

                Notification notification = new Notification();
                notification.User = user;
                notification.NotificationType = NotificationType.ProjectInvitation;

                var notificationResult = await _notificationService.CreateNotificationAsync(notification);
                if (!notificationResult.IsSuccess)
                {
                    return ResultFactory.Failure<ProjectInvitation>(notificationResult.Errors);
                }

                projectInvitation.Notification = notificationResult.Data;
                projectInvitation.IsAccepted = null;
                projectInvitation.Project = project;

                var result = await _projectInvitationRepository.AddAsync(projectInvitation);

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<ProjectInvitation>("Can not create a new project invitation.");
            }
        }

        public async Task<Result<bool>> RespondToProjectInvitationAsync(string projectInvitationId, bool isAccepted)
        {
            try
            {
                var projectInvitation = (await _projectInvitationRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeNotificationEntity()
                        .IncludeProjectEntity()
                        .WithFilter(pi => pi.Id == projectInvitationId)
                    )).FirstOrDefault();

                if (projectInvitation == null)
                {
                    return ResultFactory.Failure<bool>("Project invitation with such id does not exist.");
                }

                if (projectInvitation.IsAccepted != null)
                {
                    return ResultFactory.Failure<bool>("A response to this project invitation has already been received.");
                }

                var notificationFindResult = await _notificationService.GetNotificationByIdAsync(projectInvitation.Notification.Id);

                if (!notificationFindResult.IsSuccess)
                {
                    return ResultFactory.Failure<bool>(notificationFindResult.Errors);
                }

                var notification = notificationFindResult.Data;

                projectInvitation.IsAccepted = isAccepted;

                await _notificationService.MarkNotificationAsReadAsync(projectInvitation.Notification.Id);

                await _projectInvitationRepository.UpdateAsync(projectInvitation);

                if (isAccepted)
                {
                    await AddUserToProject(projectInvitation.Project, notification.User);
                }

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not respond to the project invitation.");
            }
        }

        private async Task AddUserToProject(Project project, User user)
        {
            ProjectMember projectMember = new ProjectMember();
            projectMember.Project = project;
            projectMember.User = user;

            await _projectMembersService.CreateProjectMemberAsync(projectMember);
        }

        public async Task<Result<List<ProjectInvitation>>> GetProjectInvitationsByUserIdAsync(string userId)
        {
            try
            {
                // Use the NotificationService to get notifications with the associated user information and type
                var notificationResult = await _notificationService.GetNotificationsByUserIdAsync(userId);

                if (!notificationResult.IsSuccess)
                {
                    return ResultFactory.Failure<List<ProjectInvitation>>(notificationResult.Errors);
                }

                // Filter notifications based on NotificationType.ProjectInvitation
                var projectInvitationNotifications = notificationResult.Data
                    .Where(n => n.NotificationType == NotificationType.ProjectInvitation)
                    .ToList();

                // Extract notification IDs
                var notificationIds = projectInvitationNotifications.Select(n => n.Id).ToList();

                // Now use the ProjectInvitationRepository to get project invitations using notification IDs
                var projectInvitationResult = await _projectInvitationRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeNotificationEntity()
                        .IncludeProjectEntity()
                        .WithFilter(pi => notificationIds.Contains(pi.Notification.Id))
                );

                return ResultFactory.Success(projectInvitationResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<ProjectInvitation>>("Can not get project invitations by user id.");
            }
        }

        public async Task<Result<List<ProjectInvitation>>> GetUnreadProjectInvitationsByUserIdAsync(string userId)
        {
            try
            {
                // Use the NotificationService to get unread notifications with the associated user information
                var notificationResult = await _notificationService.GetUnreadNotificationsByUserIdAsync(userId);

                if (!notificationResult.IsSuccess)
                {
                    return ResultFactory.Failure<List<ProjectInvitation>>(notificationResult.Errors);
                }

                // Extract notification IDs
                var notificationIds = notificationResult.Data.Select(n => n.Id).ToList();

                // Now use the ProjectInvitationRepository to get project invitations using notification IDs
                var projectInvitationResult = await _projectInvitationRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeNotificationEntity()
                        .IncludeProjectEntity()
                        .WithFilter(pi => notificationIds.Contains(pi.Notification.Id))
                );

                return ResultFactory.Success(projectInvitationResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<ProjectInvitation>>("Can not get unread project invitations by user id.");
            }
        }

        public async Task<Result<bool>> MarkProjectInvitationAsReadAsync(string projectInvitationId)
        {
            try
            {
                var projectInvitation = (await _projectInvitationRepository.GetFilteredItemsAsync(
                    builder => builder
                                    .IncludeNotificationEntity()
                                    .WithFilter(pi => pi.Id == projectInvitationId)
                    )).FirstOrDefault();

                if (projectInvitation == null)
                {
                    return ResultFactory.Failure<bool>("Can not find project invitation with such id.");
                }

                await _notificationService.MarkNotificationAsReadAsync(projectInvitation.Notification.Id);
                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not mark the project invitation as read.");
            }
        }

        public async Task<Result<bool>> DeleteProjectInvitationAsync(string projectInvitationId)
        {
            try
            {
                var projectInvitation = (await _projectInvitationRepository.GetFilteredItemsAsync(
                    builder => builder
                                    .IncludeNotificationEntity()
                                    .WithFilter(pi => pi.Id == projectInvitationId)
                    )).FirstOrDefault();

                if (projectInvitation == null)
                {
                    return ResultFactory.Failure<bool>("Can not find project invitation with such id.");
                }

                await _notificationService.DeleteNotificationAsync(projectInvitation.Notification.Id);
                await _projectInvitationRepository.DeleteAsync(projectInvitation.Id);

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not delete the project invitation.");
            }
        }

        public async Task<Result<ProjectInvitation>> GetProjectInvitationByIdAsync(string projectInvitationId)
        {
            try
            {
                var projectInvitation = (await _projectInvitationRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeNotificationEntity()
                        .IncludeProjectEntity()
                        .WithFilter(pi => pi.Id == projectInvitationId)
                    )).FirstOrDefault();

                if (projectInvitation == null)
                {
                    return ResultFactory.Failure<ProjectInvitation>("Project invitation with such id does not exist.");
                }

                return ResultFactory.Success(projectInvitation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<ProjectInvitation>("Can not get project invitation by id.");
            }
        }

        public async Task<Result<ProjectInvitation>> GetProjectInvitationByNotificationIdAsync(string notificationId)
        {
            try
            {
                var projectInvitation = (await _projectInvitationRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeNotificationEntity()
                        .IncludeProjectEntity()
                        .WithFilter(pi => pi.Notification.Id == notificationId)
                    )).FirstOrDefault();

                if (projectInvitation == null)
                {
                    return ResultFactory.Failure<ProjectInvitation>("Project invitation with such notification id does not exist.");
                }

                return ResultFactory.Success(projectInvitation);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<ProjectInvitation>("Can not get project invitation by notification id.");
            }
        }
    }
}
