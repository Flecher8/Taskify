using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
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
    public class SectionsServiceTests
    {
        [Fact]
        public async Task CreateSectionAsync_InvalidSection_ReturnsFailure()
        {
            // Arrange
            var section = new Section { /* Initialize with invalid data */ };
            var sectionRepositoryMock = new Mock<ISectionRepository>();
            var projectRepositoryMock = new Mock<IProjectRepository>();
            var customTaskRepositoryMock = new Mock<ICustomTaskRepository>();
            var validatorMock = new Mock<IValidator<Section>>();
            validatorMock.Setup(validator => validator.ValidateAsync(section))
                         .ReturnsAsync((false, new List<string>()));
            var loggerMock = new Mock<ILogger<SectionsService>>();
            var service = new SectionsService(sectionRepositoryMock.Object, projectRepositoryMock.Object, customTaskRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await service.CreateSectionAsync(section);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            sectionRepositoryMock.Verify(repo => repo.AddAsync(section), Times.Never);
        }

        [Fact]
        public async Task DeleteSectionAsync_InvalidSection_ReturnsFailure()
        {
            // Arrange
            var sectionId = "invalidId";
            var sectionRepositoryMock = new Mock<ISectionRepository>();
            sectionRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<SectionFilterBuilder>>()))
                .ReturnsAsync(new List<Section>());
            var customTaskRepositoryMock = new Mock<ICustomTaskRepository>();
            var loggerMock = new Mock<ILogger<SectionsService>>();
            var service = new SectionsService(
                                            sectionRepositoryMock.Object, 
                                            null, 
                                            customTaskRepositoryMock.Object, 
                                            null, 
                                            loggerMock.Object);

            // Act
            var result = await service.DeleteSectionAsync(sectionId);

            // Assert
            Assert.False(result.IsSuccess);
            sectionRepositoryMock.Verify(repo => repo.DeleteAsync(sectionId), Times.Never);
        }

        [Fact]
        public async Task DeleteSectionAsync_LastProjectSection_ReturnsFailure()
        {
            // Arrange
            var sectionId = "valid_section_id";
            var sectionToDelete = new Section { Id = sectionId, CustomTasks = new List<CustomTask>() }; // Create a section with no custom tasks

            var sectionRepositoryMock = new Mock<ISectionRepository>();
            sectionRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<SectionFilterBuilder>>()))
                                 .ReturnsAsync(new List<Section> { sectionToDelete });

            // Mocking the deletion of the section
            sectionRepositoryMock.Setup(repo => repo.DeleteAsync(sectionId))
                                 .Returns(Task.CompletedTask);

            // Mocking the retrieval of project sections after deletion
            sectionRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<SectionFilterBuilder>>()))
                                 .ReturnsAsync(new List<Section>());

            // Mocking the updating of sequence numbers
            sectionRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Section>()))
                                 .Returns(Task.CompletedTask);

            // Mocking the retrieval of project sections
            sectionRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<SectionFilterBuilder>>()))
                                 .ReturnsAsync(new List<Section> { sectionToDelete });

            var loggerMock = new Mock<ILogger<SectionsService>>();
            var service = new SectionsService(sectionRepositoryMock.Object, null, null, null, loggerMock.Object);

            // Act
            var result = await service.DeleteSectionAsync(sectionId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Can not delete the last section in the project.", result.Errors);
            sectionRepositoryMock.Verify(repo => repo.DeleteAsync(sectionId), Times.Never);
            sectionRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Section>()), Times.Never);
        }

        [Fact]
        public async Task GetSectionByIdAsync_ValidSectionId_ReturnsSuccess()
        {
            // Arrange
            var sectionId = "valid_section_id";
            var section = new Section { Id = sectionId };

            var sectionRepositoryMock = new Mock<ISectionRepository>();
            sectionRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<SectionFilterBuilder>>()))
                                 .ReturnsAsync(new List<Section> { section });

            var loggerMock = new Mock<ILogger<SectionsService>>();
            var service = new SectionsService(sectionRepositoryMock.Object, null, null, null, loggerMock.Object);

            // Act
            var result = await service.GetSectionByIdAsync(sectionId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(section, result.Data);
        }

        [Fact]
        public async Task GetSectionByIdAsync_InvalidSectionId_ReturnsFailure()
        {
            // Arrange
            var sectionId = "invalid_section_id";

            var sectionRepositoryMock = new Mock<ISectionRepository>();
            sectionRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<SectionFilterBuilder>>()))
                                 .ReturnsAsync(new List<Section>());

            var loggerMock = new Mock<ILogger<SectionsService>>();
            var service = new SectionsService(sectionRepositoryMock.Object, null, null, null, loggerMock.Object);

            // Act
            var result = await service.GetSectionByIdAsync(sectionId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Section with such id does not exist.", result.Errors);
        }

        [Fact]
        public async Task GetSectionsByProjectIdAsync_ValidProjectId_ReturnsSuccess()
        {
            // Arrange
            var projectId = "valid_project_id";
            var sections = new List<Section> { new Section(), new Section() };

            var sectionRepositoryMock = new Mock<ISectionRepository>();
            sectionRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<SectionFilterBuilder>>()))
                                 .ReturnsAsync(sections);

            var loggerMock = new Mock<ILogger<SectionsService>>();
            var service = new SectionsService(sectionRepositoryMock.Object, null, null, null, loggerMock.Object);

            // Act
            var result = await service.GetSectionsByProjectIdAsync(projectId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(sections, result.Data);
        }

        [Fact]
        public async Task GetSectionsByProjectIdAsync_InvalidProjectId_ReturnsFailure()
        {
            // Arrange
            var projectId = "invalid_project_id";

            var sectionRepositoryMock = new Mock<ISectionRepository>();
            sectionRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Expression<Func<Section, bool>>>()))
                                 .ReturnsAsync(new List<Section>());

            var loggerMock = new Mock<ILogger<SectionsService>>();
            var service = new SectionsService(sectionRepositoryMock.Object, null, null, null, loggerMock.Object);

            // Act
            var result = await service.GetSectionsByProjectIdAsync(projectId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Can not get sections by project id.", result.Errors);
        }

        [Fact]
        public async Task UpdateSectionAsync_ValidSection_ReturnsSuccess()
        {
            // Arrange
            var sectionId = "valid_section_id";
            var sectionToUpdate = new Section { Id = sectionId, Name = "Updated Name", SectionType = SectionType.ToDo, IsArchived = true };
            var existingSection = new Section { Id = sectionId }; // Mock existing section in the repository

            var sectionRepositoryMock = new Mock<ISectionRepository>();
            sectionRepositoryMock.Setup(repo => repo.GetByIdAsync(sectionId))
                                 .ReturnsAsync(existingSection);
            sectionRepositoryMock.Setup(repo => repo.UpdateAsync(existingSection))
                                 .Returns(Task.CompletedTask);

            var validatorMock = new Mock<IValidator<Section>>();
            validatorMock.Setup(validator => validator.ValidateAsync(sectionToUpdate))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<SectionsService>>();
            var service = new SectionsService(sectionRepositoryMock.Object, null, null, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await service.UpdateSectionAsync(sectionToUpdate);

            // Assert
            Assert.True(result.IsSuccess);
            sectionRepositoryMock.Verify(repo => repo.UpdateAsync(existingSection), Times.Once);
        }

        [Fact]
        public async Task UpdateSectionAsync_InvalidSectionId_ReturnsFailure()
        {
            // Arrange
            var sectionId = "invalid_section_id";
            var sectionToUpdate = new Section { Id = sectionId, Name = "Updated Name", SectionType = SectionType.ToDo, IsArchived = true };

            var sectionRepositoryMock = new Mock<ISectionRepository>();
            sectionRepositoryMock.Setup(repo => repo.GetByIdAsync(sectionId))
                                 .ReturnsAsync((Section)null); // Simulate section not found in the repository

            var validatorMock = new Mock<IValidator<Section>>();
            validatorMock.Setup(validator => validator.ValidateAsync(sectionToUpdate))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<SectionsService>>();
            var service = new SectionsService(sectionRepositoryMock.Object, null, null, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await service.UpdateSectionAsync(sectionToUpdate);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Can not find section with such id.", result.Errors);
            sectionRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Section>()), Times.Never);
        }

        [Fact]
        public async Task ArchiveSectionAsync_ValidSectionId_ReturnsSuccess()
        {
            // Arrange
            var sectionId = "valid_section_id";
            var sectionToArchive = new Section { Id = sectionId };

            var sectionRepositoryMock = new Mock<ISectionRepository>();
            sectionRepositoryMock.Setup(repo => repo.GetByIdAsync(sectionId))
                                 .ReturnsAsync(sectionToArchive);
            sectionRepositoryMock.Setup(repo => repo.UpdateAsync(sectionToArchive))
                                 .Returns(Task.CompletedTask);

            var loggerMock = new Mock<ILogger<SectionsService>>();
            var service = new SectionsService(sectionRepositoryMock.Object, null, null, null, loggerMock.Object);

            // Act
            var result = await service.ArchiveSectionAsync(sectionId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(sectionToArchive.IsArchived);
            sectionRepositoryMock.Verify(repo => repo.UpdateAsync(sectionToArchive), Times.Once);
        }

        [Fact]
        public async Task ArchiveSectionAsync_InvalidSectionId_ReturnsFailure()
        {
            // Arrange
            var sectionId = "invalid_section_id";

            var sectionRepositoryMock = new Mock<ISectionRepository>();
            sectionRepositoryMock.Setup(repo => repo.GetByIdAsync(sectionId))
                                 .ReturnsAsync((Section)null); // Simulate section not found in the repository

            var loggerMock = new Mock<ILogger<SectionsService>>();
            var service = new SectionsService(sectionRepositoryMock.Object, null, null, null, loggerMock.Object);

            // Act
            var result = await service.ArchiveSectionAsync(sectionId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Section with such id does not exist.", result.Errors);
            sectionRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Section>()), Times.Never);
        }

        [Fact]
        public async Task UnarchiveSectionAsync_ValidSectionId_ReturnsSuccess()
        {
            // Arrange
            var sectionId = "valid_section_id";
            var sectionToUnarchive = new Section { Id = sectionId, IsArchived = true };

            var sectionRepositoryMock = new Mock<ISectionRepository>();
            sectionRepositoryMock.Setup(repo => repo.GetByIdAsync(sectionId))
                                 .ReturnsAsync(sectionToUnarchive);
            sectionRepositoryMock.Setup(repo => repo.UpdateAsync(sectionToUnarchive))
                                 .Returns(Task.CompletedTask);

            var loggerMock = new Mock<ILogger<SectionsService>>();
            var service = new SectionsService(sectionRepositoryMock.Object, null, null, null, loggerMock.Object);

            // Act
            var result = await service.UnarchiveSectionAsync(sectionId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(sectionToUnarchive.IsArchived);
            sectionRepositoryMock.Verify(repo => repo.UpdateAsync(sectionToUnarchive), Times.Once);
        }

        [Fact]
        public async Task UnarchiveSectionAsync_InvalidSectionId_ReturnsFailure()
        {
            // Arrange
            var sectionId = "invalid_section_id";

            var sectionRepositoryMock = new Mock<ISectionRepository>();
            sectionRepositoryMock.Setup(repo => repo.GetByIdAsync(sectionId))
                                 .ReturnsAsync((Section)null); // Simulate section not found in the repository

            var loggerMock = new Mock<ILogger<SectionsService>>();
            var service = new SectionsService(sectionRepositoryMock.Object, null, null, null, loggerMock.Object);

            // Act
            var result = await service.UnarchiveSectionAsync(sectionId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Section with such id does not exist.", result.Errors);
            sectionRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Section>()), Times.Never);
        }
    }
}
