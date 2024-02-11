using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;

namespace Taskify.Tests.BAL.Tests.ValidatorsTests
{
    public class CompanyValidatorTests
    {
        [Fact]
        public async Task ValidateAsync_ValidCompany_ReturnsValidResult()
        {
            // Arrange
            var company = new Company
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Sample Company"
            };

            var validator = new CompanyValidator();

            // Act
            var result = await validator.ValidateAsync(company);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_InvalidId_ReturnsErrorMessage()
        {
            // Arrange
            var company = new Company
            {
                Id = "invalid-id",
                Name = "Sample Company"
            };

            var validator = new CompanyValidator();

            // Act
            var result = await validator.ValidateAsync(company);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Invalid company Id format.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_NullOrEmptyName_ReturnsErrorMessage()
        {
            // Arrange
            var company = new Company
            {
                Id = Guid.NewGuid().ToString(),
                Name = "", // Empty name
            };

            var validator = new CompanyValidator();

            // Act
            var result = await validator.ValidateAsync(company);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Company name cannot be null or empty.", result.ErrorMessages);
        }
    }
}
