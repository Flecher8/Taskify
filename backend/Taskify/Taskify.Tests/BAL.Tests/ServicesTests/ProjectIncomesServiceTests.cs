using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Services;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;
using Taskify.Core.Enums;
using Taskify.DAL.Helpers;
using Taskify.DAL.Interfaces;

namespace Taskify.Tests.BAL.Tests.ServicesTests
{
    public class ProjectIncomesServiceTests
    {
        [Fact]
        public async Task CreateProjectIncomeAsync_ValidInput_ReturnsSuccess()
        {
            // Arrange
            var projectIncome = new ProjectIncome
            {
                Id = "valid_project_income_id",
                Amount = 1000,
                Frequency = ProjectIncomeFrequency.Yearly,
                Project = new Project { Id = "valid_project_id" }
            };

            var projectIncomeRepositoryMock = new Mock<IProjectIncomeRepository>();
            projectIncomeRepositoryMock.Setup(repo => repo.GetByIdAsync(projectIncome.Id))
                                       .ReturnsAsync((ProjectIncome)null); // Simulate project income doesn't exist
            projectIncomeRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectIncomeFilterBuilder>>()))
                                       .ReturnsAsync(new List<ProjectIncome>()); // Simulate no project incomes for the project
            projectIncomeRepositoryMock.Setup(repo => repo.AddAsync(projectIncome))
                                       .ReturnsAsync(projectIncome);

            var projectsRepositoryMock = new Mock<IProjectRepository>();
            projectsRepositoryMock.Setup(repo => repo.GetByIdAsync(projectIncome.Project.Id))
                                  .ReturnsAsync(new Project()); // Simulate project exists

            var validatorMock = new Mock<IValidator<ProjectIncome>>();
            validatorMock.Setup(validator => validator.ValidateAsync(projectIncome))
                         .ReturnsAsync((true, new List<string>())); // Simulate validation success

            var loggerMock = new Mock<ILogger<ProjectIncomesService>>();

            var service = new ProjectIncomesService(projectIncomeRepositoryMock.Object,
                                                     projectsRepositoryMock.Object,
                                                     validatorMock.Object,
                                                     loggerMock.Object);

