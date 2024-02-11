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
    public class CompanyExpenseValidatorTests
    {
        [Fact]
        public async Task ValidateAsync_ValidCompanyExpense_ReturnsValidResult()
        {
            // Arrange
            var companyExpense = new CompanyExpense
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Hello",
            };

            var validator = new CompanyExpenseValidator();

            // Act
            var result = await validator.ValidateAsync(companyExpense);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_InvalidId_ReturnsErrorMessage()
        {
            // Arrange
            var companyExpense = new CompanyExpense
            {
                Id = "invalid-id",
            };

            var validator = new CompanyExpenseValidator();

            // Act
            var result = await validator.ValidateAsync(companyExpense);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Invalid company expense Id format.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_NegativeAmount_ReturnsErrorMessage()
        {
            // Arrange
            var companyExpense = new CompanyExpense
            {
                Id = Guid.NewGuid().ToString(),
                Amount = -10,
            };

            var validator = new CompanyExpenseValidator();

            // Act
            var result = await validator.ValidateAsync(companyExpense);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Amount can not be less than 0.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_NullOrEmptyName_ReturnsErrorMessage()
        {
            // Arrange
            var companyExpense = new CompanyExpense
            {
                Id = Guid.NewGuid().ToString(),
                Name = null,
            };

            var validator = new CompanyExpenseValidator();

            // Act
            var result = await validator.ValidateAsync(companyExpense);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Company expense name can not be null or empty.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_InvalidFrequency_ReturnsErrorMessage()
        {
            // Arrange
            var companyExpense = new CompanyExpense
            {
                Id = Guid.NewGuid().ToString(),
                Frequency = (CompanyExpenseFrequency)100, // Invalid value
            };

            var validator = new CompanyExpenseValidator();

            // Act
            var result = await validator.ValidateAsync(companyExpense);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Invalid company expense frequency.", result.ErrorMessages);
        }
    }
}
