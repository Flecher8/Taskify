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
    public class ProjectIncomeValidatorTests
    {
        [Fact]
        public async Task ValidateAsync_ValidProjectIncome_ReturnsValidResult()
        {
            // Arrange
            var projectIncome = new ProjectIncome
            {
                Id = Guid.NewGuid().ToString(),
                Amount = 100,
                Frequency = ProjectIncomeFrequency.Monthly
            };

            var validator = new ProjectIncomeValidator();

            // Act
            var result = await validator.ValidateAsync(projectIncome);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_InvalidId_ReturnsErrorMessage()
        {
            // Arrange
            var projectIncome = new ProjectIncome
            {
                Id = "invalid-id",
                Amount = 100,
                Frequency = ProjectIncomeFrequency.Monthly
            };

            var validator = new ProjectIncomeValidator();

            // Act
            var result = await validator.ValidateAsync(projectIncome);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Invalid project income Id format.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_AmountLessThanOrEqualToZero_ReturnsErrorMessage()
        {
            // Arrange
            var projectIncome = new ProjectIncome
            {
                Id = Guid.NewGuid().ToString(),
                Amount = 0,
                Frequency = ProjectIncomeFrequency.Monthly
            };

            var validator = new ProjectIncomeValidator();

            // Act
            var result = await validator.ValidateAsync(projectIncome);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Amount can not be less than 0.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_InvalidFrequency_ReturnsErrorMessage()
        {
            // Arrange
            var projectIncome = new ProjectIncome
            {
                Id = Guid.NewGuid().ToString(),
                Amount = 100,
                Frequency = (ProjectIncomeFrequency)100 // Invalid frequency
            };

            var validator = new ProjectIncomeValidator();

            // Act
            var result = await validator.ValidateAsync(projectIncome);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Invalid project income frequency.", result.ErrorMessages);
        }
    }
}
