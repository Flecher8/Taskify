using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;

namespace Taskify.Tests.BAL.Tests.ValidatorsTests
{
    public class CompanyInvitationValidatorTests
    {
        [Fact]
        public async Task ValidateAsync_ValidCompanyInvitation_ReturnsValidResult()
        {
            // Arrange
            var companyInvitation = new CompanyInvitation
            {
                Id = Guid.NewGuid().ToString(),
            };

            var validator = new CompanyInvitationValidator();

            // Act
            var result = await validator.ValidateAsync(companyInvitation);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_InvalidId_ReturnsErrorMessage()
        {
            // Arrange
            var companyInvitation = new CompanyInvitation
            {
                Id = "invalid-id",
            };

            var validator = new CompanyInvitationValidator();

            // Act
            var result = await validator.ValidateAsync(companyInvitation);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Invalid project invitation Id format.", result.ErrorMessages);
        }
    }
}
