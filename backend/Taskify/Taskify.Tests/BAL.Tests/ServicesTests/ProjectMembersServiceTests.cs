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
    public class ProjectMembersServiceTests
    {
        [Fact]
        public async Task CreateProjectMemberAsync_InvalidProjectId_ReturnsFailure()
        {
            // Arrange
            var projectMember = new ProjectMember { Project = new Project { Id = "invalid_project_id" }, User = new User { Id = "valid_user_id" } };

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(repo => repo.GetByIdAsync(projectMember.Project.Id))
                                 .ReturnsAsync((Project)null);
            projectRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectFilterBuilder>>()))
                                 .ReturnsAsync(new List<Project>());

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetByIdAsync(projectMember.User.Id))
                              .ReturnsAsync(new User { Id = "valid_user_id" });

            var projectMemberRepositoryMock = new Mock<IProjectMemberRepository>();
            var validatorMock = new Mock<IValidator<ProjectMember>>();
            validatorMock.Setup(v => v.ValidateAsync(projectMember))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<ProjectMembersService>>();
            var service = new ProjectMembersService(projectMemberRepositoryMock.Object,
                                                    projectRepositoryMock.Object,
                                                    userRepositoryMock.Object,
                                                    It.IsAny<IProjectRoleRepository>(),
                                                    validatorMock.Object,
                                                    loggerMock.Object);

            // Act
            var result = await service.CreateProjectMemberAsync(projectMember);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Invalid project specified.", result.Errors);
        }

        [Fact]
        public async Task CreateProjectMemberAsync_InvalidUserId_ReturnsFailure()
        {
            // Arrange
            var projectMember = new ProjectMember { Project = new Project { Id = "valid_project_id" }, User = new User { Id = "invalid_user_id" } };

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(repo => repo.GetByIdAsync(projectMember.Project.Id))
                                 .ReturnsAsync(new Project { Id = "valid_project_id" });
            projectRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectFilterBuilder>>()))
                                 .ReturnsAsync(new List<Project>() { new Project { Id = "valid_project_id" } });

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetByIdAsync(projectMember.User.Id))
                              .ReturnsAsync((User)null);

            var projectMemberRepositoryMock = new Mock<IProjectMemberRepository>();
            var validatorMock = new Mock<IValidator<ProjectMember>>();
            validatorMock.Setup(v => v.ValidateAsync(projectMember))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<ProjectMembersService>>();
            var service = new ProjectMembersService(projectMemberRepositoryMock.Object,
                                                    projectRepositoryMock.Object,
                                                    userRepositoryMock.Object,
                                                    It.IsAny<IProjectRoleRepository>(),
                                                    validatorMock.Object,
                                                    loggerMock.Object);

            // Act
            var result = await service.CreateProjectMemberAsync(projectMember);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Invalid user specified.", result.Errors);
        }

        [Fact]
        public async Task DeleteProjectMemberAsync_ValidId_ReturnsSuccess()
        {
            // Arrange
            var projectId = "valid_project_id";

            var projectMemberRepositoryMock = new Mock<IProjectMemberRepository>();
            projectMemberRepositoryMock.Setup(repo => repo.DeleteAsync(projectId))
                                       .Returns(Task.CompletedTask);

            var loggerMock = new Mock<ILogger<ProjectMembersService>>();
            var service = new ProjectMembersService(projectMemberRepositoryMock.Object,
                                                    It.IsAny<IProjectRepository>(),
                                                    It.IsAny<IUserRepository>(),
                                                    It.IsAny<IProjectRoleRepository>(),
                                                    It.IsAny<IValidator<ProjectMember>>(),
                                                    loggerMock.Object);

            // Act
            var result = await service.DeleteProjectMemberAsync(projectId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task UpdateProjectMemberAsync_ValidProjectMember_ReturnsSuccess()
        {
            // Arrange
            var projectMember = new ProjectMember { Id = "valid_member_id", ProjectRole = new ProjectRole { Id = "valid_role_id" } };

            var projectMemberRepositoryMock = new Mock<IProjectMemberRepository>();
            projectMemberRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectMemberFilterBuilder>>()))
                                       .ReturnsAsync(new List<ProjectMember> { projectMember });
            projectMemberRepositoryMock.Setup(repo => repo.UpdateAsync(projectMember))
                                       .Returns(Task.CompletedTask);

            var projectRoleRepositoryMock = new Mock<IProjectRoleRepository>();
            projectRoleRepositoryMock.Setup(repo => repo.GetByIdAsync(projectMember.ProjectRole.Id))
                                     .ReturnsAsync(new ProjectRole { Id = "valid_role_id" });

            var validatorMock = new Mock<IValidator<ProjectMember>>();
            validatorMock.Setup(v => v.ValidateAsync(projectMember))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<ProjectMembersService>>();
            var service = new ProjectMembersService(projectMemberRepositoryMock.Object,
                                                    It.IsAny<IProjectRepository>(),
                                                    It.IsAny<IUserRepository>(),
                                                    projectRoleRepositoryMock.Object,
                                                    validatorMock.Object,
                                                    loggerMock.Object);

            // Act
            var result = await service.UpdateProjectMemberAsync(projectMember);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task GetMembersByProjectIdAsync_ValidProjectId_ReturnsSuccess()
        {
            // Arrange
            var projectId = "valid_project_id";
            var members = new List<ProjectMember> { new ProjectMember(), new ProjectMember() };

            var projectMemberRepositoryMock = new Mock<IProjectMemberRepository>();
            projectMemberRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectMemberFilterBuilder>>()))
                                       .ReturnsAsync(members);

            var loggerMock = new Mock<ILogger<ProjectMembersService>>();
            var service = new ProjectMembersService(projectMemberRepositoryMock.Object,
                                                    It.IsAny<IProjectRepository>(),
                                                    It.IsAny<IUserRepository>(),
                                                    It.IsAny<IProjectRoleRepository>(),
                                                    It.IsAny<IValidator<ProjectMember>>(),
                                                    loggerMock.Object);

            // Act
            var result = await service.GetMembersByProjectIdAsync(projectId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(members, result.Data);
        }

        [Fact]
        public async Task GetMembersByRoleIdAsync_ValidRoleId_ReturnsSuccess()
        {
            // Arrange
            var roleId = "valid_role_id";
            var members = new List<ProjectMember> { new ProjectMember(), new ProjectMember() };

            var projectMemberRepositoryMock = new Mock<IProjectMemberRepository>();
            projectMemberRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectMemberFilterBuilder>>()))
                                       .ReturnsAsync(members);

            var loggerMock = new Mock<ILogger<ProjectMembersService>>();
            var service = new ProjectMembersService(projectMemberRepositoryMock.Object,
                                                    It.IsAny<IProjectRepository>(),
                                                    It.IsAny<IUserRepository>(),
                                                    It.IsAny<IProjectRoleRepository>(),
                                                    It.IsAny<IValidator<ProjectMember>>(),
                                                    loggerMock.Object);

            // Act
            var result = await service.GetMembersByRoleIdAsync(roleId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(members, result.Data);
        }

        [Fact]
        public async Task GetRoleByUserIdAsync_ValidUserId_ReturnsSuccess()
        {
            // Arrange
            var userId = "valid_user_id";
            var projectRole = new ProjectRole { Id = "valid_role_id" };
            var member = new ProjectMember { User = new User { Id = userId }, ProjectRole = projectRole };

            var projectMemberRepositoryMock = new Mock<IProjectMemberRepository>();
            projectMemberRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectMemberFilterBuilder>>()))
                                       .ReturnsAsync(new List<ProjectMember> { member });

            var loggerMock = new Mock<ILogger<ProjectMembersService>>();
            var service = new ProjectMembersService(projectMemberRepositoryMock.Object,
                                                    It.IsAny<IProjectRepository>(),
                                                    It.IsAny<IUserRepository>(),
                                                    It.IsAny<IProjectRoleRepository>(),
                                                    It.IsAny<IValidator<ProjectMember>>(),
                                                    loggerMock.Object);

            // Act
            var result = await service.GetRoleByUserIdAsync(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(projectRole, result.Data);
        }

        [Fact]
        public async Task GetProjectMemberByIdAsync_ValidId_ReturnsSuccess()
        {
            // Arrange
            var memberId = "valid_member_id";
            var member = new ProjectMember { Id = memberId };

            var projectMemberRepositoryMock = new Mock<IProjectMemberRepository>();
            projectMemberRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectMemberFilterBuilder>>()))
                                       .ReturnsAsync(new List<ProjectMember> { member });

            var loggerMock = new Mock<ILogger<ProjectMembersService>>();
            var service = new ProjectMembersService(projectMemberRepositoryMock.Object,
                                                    It.IsAny<IProjectRepository>(),
                                                    It.IsAny<IUserRepository>(),
                                                    It.IsAny<IProjectRoleRepository>(),
                                                    It.IsAny<IValidator<ProjectMember>>(),
                                                    loggerMock.Object);

            // Act
            var result = await service.GetProjectMemberByIdAsync(memberId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(member, result.Data);
        }

        [Fact]
        public async Task GetProjectsByUserIdAsync_ValidUserId_ReturnsSuccess()
        {
            // Arrange
            var userId = "valid_user_id";
            var projects = new List<Project> { new Project(), new Project() };

            var projectMemberRepositoryMock = new Mock<IProjectMemberRepository>();
            projectMemberRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectMemberFilterBuilder>>()))
                                       .ReturnsAsync(new List<ProjectMember> { new ProjectMember { Project = projects[0] }, new ProjectMember { Project = projects[1] } });

            var loggerMock = new Mock<ILogger<ProjectMembersService>>();
            var service = new ProjectMembersService(projectMemberRepositoryMock.Object,
                                                    It.IsAny<IProjectRepository>(),
                                                    It.IsAny<IUserRepository>(),
                                                    It.IsAny<IProjectRoleRepository>(),
                                                    It.IsAny<IValidator<ProjectMember>>(),
                                                    loggerMock.Object);

            // Act
            var result = await service.GetProjectsByUserIdAsync(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(projects, result.Data);
        }

        [Fact]
        public async Task IsUserAlreadyMemberAsync_ExistingMember_ReturnsSuccess()
        {
            // Arrange
            var userId = "valid_user_id";
            var projectId = "valid_project_id";

            var projectMemberRepositoryMock = new Mock<IProjectMemberRepository>();
            projectMemberRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectMemberFilterBuilder>>()))
                                       .ReturnsAsync(new List<ProjectMember> { new ProjectMember() });

            var loggerMock = new Mock<ILogger<ProjectMembersService>>();
            var service = new ProjectMembersService(projectMemberRepositoryMock.Object,
                                                    It.IsAny<IProjectRepository>(),
                                                    It.IsAny<IUserRepository>(),
                                                    It.IsAny<IProjectRoleRepository>(),
                                                    It.IsAny<IValidator<ProjectMember>>(),
                                                    loggerMock.Object);

            // Act
            var result = await service.IsUserAlreadyMemberAsync(userId, projectId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task IsUserAlreadyMemberAsync_NonExistingMember_ReturnsSuccess()
        {
            // Arrange
            var userId = "valid_user_id";
            var projectId = "valid_project_id";

            var projectMemberRepositoryMock = new Mock<IProjectMemberRepository>();
            projectMemberRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectMemberFilterBuilder>>()))
                                       .ReturnsAsync(new List<ProjectMember>());

            var loggerMock = new Mock<ILogger<ProjectMembersService>>();
            var service = new ProjectMembersService(projectMemberRepositoryMock.Object,
                                                    It.IsAny<IProjectRepository>(),
                                                    It.IsAny<IUserRepository>(),
                                                    It.IsAny<IProjectRoleRepository>(),
                                                    It.IsAny<IValidator<ProjectMember>>(),
                                                    loggerMock.Object);

            // Act
            var result = await service.IsUserAlreadyMemberAsync(userId, projectId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.Data);
        }
    }
}
