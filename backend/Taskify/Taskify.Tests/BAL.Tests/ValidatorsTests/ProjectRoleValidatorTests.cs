using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;
using Taskify.Core.Enums;

namespace Taskify.Tests.BAL.Tests.ValidatorsTests
{
    public class ProjectRoleValidatorTests
    {
        [Fact]
        public async Task ValidateAsync_ValidProjectRole_ReturnsValidResult()
        {
            // Arrange
            var projectRole = new ProjectRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Administrator",
                ProjectRoleType = ProjectRoleType.Admin
            };

            var validator = new ProjectRoleValidator();

            // Act
            var result = await validator.ValidateAsync(projectRole);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_InvalidId_ReturnsErrorMessage()
        {
            // Arrange
            var projectRole = new ProjectRole
            {
                Id = "invalid-id",
                Name = "Administrator",
                ProjectRoleType = ProjectRoleType.Admin
            };

            var validator = new ProjectRoleValidator();

            // Act
            var result = await validator.ValidateAsync(projectRole);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Invalid project role Id format.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_NullOrEmptyName_ReturnsErrorMessage()
        {
            // Arrange
            var projectRole = new ProjectRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = null, // Invalid value
                ProjectRoleType = ProjectRoleType.Admin
            };

            var validator = new ProjectRoleValidator();

            // Act
            var result = await validator.ValidateAsync(projectRole);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Project role name cannot be null or empty.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_InvalidRoleType_ReturnsErrorMessage()
        {
            // Arrange
            var projectRole = new ProjectRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Administrator",
                ProjectRoleType = (ProjectRoleType)100 // Invalid value
            };

            var validator = new ProjectRoleValidator();

            // Act
            var result = await validator.ValidateAsync(projectRole);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Invalid project role type.", result.ErrorMessages);
        }
    }
}
