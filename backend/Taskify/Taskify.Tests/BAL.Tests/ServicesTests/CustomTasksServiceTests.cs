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
    public class CustomTasksServiceTests
    {
        [Fact]
        public async Task CreateCustomTaskAsync_ValidCustomTask_ReturnsSuccess()
        {
            // Arrange
            var customTask = new CustomTask();
            var section = new Section { Id = "valid_section_id" };
            customTask.Section = section;
            var customTaskRepositoryMock = new Mock<ICustomTaskRepository>();
            var sectionRepositoryMock = new Mock<ISectionRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var validatorMock = new Mock<IValidator<CustomTask>>();
            var loggerMock = new Mock<ILogger<CustomTasksService>>();
            var customTasksService = new CustomTasksService(
                                                            customTaskRepositoryMock.Object, 
                                                            sectionRepositoryMock.Object, 
                                                            userRepositoryMock.Object, 
                                                            validatorMock.Object, 
                                                            loggerMock.Object);

            sectionRepositoryMock.Setup(x => x.GetByIdAsync(customTask.Section.Id))
                                 .ReturnsAsync(section);

            customTaskRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CustomTaskFilterBuilder>>()))
                                     .ReturnsAsync(new List<CustomTask>());

            customTaskRepositoryMock.Setup(x => x.AddAsync(customTask))
                                     .ReturnsAsync(customTask);

            validatorMock.Setup(x => x.ValidateAsync(customTask))
                         .ReturnsAsync((true, new List<string>()));

            // Act
            var result = await customTasksService.CreateCustomTaskAsync(customTask);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(customTask, result.Data);
            customTaskRepositoryMock.Verify(x => x.AddAsync(customTask), Times.Once);
        }

        [Fact]
        public async Task CreateCustomTaskAsync_InvalidCustomTask_ReturnsFailure()
        {
            // Arrange
            var customTask = new CustomTask();
            var customTaskRepositoryMock = new Mock<ICustomTaskRepository>();
            var sectionRepositoryMock = new Mock<ISectionRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var validatorMock = new Mock<IValidator<CustomTask>>();
            var loggerMock = new Mock<ILogger<CustomTasksService>>();
            var customTasksService = new CustomTasksService(customTaskRepositoryMock.Object, sectionRepositoryMock.Object, userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            customTaskRepositoryMock.Setup(x => x.AddAsync(customTask))
                                     .ThrowsAsync(new Exception("Simulated error"));

            validatorMock.Setup(x => x.ValidateAsync(customTask))
                         .ReturnsAsync((false, new List<string> { "Validation error" }));

            // Act
            var result = await customTasksService.CreateCustomTaskAsync(customTask);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Validation error", result.Errors);
        }

        [Fact]
        public async Task DeleteCustomTaskAsync_ExistingId_ReturnsSuccess()
        {
            // Arrange
            var id = "existing_id";
            var customTaskRepositoryMock = new Mock<ICustomTaskRepository>();
            var sectionRepositoryMock = new Mock<ISectionRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var validatorMock = new Mock<IValidator<CustomTask>>();
            var loggerMock = new Mock<ILogger<CustomTasksService>>();
            var customTasksService = new CustomTasksService(customTaskRepositoryMock.Object, sectionRepositoryMock.Object, userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            customTaskRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CustomTaskFilterBuilder>>()))
                                     .ReturnsAsync(new List<CustomTask> { new CustomTask { Id = id } });

            customTaskRepositoryMock.Setup(x => x.DeleteAsync(id))
                                     .Returns(Task.CompletedTask);

            // Act
            var result = await customTasksService.DeleteCustomTaskAsync(id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            customTaskRepositoryMock.Verify(x => x.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteCustomTaskAsync_NonExistingId_ReturnsFailure()
        {
            // Arrange
            var id = "non_existing_id";
            var customTaskRepositoryMock = new Mock<ICustomTaskRepository>();
            var sectionRepositoryMock = new Mock<ISectionRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var validatorMock = new Mock<IValidator<CustomTask>>();
            var loggerMock = new Mock<ILogger<CustomTasksService>>();
            var customTasksService = new CustomTasksService(customTaskRepositoryMock.Object, sectionRepositoryMock.Object, userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            customTaskRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CustomTaskFilterBuilder>>()))
                                     .ReturnsAsync(new List<CustomTask>());

            // Act
            var result = await customTasksService.DeleteCustomTaskAsync(id);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Contains("Custom task with such id does not exist.", result.Errors);
        }

        [Fact]
        public async Task GetCustomTaskByIdAsync_ExistingId_ReturnsSuccess()
        {
            // Arrange
            var id = "existing_id";
            var customTask = new CustomTask { Id = id };
            var customTaskRepositoryMock = new Mock<ICustomTaskRepository>();
            var sectionRepositoryMock = new Mock<ISectionRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var validatorMock = new Mock<IValidator<CustomTask>>();
            var loggerMock = new Mock<ILogger<CustomTasksService>>();
            var customTasksService = new CustomTasksService(customTaskRepositoryMock.Object, sectionRepositoryMock.Object, userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            customTaskRepositoryMock.Setup(x => x.GetByIdAsync(id))
                                     .ReturnsAsync(customTask);

            // Act
            var result = await customTasksService.GetCustomTaskByIdAsync(id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(customTask, result.Data);
            customTaskRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetCustomTaskByIdAsync_NonExistingId_ReturnsFailure()
        {
            // Arrange
            var id = "non_existing_id";
            var customTaskRepositoryMock = new Mock<ICustomTaskRepository>();
            var sectionRepositoryMock = new Mock<ISectionRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var validatorMock = new Mock<IValidator<CustomTask>>();
            var loggerMock = new Mock<ILogger<CustomTasksService>>();
            var customTasksService = new CustomTasksService(customTaskRepositoryMock.Object, sectionRepositoryMock.Object, userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            customTaskRepositoryMock.Setup(x => x.GetByIdAsync(id))
                                     .ReturnsAsync((CustomTask)null);

            // Act
            var result = await customTasksService.GetCustomTaskByIdAsync(id);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Custom task with such id does not exist.", result.Errors);
        }

        [Fact]
        public async Task GetCustomTasksBySectionIdAsync_ExistingId_ReturnsSuccess()
        {
            // Arrange
            var sectionId = "existing_section_id";
            var customTasks = new List<CustomTask> { new CustomTask(), new CustomTask() };
            var customTaskRepositoryMock = new Mock<ICustomTaskRepository>();
            customTaskRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CustomTaskFilterBuilder>>()))
                                     .ReturnsAsync(customTasks);
            var sectionRepositoryMock = new Mock<ISectionRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var validatorMock = new Mock<IValidator<CustomTask>>();
            var loggerMock = new Mock<ILogger<CustomTasksService>>();
            var customTasksService = new CustomTasksService(customTaskRepositoryMock.Object, sectionRepositoryMock.Object, userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await customTasksService.GetCustomTasksBySectionIdAsync(sectionId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(customTasks, result.Data);
            customTaskRepositoryMock.Verify(x => x.GetFilteredItemsAsync(It.IsAny<Action<CustomTaskFilterBuilder>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCustomTaskAsync_ValidTask_ReturnsSuccess()
        {
            // Arrange
            var customTask = new CustomTask { Id = "valid_id" };
            var customTaskRepositoryMock = new Mock<ICustomTaskRepository>();
            customTaskRepositoryMock.Setup(x => x.GetByIdAsync(customTask.Id))
                                     .ReturnsAsync(customTask);
            customTaskRepositoryMock.Setup(x => x.UpdateAsync(customTask))
                                     .Returns(Task.CompletedTask);
            var sectionRepositoryMock = new Mock<ISectionRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var validatorMock = new Mock<IValidator<CustomTask>>();
            validatorMock.Setup(x => x.ValidateAsync(customTask))
                         .ReturnsAsync((true, new List<string>()));
            var loggerMock = new Mock<ILogger<CustomTasksService>>();
            var customTasksService = new CustomTasksService(customTaskRepositoryMock.Object, sectionRepositoryMock.Object, userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await customTasksService.UpdateCustomTaskAsync(customTask);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            customTaskRepositoryMock.Verify(x => x.UpdateAsync(customTask), Times.Once);
        }

        [Fact]
        public async Task UpdateCustomTaskAsync_InvalidTask_ReturnsFailure()
        {
            // Arrange
            var customTask = new CustomTask { Id = "invalid_id" };
            var customTaskRepositoryMock = new Mock<ICustomTaskRepository>();
            customTaskRepositoryMock.Setup(x => x.GetByIdAsync(customTask.Id))
                                     .ReturnsAsync((CustomTask)null);
            var sectionRepositoryMock = new Mock<ISectionRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var validatorMock = new Mock<IValidator<CustomTask>>();
            validatorMock.Setup(x => x.ValidateAsync(customTask))
                .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<CustomTasksService>>();
            var customTasksService = new CustomTasksService(customTaskRepositoryMock.Object, sectionRepositoryMock.Object, userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await customTasksService.UpdateCustomTaskAsync(customTask);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Contains("Can not find custom task with such id.", result.Errors);
        }

        [Fact]
        public async Task MoveCustomTaskAsync_ValidInput_ReturnsSuccess()
        {
            // Arrange
            var customTaskId = "valid_id";
            var targetSequenceNumber = 0;
            var customTaskWithSection = new List<CustomTask> { new CustomTask { Id = customTaskId, Section = new Section { Id = "section_id" } } };
            var customTaskRepositoryMock = new Mock<ICustomTaskRepository>();
            customTaskRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CustomTaskFilterBuilder>>()))
                                     .ReturnsAsync(customTaskWithSection);
            customTaskRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<CustomTask>()))
                                     .Returns(Task.CompletedTask);
            var sectionRepositoryMock = new Mock<ISectionRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var validatorMock = new Mock<IValidator<CustomTask>>();
            var loggerMock = new Mock<ILogger<CustomTasksService>>();
            var customTasksService = new CustomTasksService(customTaskRepositoryMock.Object, sectionRepositoryMock.Object, userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await customTasksService.MoveCustomTaskAsync(customTaskId, targetSequenceNumber);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task ArchiveCustomTaskAsync_ValidInput_ReturnsSuccess()
        {
            // Arrange
            var customTaskId = "valid_id";
            var customTask = new CustomTask { Id = customTaskId };
            var customTaskRepositoryMock = new Mock<ICustomTaskRepository>();
            customTaskRepositoryMock.Setup(x => x.GetByIdAsync(customTaskId))
                                     .ReturnsAsync(customTask);
            customTaskRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<CustomTask>()))
                                     .Returns(Task.CompletedTask);
            var sectionRepositoryMock = new Mock<ISectionRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var validatorMock = new Mock<IValidator<CustomTask>>();
            var loggerMock = new Mock<ILogger<CustomTasksService>>();
            var customTasksService = new CustomTasksService(customTaskRepositoryMock.Object, sectionRepositoryMock.Object, userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await customTasksService.ArchiveCustomTaskAsync(customTaskId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            customTaskRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<CustomTask>()), Times.Once);
        }

        [Fact]
        public async Task UnarchiveCustomTaskAsync_ValidInput_ReturnsSuccess()
        {
            // Arrange
            var customTaskId = "valid_id";
            var customTask = new CustomTask { Id = customTaskId };
            var customTaskRepositoryMock = new Mock<ICustomTaskRepository>();
            customTaskRepositoryMock.Setup(x => x.GetByIdAsync(customTaskId))
                                     .ReturnsAsync(customTask);
            customTaskRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<CustomTask>()))
                                     .Returns(Task.CompletedTask);
            var sectionRepositoryMock = new Mock<ISectionRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var validatorMock = new Mock<IValidator<CustomTask>>();
            var loggerMock = new Mock<ILogger<CustomTasksService>>();
            var customTasksService = new CustomTasksService(customTaskRepositoryMock.Object, sectionRepositoryMock.Object, userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await customTasksService.UnarchiveCustomTaskAsync(customTaskId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            customTaskRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<CustomTask>()), Times.Once);
        }

        [Fact]
        public async Task MoveCustomTaskAsync_CustomTaskNotFound_ReturnsFailure()
        {
            // Arrange
            var customTaskId = "invalid_id";
            var customTaskRepositoryMock = new Mock<ICustomTaskRepository>();
            customTaskRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CustomTaskFilterBuilder>>()))
                                     .ReturnsAsync(new List<CustomTask>());
            var sectionRepositoryMock = new Mock<ISectionRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var validatorMock = new Mock<IValidator<CustomTask>>();
            var loggerMock = new Mock<ILogger<CustomTasksService>>();
            var customTasksService = new CustomTasksService(customTaskRepositoryMock.Object, sectionRepositoryMock.Object, userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await customTasksService.MoveCustomTaskAsync(customTaskId, 0);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Contains("Custom task with such id does not exist.", result.Errors);
        }

        [Fact]
        public async Task ArchiveCustomTaskAsync_CustomTaskNotFound_ReturnsFailure()
        {
            // Arrange
            var customTaskId = "invalid_id";
            var customTaskRepositoryMock = new Mock<ICustomTaskRepository>();
            customTaskRepositoryMock.Setup(x => x.GetByIdAsync(customTaskId))
                                     .ReturnsAsync((CustomTask)null);
            var sectionRepositoryMock = new Mock<ISectionRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var validatorMock = new Mock<IValidator<CustomTask>>();
            var loggerMock = new Mock<ILogger<CustomTasksService>>();
            var customTasksService = new CustomTasksService(customTaskRepositoryMock.Object, sectionRepositoryMock.Object, userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await customTasksService.ArchiveCustomTaskAsync(customTaskId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Contains("Custom task with such id does not exist.", result.Errors);
        }

        [Fact]
        public async Task UnarchiveCustomTaskAsync_CustomTaskNotFound_ReturnsFailure()
        {
            // Arrange
            var customTaskId = "invalid_id";
            var customTaskRepositoryMock = new Mock<ICustomTaskRepository>();
            customTaskRepositoryMock.Setup(x => x.GetByIdAsync(customTaskId))
                                     .ReturnsAsync((CustomTask)null);
            var sectionRepositoryMock = new Mock<ISectionRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var validatorMock = new Mock<IValidator<CustomTask>>();
            var loggerMock = new Mock<ILogger<CustomTasksService>>();
            var customTasksService = new CustomTasksService(customTaskRepositoryMock.Object, sectionRepositoryMock.Object, userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await customTasksService.UnarchiveCustomTaskAsync(customTaskId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Contains("Custom task with such id does not exist.", result.Errors);
        }

        [Fact]
        public async Task RedirectCustomTaskAsync_CustomTaskNotFound_ReturnsFailure()
        {
            // Arrange
            var customTaskId = "invalid_id";
            var targetSectionId = "valid_id";
            var targetSequenceNumber = 0;
            var customTaskRepositoryMock = new Mock<ICustomTaskRepository>();
            customTaskRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CustomTaskFilterBuilder>>()))
                                     .ReturnsAsync(new List<CustomTask>());
            var sectionRepositoryMock = new Mock<ISectionRepository>();
            sectionRepositoryMock.Setup(x => x.GetByIdAsync(targetSectionId))
                                  .ReturnsAsync(new Section());
            var userRepositoryMock = new Mock<IUserRepository>();
            var validatorMock = new Mock<IValidator<CustomTask>>();
            var loggerMock = new Mock<ILogger<CustomTasksService>>();
            var customTasksService = new CustomTasksService(customTaskRepositoryMock.Object, sectionRepositoryMock.Object, userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await customTasksService.RedirectCustomTaskAsync(customTaskId, targetSectionId, targetSequenceNumber);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Contains("Custom task with such id does not exist.", result.Errors);
        }
    }
}
