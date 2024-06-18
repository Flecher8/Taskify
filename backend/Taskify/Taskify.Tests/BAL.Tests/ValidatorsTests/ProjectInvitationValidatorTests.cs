using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;

namespace Taskify.Tests.BAL.Tests.ValidatorsTests
{
    public class ProjectInvitationValidatorTests
    {
        [Fact]
        public async Task ValidateAsync_ValidProjectInvitation_ReturnsValidResult()
        {
            // Arrange
            var projectInvitation = new ProjectInvitation
            {
                Id = Guid.NewGuid().ToString()
            };

            var validator = new ProjectInvitationValidator();

            // Act
            var result = await validator.ValidateAsync(projectInvitation);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_InvalidId_ReturnsErrorMessage()
        {
            // Arrange
            var projectInvitation = new ProjectInvitation
            {
                Id = "invalid-id"
            };

            var validator = new ProjectInvitationValidator();

            // Act
            var result = await validator.ValidateAsync(projectInvitation);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Invalid project invitation Id format.", result.ErrorMessages);
        }
    }
}
