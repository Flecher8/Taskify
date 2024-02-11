using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;

namespace Taskify.Tests.BAL.Tests.ValidatorsTests
{
    public class ProjectValidatorTests
    {
        [Fact]
        public async Task ValidateAsync_ValidProject_ReturnsValidResult()
        {
            // Arrange
            var project = new Project
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Project 1"
            };

            var validator = new ProjectValidator();

            // Act
            var result = await validator.ValidateAsync(project);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_InvalidId_ReturnsErrorMessage()
        {
            // Arrange
            var project = new Project
            {
                Id = "invalid-id",
                Name = "Project 1"
            };

            var validator = new ProjectValidator();

            // Act
            var result = await validator.ValidateAsync(project);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Invalid project Id format.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_NullOrEmptyName_ReturnsErrorMessage()
        {
            // Arrange
            var project = new Project
            {
                Id = Guid.NewGuid().ToString(),
                Name = null // Invalid value
            };

            var validator = new ProjectValidator();

            // Act
            var result = await validator.ValidateAsync(project);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Project name cannot be null or empty.", result.ErrorMessages);
        }
    }
}
