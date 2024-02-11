using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;

namespace Taskify.Tests.BAL.Tests.ValidatorsTests
{
    public class SubscriptionValidatorTests
    {
        [Fact]
        public async Task ValidateAsync_ValidSubscription_ReturnsValidResult()
        {
            // Arrange
            var subscription = new Subscription
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Basic",
                PricePerMonth = 10,
                DurationInDays = 30,
                ProjectsLimit = 5,
                ProjectMembersLimit = 10,
                ProjectSectionsLimit = 20,
                ProjectTasksLimit = 50
            };

            var validator = new SubscriptionValidator();

            // Act
            var result = await validator.ValidateAsync(subscription);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_InvalidId_ReturnsErrorMessage()
        {
            // Arrange
            var subscription = new Subscription
            {
                Id = "invalid-id",
                Name = "Basic",
                PricePerMonth = 10,
                DurationInDays = 30,
                ProjectsLimit = 5,
                ProjectMembersLimit = 10,
                ProjectSectionsLimit = 20,
                ProjectTasksLimit = 50
            };

            var validator = new SubscriptionValidator();

            // Act
            var result = await validator.ValidateAsync(subscription);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Id must be a valid Guid string.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_InvalidNameLength_ReturnsErrorMessage()
        {
            // Arrange
            var subscription = new Subscription
            {
                Id = Guid.NewGuid().ToString(),
                Name = "B", // Name length less than minimum
                PricePerMonth = 10,
                DurationInDays = 30,
                ProjectsLimit = 5,
                ProjectMembersLimit = 10,
                ProjectSectionsLimit = 20,
                ProjectTasksLimit = 50
            };

            var validator = new SubscriptionValidator();

            // Act
            var result = await validator.ValidateAsync(subscription);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Name length must be between 3 and 50 characters.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_DurationInDaysLessThanOne_ReturnsErrorMessage()
        {
            // Arrange
            var subscription = new Subscription
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Basic",
                PricePerMonth = 10,
                DurationInDays = 0, // Invalid value
                ProjectsLimit = 5,
                ProjectMembersLimit = 10,
                ProjectSectionsLimit = 20,
                ProjectTasksLimit = 50
            };

            var validator = new SubscriptionValidator();

            // Act
            var result = await validator.ValidateAsync(subscription);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Duration in days must be greater than 0.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_ProjectsLimitLessThanOne_ReturnsErrorMessage()
        {
            // Arrange
            var subscription = new Subscription
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Basic",
                PricePerMonth = 10,
                DurationInDays = 30,
                ProjectsLimit = 0, // Invalid value
                ProjectMembersLimit = 10,
                ProjectSectionsLimit = 20,
                ProjectTasksLimit = 50
            };

            var validator = new SubscriptionValidator();

            // Act
            var result = await validator.ValidateAsync(subscription);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Projects limit must be greater than 0.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_ProjectMembersLimitLessThanOne_ReturnsErrorMessage()
        {
            // Arrange
            var subscription = new Subscription
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Basic",
                PricePerMonth = 10,
                DurationInDays = 30,
                ProjectsLimit = 5,
                ProjectMembersLimit = 0, // Invalid value
                ProjectSectionsLimit = 20,
                ProjectTasksLimit = 50
            };

            var validator = new SubscriptionValidator();

            // Act
            var result = await validator.ValidateAsync(subscription);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Project members limit must be greater than 0.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_ProjectSectionsLimitLessThanOne_ReturnsErrorMessage()
        {
            // Arrange
            var subscription = new Subscription
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Basic",
                PricePerMonth = 10,
                DurationInDays = 30,
                ProjectsLimit = 5,
                ProjectMembersLimit = 10,
                ProjectSectionsLimit = 0, // Invalid value
                ProjectTasksLimit = 50
            };

            var validator = new SubscriptionValidator();

            // Act
            var result = await validator.ValidateAsync(subscription);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Project sections limit must be greater than 0.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_ProjectTasksLimitLessThanOne_ReturnsErrorMessage()
        {
            // Arrange
            var subscription = new Subscription
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Basic",
                PricePerMonth = 10,
                DurationInDays = 30,
                ProjectsLimit = 5,
                ProjectMembersLimit = 10,
                ProjectSectionsLimit = 20,
                ProjectTasksLimit = 0 // Invalid value
            };

            var validator = new SubscriptionValidator();

            // Act
            var result = await validator.ValidateAsync(subscription);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Project tasks limit must be greater than 0.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_AllErrors_ReturnsAllErrorMessages()
        {
            // Arrange
            var subscription = new Subscription
            {
                Id = "invalid-id",
                Name = null, // Invalid value
                PricePerMonth = -10, // Invalid value
                DurationInDays = 0, // Invalid value
                ProjectsLimit = 0, // Invalid value
                ProjectMembersLimit = 0, // Invalid value
                ProjectSectionsLimit = 0, // Invalid value
                ProjectTasksLimit = 0 // Invalid value
            };

            var validator = new SubscriptionValidator();

            // Act
            var result = await validator.ValidateAsync(subscription);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Id must be a valid Guid string.", result.ErrorMessages);
            Assert.Contains("Name cannot be null or empty.", result.ErrorMessages);
            Assert.Contains("Price per month must be greater than 0.", result.ErrorMessages);
            Assert.Contains("Duration in days must be greater than 0.", result.ErrorMessages);
            Assert.Contains("Projects limit must be greater than 0.", result.ErrorMessages);
            Assert.Contains("Project members limit must be greater than 0.", result.ErrorMessages);
            Assert.Contains("Project sections limit must be greater than 0.", result.ErrorMessages);
            Assert.Contains("Project tasks limit must be greater than 0.", result.ErrorMessages);
        }
    }
}
