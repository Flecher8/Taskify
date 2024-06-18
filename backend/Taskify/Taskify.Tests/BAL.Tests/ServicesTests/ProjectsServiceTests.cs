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
    public class ProjectsServiceTests
    {
        [Fact]
        public async Task CreateProjectAsync_ValidProject_ReturnsSuccess()
        {
            // Arrange
            var project = new Project { User = new User { Id = "valid_user_id" } };

            var userRepositoryMock = new Mock<IUsersService>();
            userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(project.User.Id))
                              .ReturnsAsync(ResultFactory.Success(new User { Id = "valid_user_id" }));

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(repo => repo.AddAsync(project))
                                 .ReturnsAsync(project);

            var sectionRepositoryMock = new Mock<ISectionRepository>();
            sectionRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Section>()))
                                 .ReturnsAsync(new Section());

            var validatorMock = new Mock<IValidator<Project>>();
            validatorMock.Setup(validator => validator.ValidateAsync(project))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<ProjectsService>>();
            var service = new ProjectsService(projectRepositoryMock.Object,
                                              userRepositoryMock.Object,
                                              sectionRepositoryMock.Object,
                                              null,
                                              null,
                                              null,
                                              validatorMock.Object,
                                              loggerMock.Object);

            // Act
            var result = await service.CreateProjectAsync(project);

            // Assert
            Assert.True(result.IsSuccess);
            projectRepositoryMock.Verify(repo => repo.AddAsync(project), Times.Once);
            sectionRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Section>()), Times.Exactly(3)); // Verify section creation
        }

        [Fact]
        public async Task CreateProjectAsync_InvalidUser_ReturnsFailure()
        {
            // Arrange
            var project = new Project { User = new User { Id = "invalid_user_id" } };

            var userRepositoryMock = new Mock<IUsersService>();
            userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(project.User.Id))
                              .ReturnsAsync(ResultFactory.Failure<User>("Can not find such user id."));

            var loggerMock = new Mock<ILogger<ProjectsService>>();
            var service = new ProjectsService(null,
                                              userRepositoryMock.Object,
                                              null,
                                              null,
                                              null,
                                              null,
                                              null,
                                              loggerMock.Object);

            // Act
            var result = await service.CreateProjectAsync(project);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Can not find such user id.", result.Errors);
        }

        [Fact]
        public async Task DeleteProjectAsync_ValidProjectId_ReturnsSuccess()
        {
            // Arrange
            var projectId = "valid_project_id";

            var projectRoleRepositoryMock = new Mock<IProjectRoleRepository>();
            projectRoleRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectRoleFilterBuilder>>()))
                                     .ReturnsAsync(new List<ProjectRole>());

            var projectInvitationRepositoryMock = new Mock<IProjectInvitationRepository>();
            projectInvitationRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectInvitationFilterBuilder>>()))
                                           .ReturnsAsync(new List<ProjectInvitation>());

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(repo => repo.DeleteAsync(projectId))
                                 .Returns(Task.CompletedTask);

            var loggerMock = new Mock<ILogger<ProjectsService>>();
            var service = new ProjectsService(projectRepositoryMock.Object,
                                              null,
                                              null,
                                              projectInvitationRepositoryMock.Object,
                                              null,
                                              projectRoleRepositoryMock.Object,
                                              null,
                                              loggerMock.Object);

            // Act
            var result = await service.DeleteProjectAsync(projectId);

            // Assert
            Assert.True(result.IsSuccess);
            projectRepositoryMock.Verify(repo => repo.DeleteAsync(projectId), Times.Once);
        }

        [Fact]
        public async Task DeleteProjectAsync_RepositoryError_ReturnsFailure()
        {
            // Arrange
            var projectId = "valid_project_id";

            var projectRoleRepositoryMock = new Mock<IProjectRoleRepository>();
            projectRoleRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectRoleFilterBuilder>>()))
                                     .ReturnsAsync(new List<ProjectRole>());

            var projectInvitationRepositoryMock = new Mock<IProjectInvitationRepository>();
            projectInvitationRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectInvitationFilterBuilder>>()))
                                           .ReturnsAsync(new List<ProjectInvitation>());

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(repo => repo.DeleteAsync(projectId))
                                 .ThrowsAsync(new Exception("Simulated repository error"));

            var loggerMock = new Mock<ILogger<ProjectsService>>();
            var service = new ProjectsService(projectRepositoryMock.Object,
                                              null,
                                              null,
                                              projectInvitationRepositoryMock.Object,
                                              null,
                                              projectRoleRepositoryMock.Object,
                                              null,
                                              loggerMock.Object);

            // Act
            var result = await service.DeleteProjectAsync(projectId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Can not delete the project.", result.Errors);
        }

        [Fact]
        public async Task GetProjectByIdAsync_InvalidProjectId_ReturnsFailure()
        {
            // Arrange
            var projectId = "invalid_project_id";

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(repo => repo.GetByIdAsync(projectId))
                                 .ReturnsAsync((Project)null);

            var loggerMock = new Mock<ILogger<ProjectsService>>();
            var service = new ProjectsService(projectRepositoryMock.Object,
                                              null,
                                              null,
                                              null,
                                              null,
                                              null,
                                              null,
                                              loggerMock.Object);

            // Act
            var result = await service.GetProjectByIdAsync(projectId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Can not get the project by id.", result.Errors.FirstOrDefault());
        }

        [Fact]
        public async Task UpdateProjectAsync_InvalidProject_ReturnsFailure()
        {
            // Arrange
            var projectToUpdate = new Project { Id = "invalid_project_id", Name = "Updated Project" };

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(repo => repo.GetByIdAsync(projectToUpdate.Id))
                                 .ReturnsAsync((Project)null); // Simulate project not found

            var loggerMock = new Mock<ILogger<ProjectsService>>();
            var service = new ProjectsService(projectRepositoryMock.Object,
                                              null,
                                              null,
                                              null,
                                              null,
                                              null,
                                              new ProjectValidator(), // Use the mock validator
                                              loggerMock.Object);

            // Act
            var result = await service.UpdateProjectAsync(projectToUpdate);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid project Id format.", result.Errors); // Validate error message from validator
            projectRepositoryMock.Verify(repo => repo.UpdateAsync(projectToUpdate), Times.Never);
        }

        [Fact]
        public async Task GetProjectsByUserIdAsync_ValidUserId_ReturnsSuccess()
        {
            // Arrange
            var userId = "valid_user_id";
            var projects = new List<Project> { new Project(), new Project() };

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectFilterBuilder>>()))
                                 .ReturnsAsync(projects);

            var loggerMock = new Mock<ILogger<ProjectsService>>();
            var service = new ProjectsService(projectRepositoryMock.Object,
                                              null,
                                              null,
                                              null,
                                              null,
                                              null,
                                              null,
                                              loggerMock.Object);

            // Act
            var result = await service.GetProjectsByUserIdAsync(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(projects, result.Data);
        }

        [Fact]
        public async Task GetProjectsByUserIdAsync_InvalidUserId_ReturnsFailure()
        {
            // Arrange
            var userId = "invalid_user_id";

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectFilterBuilder>>()))
                                 .ThrowsAsync(new Exception("Simulated repository error"));

            var loggerMock = new Mock<ILogger<ProjectsService>>();
            var service = new ProjectsService(projectRepositoryMock.Object,
                                              null,
                                              null,
                                              null,
                                              null,
                                              null,
                                              null,
                                              loggerMock.Object);

            // Act
            var result = await service.GetProjectsByUserIdAsync(userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Can not get projects by user id.", result.Errors);
        }
    }
}
