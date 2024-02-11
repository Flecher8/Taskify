using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;

namespace Taskify.Tests.BAL.Tests.ValidatorsTests
{
    public class CompanyMemberValidatorTests
    {
        [Fact]
        public async Task ValidateAsync_ValidCompanyMember_ReturnsValidResult()
        {
            // Arrange
            var companyMember = new CompanyMember
            {
                Id = Guid.NewGuid().ToString(),
                Salary = 1000 // Assuming a valid salary
            };

            var validator = new CompanyMemberValidator();

            // Act
            var result = await validator.ValidateAsync(companyMember);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_InvalidId_ReturnsErrorMessage()
        {
            // Arrange
            var companyMember = new CompanyMember
            {
                Id = "invalid-id",
                Salary = 1000 // Assuming a valid salary
            };

            var validator = new CompanyMemberValidator();

            // Act
            var result = await validator.ValidateAsync(companyMember);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Invalid company member Id format.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_NegativeSalary_ReturnsErrorMessage()
        {
            // Arrange
            var companyMember = new CompanyMember
            {
                Id = Guid.NewGuid().ToString(),
                Salary = -100 // Negative salary
            };

            var validator = new CompanyMemberValidator();

            // Act
            var result = await validator.ValidateAsync(companyMember);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Salary can not be less than 0.", result.ErrorMessages);
        }
    }
}
