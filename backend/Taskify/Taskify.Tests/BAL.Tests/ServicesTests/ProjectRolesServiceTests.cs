using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Services;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;
using Taskify.DAL.Helpers;
using Taskify.DAL.Interfaces;

namespace Taskify.Tests.BAL.Tests.ServicesTests
{
    public class ProjectRolesServiceTests
    {
        [Fact]
        public async Task CreateProjectRoleAsync_ValidProjectRole_ReturnsSuccess()
        {
            // Arrange
            var projectRole = new ProjectRole { Id = "1", Name = "Test Role", Project = new Project { Id = "1" } };

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(repo => repo.GetByIdAsync(projectRole.Project.Id))
                                 .ReturnsAsync(new Project { Id = "1" });

            var projectRoleRepositoryMock = new Mock<IProjectRoleRepository>();
            projectRoleRepositoryMock.Setup(repo => repo.AddAsync(projectRole))
                                     .ReturnsAsync(projectRole);

            var validatorMock = new Mock<IValidator<ProjectRole>>();
            validatorMock.Setup(v => v.ValidateAsync(projectRole))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<ProjectRolesService>>();
            var service = new ProjectRolesService(projectRoleRepositoryMock.Object,
                                                  projectRepositoryMock.Object,
                                                  validatorMock.Object,
                                                  loggerMock.Object);

            // Act
            var result = await service.CreateProjectRoleAsync(projectRole);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(projectRole, result.Data);
            projectRoleRepositoryMock.Verify(repo => repo.AddAsync(projectRole), Times.Once);
        }

        [Fact]
        public async Task CreateProjectRoleAsync_InvalidProjectId_ReturnsFailure()
        {
            // Arrange
            var projectRole = new ProjectRole { Id = "1", Name = "Test Role", Project = new Project { Id = "invalid_id" } };

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(repo => repo.GetByIdAsync(projectRole.Project.Id))
                                 .ReturnsAsync((Project)null); // Simulating project not found

            var projectRoleRepositoryMock = new Mock<IProjectRoleRepository>();

            var validatorMock = new Mock<IValidator<ProjectRole>>();
            validatorMock.Setup(v => v.ValidateAsync(projectRole))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<ProjectRolesService>>();
            var service = new ProjectRolesService(projectRoleRepositoryMock.Object,
                                                  projectRepositoryMock.Object,
                                                  validatorMock.Object,
                                                  loggerMock.Object);

            // Act
            var result = await service.CreateProjectRoleAsync(projectRole);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Can not find project with such id.", result.Errors);
            projectRoleRepositoryMock.Verify(repo => repo.AddAsync(projectRole), Times.Never);
        }

        [Fact]
        public async Task GetProjectRoleByIdAsync_ValidId_ReturnsSuccess()
        {
            // Arrange
            var projectId = "valid_project_id";
            var projectRole = new ProjectRole { Id = "1", Name = "Test Role", Project = new Project { Id = projectId } };

            var projectRoleRepositoryMock = new Mock<IProjectRoleRepository>();
            projectRoleRepositoryMock.Setup(repo => repo.GetByIdAsync(projectRole.Id))
                                     .ReturnsAsync(projectRole);

            var loggerMock = new Mock<ILogger<ProjectRolesService>>();
            var service = new ProjectRolesService(projectRoleRepositoryMock.Object,
                                                  null,
                                                  null,
                                                  loggerMock.Object);

            // Act
            var result = await service.GetProjectRoleByIdAsync(projectRole.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(projectRole, result.Data);
        }

        [Fact]
        public async Task GetProjectRoleByIdAsync_InvalidId_ReturnsFailure()
        {
            // Arrange
            var invalidId = "invalid_id";

            var projectRoleRepositoryMock = new Mock<IProjectRoleRepository>();
            projectRoleRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                                     .ReturnsAsync((ProjectRole)null);

            var loggerMock = new Mock<ILogger<ProjectRolesService>>();
            var service = new ProjectRolesService(projectRoleRepositoryMock.Object,
                                                  null,
                                                  null,
                                                  loggerMock.Object);

            // Act
            var result = await service.GetProjectRoleByIdAsync(invalidId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Project role with such id does not exist.", result.Errors);
        }

        [Fact]
        public async Task GetRolesByProjectIdAsync_ValidProjectId_ReturnsSuccess()
        {
            // Arrange
            var projectId = "valid_project_id";
            var projectRoles = new List<ProjectRole> { new ProjectRole(), new ProjectRole() };

            var projectRoleRepositoryMock = new Mock<IProjectRoleRepository>();
            projectRoleRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectRoleFilterBuilder>>()))
                                     .ReturnsAsync(projectRoles);

            var loggerMock = new Mock<ILogger<ProjectRolesService>>();
            var service = new ProjectRolesService(projectRoleRepositoryMock.Object,
                                                  null,
                                                  null,
                                                  loggerMock.Object);

            // Act
            var result = await service.GetRolesByProjectIdAsync(projectId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(projectRoles, result.Data);
        }

        [Fact]
        public async Task GetRolesByProjectIdAsync_InvalidProjectId_ReturnsFailure()
        {
            // Arrange
            var invalidProjectId = "invalid_project_id";

            var projectRoleRepositoryMock = new Mock<IProjectRoleRepository>();
            projectRoleRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectRoleFilterBuilder>>()))
                                     .ThrowsAsync(new Exception("Simulated error"));

