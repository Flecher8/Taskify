using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Interfaces;
using Taskify.BLL.Services;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;
using Taskify.Core.Result;
using Taskify.DAL.Helpers;
using Taskify.DAL.Interfaces;

namespace Taskify.Tests.BAL.Tests.ServicesTests
{
    public class ProjectInvitationsServiceTests
    {
        [Fact]
        public async Task RespondToProjectInvitationAsync_ValidInput_ReturnsSuccess()
        {
            // Arrange
            var projectInvitationId = "valid_project_invitation_id";
            var projectInvitation = new ProjectInvitation
            {
                Id = projectInvitationId,
                Notification = new Notification { Id = "valid_notification_id" },
                IsAccepted = null,
                Project = new Project()
            };

            var notificationServiceMock = new Mock<INotificationService>();
            notificationServiceMock.Setup(service => service.GetNotificationByIdAsync(projectInvitation.Notification.Id))
                                    .ReturnsAsync(ResultFactory.Success(projectInvitation.Notification));

            notificationServiceMock.Setup(service => service.MarkNotificationAsReadAsync(projectInvitation.Notification.Id))
                                    .ReturnsAsync(ResultFactory.Success(true));

            var projectInvitationRepositoryMock = new Mock<IProjectInvitationRepository>();
            projectInvitationRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectInvitationFilterBuilder>>()))
                                          .ReturnsAsync(new List<ProjectInvitation> { projectInvitation });

            projectInvitationRepositoryMock.Setup(repo => repo.UpdateAsync(projectInvitation))
                                          .Returns(Task.CompletedTask);

            var projectMembersServiceMock = new Mock<IProjectMembersService>();
            projectMembersServiceMock.Setup(service => service.CreateProjectMemberAsync(It.IsAny<ProjectMember>()))
                                     .ReturnsAsync(ResultFactory.Success(new ProjectMember())); // Simulate success

            var loggerMock = new Mock<ILogger<ProjectInvitationsService>>();

            var service = new ProjectInvitationsService(projectInvitationRepositoryMock.Object,
                                                        It.IsAny<IUserRepository>(),
                                                        It.IsAny<IProjectRepository>(),
                                                        It.IsAny<IValidator<ProjectInvitation>>(),
                                                        loggerMock.Object,
                                                        notificationServiceMock.Object,
                                                        projectMembersServiceMock.Object); // Inject the projectMembersServiceMock

