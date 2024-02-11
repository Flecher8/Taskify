using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;

namespace Taskify.Tests.BAL.Tests.ValidatorsTests
{
    public class ProjectMemberValidatorTests
    {
        [Fact]
        public async Task ValidateAsync_ValidProjectMember_ReturnsValidResult()
        {
            // Arrange
            var projectMember = new ProjectMember
            {
                Id = Guid.NewGuid().ToString()
            };

            var validator = new ProjectMemberValidator();

            // Act
            var result = await validator.ValidateAsync(projectMember);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_InvalidId_ReturnsErrorMessage()
        {
            // Arrange
            var projectMember = new ProjectMember
            {
                Id = "invalid-id"
            };

            var validator = new ProjectMemberValidator();

            // Act
            var result = await validator.ValidateAsync(projectMember);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Invalid project member Id format.", result.ErrorMessages);
        }
    }
}