            // Act
            var result = await service.CreateProjectIncomeAsync(projectIncome);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(projectIncome, result.Data);
        }

        [Fact]
        public async Task DeleteProjectIncomeAsync_ValidInput_ReturnsSuccess()
        {
            // Arrange
            var projectIncomeId = "valid_project_income_id";

            var projectIncomeRepositoryMock = new Mock<IProjectIncomeRepository>();
            projectIncomeRepositoryMock.Setup(repo => repo.DeleteAsync(projectIncomeId))
                                       .Returns(Task.CompletedTask); // Simulate success

            var loggerMock = new Mock<ILogger<ProjectIncomesService>>();

            var service = new ProjectIncomesService(projectIncomeRepositoryMock.Object,
                                                     It.IsAny<IProjectRepository>(),
                                                     It.IsAny<IValidator<ProjectIncome>>(),
                                                     loggerMock.Object);

            // Act
            var result = await service.DeleteProjectIncomeAsync(projectIncomeId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task GetProjectIncomeByProjectIdAsync_ValidInput_ReturnsSuccess()
        {
            // Arrange
            var projectId = "valid_project_id";
            var projectIncome = new ProjectIncome
            {
                Id = "valid_project_income_id",
                Amount = 1000,
                Frequency = ProjectIncomeFrequency.Yearly,
                Project = new Project { Id = projectId }
            };

            var projectIncomeRepositoryMock = new Mock<IProjectIncomeRepository>();
            projectIncomeRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectIncomeFilterBuilder>>()))
                                       .ReturnsAsync(new List<ProjectIncome> { projectIncome });

            var loggerMock = new Mock<ILogger<ProjectIncomesService>>();

            var service = new ProjectIncomesService(projectIncomeRepositoryMock.Object,
                                                     It.IsAny<IProjectRepository>(),
                                                     It.IsAny<IValidator<ProjectIncome>>(),
                                                     loggerMock.Object);

            // Act
            var result = await service.GetProjectIncomesByProjectIdAsync(projectId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(projectIncome, result.Data);
        }

        [Fact]
        public async Task UpdateProjectIncomeAsync_ValidInput_ReturnsSuccess()
        {
            // Arrange
            var projectIncome = new ProjectIncome
            {
                Id = "valid_project_income_id",
                Amount = 1500,
                Frequency = ProjectIncomeFrequency.Yearly
            };

            var projectIncomeRepositoryMock = new Mock<IProjectIncomeRepository>();
            projectIncomeRepositoryMock.Setup(repo => repo.GetByIdAsync(projectIncome.Id))
                                       .ReturnsAsync(projectIncome); // Simulate project income exists
            projectIncomeRepositoryMock.Setup(repo => repo.UpdateAsync(projectIncome))
                                       .Returns(Task.CompletedTask); // Simulate success

            var validatorMock = new Mock<IValidator<ProjectIncome>>();
            validatorMock.Setup(validator => validator.ValidateAsync(projectIncome))
                         .ReturnsAsync((true, new List<string>())); // Simulate validation success

            var loggerMock = new Mock<ILogger<ProjectIncomesService>>();

            var service = new ProjectIncomesService(projectIncomeRepositoryMock.Object,
                                                     It.IsAny<IProjectRepository>(),
                                                     validatorMock.Object,
                                                     loggerMock.Object);

            // Act
            var result = await service.UpdateProjectIncomeAsync(projectIncome);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task CreateProjectIncomeAsync_ProjectIncomeAlreadyExists_ReturnsFailure()
        {
            // Arrange
            var projectIncome = new ProjectIncome
            {
                Id = "existing_project_income_id",
                Amount = 1000,
                Frequency = ProjectIncomeFrequency.Yearly,
                Project = new Project { Id = "valid_project_id" }
            };

            var projectIncomeRepositoryMock = new Mock<IProjectIncomeRepository>();
            projectIncomeRepositoryMock.Setup(repo => repo.GetByIdAsync(projectIncome.Id))
                                       .ReturnsAsync(projectIncome); // Simulate project income exists

            var loggerMock = new Mock<ILogger<ProjectIncomesService>>();

            var projectsRepositoryMock = new Mock<IProjectRepository>();
            projectsRepositoryMock.Setup(repo => repo.GetByIdAsync(projectIncome.Project.Id))
                                  .ReturnsAsync(new Project()); // Simulate project exists

            var validatorMock = new Mock<IValidator<ProjectIncome>>();
            validatorMock.Setup(validator => validator.ValidateAsync(projectIncome))
                         .ReturnsAsync((true, new List<string>())); // Simulate validation success

            var service = new ProjectIncomesService(projectIncomeRepositoryMock.Object,
                                                     projectsRepositoryMock.Object,
                                                     validatorMock.Object,
                                                     loggerMock.Object);

            // Act
            var result = await service.CreateProjectIncomeAsync(projectIncome);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Project income with such id already exists.", result.Errors);
        }

        [Fact]
        public async Task DeleteProjectIncomeAsync_InvalidId_ReturnsFailure()
        {
            // Arrange
            var invalidProjectIncomeId = "invalid_project_income_id";

            var projectIncomeRepositoryMock = new Mock<IProjectIncomeRepository>();
            projectIncomeRepositoryMock.Setup(repo => repo.DeleteAsync(invalidProjectIncomeId))
                                       .ThrowsAsync(new Exception("Database connection failed.")); // Simulate failure

            var loggerMock = new Mock<ILogger<ProjectIncomesService>>();

            var service = new ProjectIncomesService(projectIncomeRepositoryMock.Object,
                                                     It.IsAny<IProjectRepository>(),
                                                     It.IsAny<IValidator<ProjectIncome>>(),
                                                     loggerMock.Object);

            // Act
            var result = await service.DeleteProjectIncomeAsync(invalidProjectIncomeId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Contains("Can not delete the project income.", result.Errors);
        }

        [Fact]
        public async Task GetProjectIncomeByProjectIdAsync_InvalidProjectId_ReturnsFailure()
        {
            // Arrange
            var invalidProjectId = "invalid_project_id";

            var projectIncomeRepositoryMock = new Mock<IProjectIncomeRepository>();
            projectIncomeRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<ProjectIncomeFilterBuilder>>()))
                                       .ThrowsAsync(new Exception("Database connection failed.")); // Simulate failure

            var loggerMock = new Mock<ILogger<ProjectIncomesService>>();

            var service = new ProjectIncomesService(projectIncomeRepositoryMock.Object,
                                                     It.IsAny<IProjectRepository>(),
                                                     It.IsAny<IValidator<ProjectIncome>>(),
                                                     loggerMock.Object);

            // Act
            var result = await service.GetProjectIncomesByProjectIdAsync(invalidProjectId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Can not get project income by project id.", result.Errors);
        }

        [Fact]
        public async Task UpdateProjectIncomeAsync_InvalidInput_ReturnsFailure()
        {
            // Arrange
            var projectIncome = new ProjectIncome
            {
                Id = "invalid_project_income_id",
                Amount = -100, // Invalid amount
                Frequency = ProjectIncomeFrequency.Yearly
            };

            var projectIncomeRepositoryMock = new Mock<IProjectIncomeRepository>();
            projectIncomeRepositoryMock.Setup(repo => repo.GetByIdAsync(projectIncome.Id))
                                       .ReturnsAsync((ProjectIncome)null); // Simulate project income doesn't exist

            var validatorMock = new Mock<IValidator<ProjectIncome>>();
            validatorMock.Setup(validator => validator.ValidateAsync(projectIncome))
                         .ReturnsAsync((false, new List<string> { "Amount must be greater than 0." })); // Simulate validation failure

            var loggerMock = new Mock<ILogger<ProjectIncomesService>>();

            var service = new ProjectIncomesService(projectIncomeRepositoryMock.Object,
                                                     It.IsAny<IProjectRepository>(),
                                                     validatorMock.Object,
                                                     loggerMock.Object);

            // Act
            var result = await service.UpdateProjectIncomeAsync(projectIncome);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Contains("Amount must be greater than 0.", result.Errors);
        }
    }
}