            // Act
            var result = await service.RespondToProjectInvitationAsync(projectInvitationId, true);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);

            // Verify that MarkNotificationAsReadAsync was called with the correct notification ID
            notificationServiceMock.Verify(service => service.MarkNotificationAsReadAsync(projectInvitation.Notification.Id), Times.Once);

            // Verify that CreateProjectMemberAsync was called with the correct arguments
            projectMembersServiceMock.Verify(service => service.CreateProjectMemberAsync(It.IsAny<ProjectMember>()), Times.Once);
        }

        [Fact]
        public async Task GetProjectInvitationsByUserIdAsync_ValidUserId_ReturnsSuccess()
        {
            // Arrange
            var userId = "valid_user_id";
            var notifications = new List<Notification> { new Notification(), new Notification() };
            var projectInvitations = notifications.Select(notification => new ProjectInvitation { Notification = notification }).ToList();

            var notificationServiceMock = new Mock<INotificationService>();
            notificationServiceMock.Setup(service => service.GetNotificationsByUserIdAsync(userId))
                                   .ReturnsAsync(ResultFactory.Success(notifications));

            var projectInvitationRepositoryMock = new Mock<IProjectInvitationRepository>();
            projectInvitationRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectInvitationFilterBuilder>>()))
                                          .ReturnsAsync(projectInvitations);

            var loggerMock = new Mock<ILogger<ProjectInvitationsService>>();

            var service = new ProjectInvitationsService(projectInvitationRepositoryMock.Object,
                                                        It.IsAny<IUserRepository>(),
                                                        It.IsAny<IProjectRepository>(),
                                                        It.IsAny<IValidator<ProjectInvitation>>(),
                                                        loggerMock.Object,
                                                        notificationServiceMock.Object,
                                                        It.IsAny<IProjectMembersService>());

            // Act
            var result = await service.GetProjectInvitationsByUserIdAsync(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(projectInvitations, result.Data);
        }

        [Fact]
        public async Task GetUnreadProjectInvitationsByUserIdAsync_ValidUserId_ReturnsSuccess()
        {
            // Arrange
            var userId = "valid_user_id";
            var unreadNotifications = new List<Notification> { new Notification(), new Notification() };
            var unreadProjectInvitations = unreadNotifications.Select(notification => new ProjectInvitation { Notification = notification }).ToList();

            var notificationServiceMock = new Mock<INotificationService>();
            notificationServiceMock.Setup(service => service.GetUnreadNotificationsByUserIdAsync(userId))
                                   .ReturnsAsync(ResultFactory.Success(unreadNotifications));

            var projectInvitationRepositoryMock = new Mock<IProjectInvitationRepository>();
            projectInvitationRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectInvitationFilterBuilder>>()))
                                          .ReturnsAsync(unreadProjectInvitations);

            var loggerMock = new Mock<ILogger<ProjectInvitationsService>>();

            var service = new ProjectInvitationsService(projectInvitationRepositoryMock.Object,
                                                        It.IsAny<IUserRepository>(),
                                                        It.IsAny<IProjectRepository>(),
                                                        It.IsAny<IValidator<ProjectInvitation>>(),
                                                        loggerMock.Object,
                                                        notificationServiceMock.Object,
                                                        It.IsAny<IProjectMembersService>());

            // Act
            var result = await service.GetUnreadProjectInvitationsByUserIdAsync(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(unreadProjectInvitations, result.Data);
        }

        [Fact]
        public async Task MarkProjectInvitationAsReadAsync_ValidInput_ReturnsSuccess()
        {
            // Arrange
            var projectInvitationId = "valid_project_invitation_id";
            var projectInvitation = new ProjectInvitation
            {
                Id = projectInvitationId,
                Notification = new Notification { Id = "valid_notification_id" }
            };

            var notificationServiceMock = new Mock<INotificationService>();
            notificationServiceMock.Setup(service => service.MarkNotificationAsReadAsync(projectInvitation.Notification.Id))
                                    .ReturnsAsync(ResultFactory.Success(true)); // Simulate success

            var projectInvitationRepositoryMock = new Mock<IProjectInvitationRepository>();
            projectInvitationRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectInvitationFilterBuilder>>()))
                                          .ReturnsAsync(new List<ProjectInvitation> { projectInvitation });

            var loggerMock = new Mock<ILogger<ProjectInvitationsService>>();

            var service = new ProjectInvitationsService(projectInvitationRepositoryMock.Object,
                                                        It.IsAny<IUserRepository>(),
                                                        It.IsAny<IProjectRepository>(),
                                                        It.IsAny<IValidator<ProjectInvitation>>(),
                                                        loggerMock.Object,
                                                        notificationServiceMock.Object,
                                                        It.IsAny<IProjectMembersService>());

            // Act
            var result = await service.MarkProjectInvitationAsReadAsync(projectInvitationId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task MarkProjectInvitationAsReadAsync_InvalidInput_ReturnsFailure()
        {
            // Arrange
            var projectInvitationId = "invalid_project_invitation_id";

            var projectInvitationRepositoryMock = new Mock<IProjectInvitationRepository>();
            projectInvitationRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectInvitationFilterBuilder>>()))
                                          .ReturnsAsync(new List<ProjectInvitation>()); // Simulate not finding project invitation

            var loggerMock = new Mock<ILogger<ProjectInvitationsService>>();

            var service = new ProjectInvitationsService(projectInvitationRepositoryMock.Object,
                                                        It.IsAny<IUserRepository>(),
                                                        It.IsAny<IProjectRepository>(),
                                                        It.IsAny<IValidator<ProjectInvitation>>(),
                                                        loggerMock.Object,
                                                        It.IsAny<INotificationService>(),
                                                        It.IsAny<IProjectMembersService>());

            // Act
            var result = await service.MarkProjectInvitationAsReadAsync(projectInvitationId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Contains("Can not find project invitation with such id.", result.Errors);
        }

        [Fact]
        public async Task DeleteProjectInvitationAsync_ValidInput_ReturnsSuccess()
        {
            // Arrange
            var projectInvitationId = "valid_project_invitation_id";
            var projectInvitation = new ProjectInvitation
            {
                Id = projectInvitationId,
                Notification = new Notification { Id = "valid_notification_id" }
            };

            var notificationServiceMock = new Mock<INotificationService>();
            notificationServiceMock.Setup(service => service.DeleteNotificationAsync(projectInvitation.Notification.Id))
                                    .ReturnsAsync(ResultFactory.Success(true)); // Simulate success

            var projectInvitationRepositoryMock = new Mock<IProjectInvitationRepository>();
            projectInvitationRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectInvitationFilterBuilder>>()))
                                          .ReturnsAsync(new List<ProjectInvitation> { projectInvitation });

            projectInvitationRepositoryMock.Setup(repo => repo.DeleteAsync(projectInvitationId))
                                          .Returns(Task.CompletedTask); // Simulate success

            var loggerMock = new Mock<ILogger<ProjectInvitationsService>>();

            var service = new ProjectInvitationsService(projectInvitationRepositoryMock.Object,
                                                        It.IsAny<IUserRepository>(),
                                                        It.IsAny<IProjectRepository>(),
                                                        It.IsAny<IValidator<ProjectInvitation>>(),
                                                        loggerMock.Object,
                                                        notificationServiceMock.Object,
                                                        It.IsAny<IProjectMembersService>());

            // Act
            var result = await service.DeleteProjectInvitationAsync(projectInvitationId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task GetProjectInvitationByIdAsync_ValidInput_ReturnsSuccess()
        {
            // Arrange
            var projectInvitationId = "valid_project_invitation_id";
            var projectInvitation = new ProjectInvitation
            {
                Id = projectInvitationId,
                Notification = new Notification { Id = "valid_notification_id" }
            };

            var projectInvitationRepositoryMock = new Mock<IProjectInvitationRepository>();
            projectInvitationRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectInvitationFilterBuilder>>()))
                                          .ReturnsAsync(new List<ProjectInvitation> { projectInvitation });

            var loggerMock = new Mock<ILogger<ProjectInvitationsService>>();

            var service = new ProjectInvitationsService(projectInvitationRepositoryMock.Object,
                                                        It.IsAny<IUserRepository>(),
                                                        It.IsAny<IProjectRepository>(),
                                                        It.IsAny<IValidator<ProjectInvitation>>(),
                                                        loggerMock.Object,
                                                        It.IsAny<INotificationService>(),
                                                        It.IsAny<IProjectMembersService>());

            // Act
            var result = await service.GetProjectInvitationByIdAsync(projectInvitationId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(projectInvitation, result.Data);
        }
    }
}
