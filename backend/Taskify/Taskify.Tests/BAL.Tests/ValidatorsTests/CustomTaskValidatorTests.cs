using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;

namespace Taskify.Tests.BAL.Tests.ValidatorsTests
{
    public class CustomTaskValidatorTests
    {
        [Fact]
        public async Task ValidateAsync_ValidCustomTask_ReturnsValidResult()
        {
            // Arrange
            var customTask = new CustomTask
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Sample Task",
                SequenceNumber = 1
            };

            var validator = new CustomTaskValidator();

            // Act
            var result = await validator.ValidateAsync(customTask);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_InvalidId_ReturnsErrorMessage()
        {
            // Arrange
            var customTask = new CustomTask
            {
                Id = "invalid-id",
                Name = "Sample Task",
                SequenceNumber = 1
            };

            var validator = new CustomTaskValidator();

            // Act
            var result = await validator.ValidateAsync(customTask);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Invalid custom task Id format.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_NullOrEmptyName_ReturnsErrorMessage()
        {
            // Arrange
            var customTask = new CustomTask
            {
                Id = Guid.NewGuid().ToString(),
                Name = "", // Empty name
                SequenceNumber = 1
            };

            var validator = new CustomTaskValidator();

            // Act
            var result = await validator.ValidateAsync(customTask);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Custom task name cannot be null or empty.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_InvalidStoryPoints_ReturnsErrorMessage()
        {
            // Arrange
            var customTask = new CustomTask
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Sample Task",
                StoryPoints = 150,
                SequenceNumber = 1
            };

            var validator = new CustomTaskValidator();

            // Act
            var result = await validator.ValidateAsync(customTask);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("StoryPoints must be greater than 0, or null, or less than 100.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_NegativeSequenceNumber_ReturnsErrorMessage()
        {
            // Arrange
            var customTask = new CustomTask
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Sample Task",
                SequenceNumber = -1
            };

            var validator = new CustomTaskValidator();

            // Act
            var result = await validator.ValidateAsync(customTask);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("SequenceNumber must be greater than or equal to 0.", result.ErrorMessages);
        }
    }
}