            var loggerMock = new Mock<ILogger<ProjectRolesService>>();
            var service = new ProjectRolesService(projectRoleRepositoryMock.Object,
                                                  null,
                                                  null,
                                                  loggerMock.Object);

            // Act
            var result = await service.GetRolesByProjectIdAsync(invalidProjectId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Can not get project roles by project id.", result.Errors);
        }

        [Fact]
        public async Task DeleteProjectRoleAsync_ValidId_ReturnsSuccess()
        {
            // Arrange
            var validId = "valid_project_role_id";

            var projectRoleRepositoryMock = new Mock<IProjectRoleRepository>();
            projectRoleRepositoryMock.Setup(repo => repo.DeleteAsync(validId))
                                     .Returns(Task.CompletedTask);

            var loggerMock = new Mock<ILogger<ProjectRolesService>>();
            var service = new ProjectRolesService(projectRoleRepositoryMock.Object,
                                                  null,
                                                  null,
                                                  loggerMock.Object);

            // Act
            var result = await service.DeleteProjectRoleAsync(validId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task DeleteProjectRoleAsync_InvalidId_ReturnsFailure()
        {
            // Arrange
            var invalidId = "invalid_project_role_id";

            var projectRoleRepositoryMock = new Mock<IProjectRoleRepository>();
            projectRoleRepositoryMock.Setup(repo => repo.DeleteAsync(invalidId))
                                     .ThrowsAsync(new Exception("Simulated error"));

            var loggerMock = new Mock<ILogger<ProjectRolesService>>();
            var service = new ProjectRolesService(projectRoleRepositoryMock.Object,
                                                  null,
                                                  null,
                                                  loggerMock.Object);

            // Act
            var result = await service.DeleteProjectRoleAsync(invalidId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Contains("Can not delete the project role.", result.Errors);
        }

        [Fact]
        public async Task UpdateProjectRoleAsync_ValidProjectRole_ReturnsSuccess()
        {
            // Arrange
            var projectRole = new ProjectRole { Id = "1", Name = "Test Role", Project = new Project { Id = "1" } };

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(repo => repo.GetByIdAsync(projectRole.Project.Id))
                                 .ReturnsAsync(new Project { Id = "1" });

            var projectRoleRepositoryMock = new Mock<IProjectRoleRepository>();
            projectRoleRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectRoleFilterBuilder>>()))
                                     .ReturnsAsync(new List<ProjectRole> { projectRole });

            projectRoleRepositoryMock.Setup(repo => repo.UpdateAsync(projectRole))
                                     .Returns(Task.CompletedTask);

            var validatorMock = new Mock<IValidator<ProjectRole>>();
            validatorMock.Setup(v => v.ValidateAsync(projectRole))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<ProjectRolesService>>();
            var service = new ProjectRolesService(projectRoleRepositoryMock.Object,
                                                  projectRepositoryMock.Object,
                                                  validatorMock.Object,
                                                  loggerMock.Object);

            // Act
            var result = await service.UpdateProjectRoleAsync(projectRole);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task UpdateProjectRoleAsync_InvalidProjectRole_ReturnsFailure()
        {
            // Arrange
            var projectRole = new ProjectRole { Id = "1", Name = "Test Role", Project = new Project { Id = "invalid_id" } };

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(repo => repo.GetByIdAsync(projectRole.Project.Id))
                                .ReturnsAsync((Project)null);

            var projectRoleRepositoryMock = new Mock<IProjectRoleRepository>();
            projectRoleRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectRoleFilterBuilder>>()))
                                .ReturnsAsync(new List<ProjectRole>() { projectRole });

            var validatorMock = new Mock<IValidator<ProjectRole>>();
            validatorMock.Setup(v => v.ValidateAsync(projectRole))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<ProjectRolesService>>();
            var service = new ProjectRolesService(projectRoleRepositoryMock.Object,
                                                  projectRepositoryMock.Object,
                                                  validatorMock.Object,
                                                  loggerMock.Object);

            // Act
            var result = await service.UpdateProjectRoleAsync(projectRole);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Contains("Can not find project with such id.", result.Errors);
        }

        [Fact]
        public async Task UpdateProjectRoleAsync_InvalidRoleId_ReturnsFailure()
        {
            // Arrange
            var projectRole = new ProjectRole { Id = "invalid_id", Name = "Test Role", Project = new Project { Id = "1" } };

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(repo => repo.GetByIdAsync(projectRole.Project.Id))
                                 .ReturnsAsync(new Project { Id = "1" });

            var projectRoleRepositoryMock = new Mock<IProjectRoleRepository>();
            projectRoleRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectRoleFilterBuilder>>()))
                                     .ReturnsAsync(new List<ProjectRole>());

            var validatorMock = new Mock<IValidator<ProjectRole>>();
            validatorMock.Setup(v => v.ValidateAsync(projectRole))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<ProjectRolesService>>();
            var service = new ProjectRolesService(projectRoleRepositoryMock.Object,
                                                  projectRepositoryMock.Object,
                                                  validatorMock.Object,
                                                  loggerMock.Object);

            // Act
            var result = await service.UpdateProjectRoleAsync(projectRole);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Contains("Can not find project role with such id.", result.Errors);
        }
    }
}
