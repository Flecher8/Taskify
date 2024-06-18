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
    public class NotificationValidatorTests
    {
        [Fact]
        public async Task ValidateAsync_ValidNotification_ReturnsValidResult()
        {
            // Arrange
            var notification = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                NotificationType = NotificationType.ProjectInvitation
            };

            var validator = new NotificationValidator();

            // Act
            var result = await validator.ValidateAsync(notification);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_InvalidId_ReturnsErrorMessage()
        {
            // Arrange
            var notification = new Notification
            {
                Id = "invalid-id",
                NotificationType = NotificationType.ProjectInvitation
            };

            var validator = new NotificationValidator();

            // Act
            var result = await validator.ValidateAsync(notification);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Invalid notification Id format.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_InvalidNotificationType_ReturnsErrorMessage()
        {
            // Arrange
            var notification = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                NotificationType = (NotificationType)100 // Invalid notification type
            };

            var validator = new NotificationValidator();

            // Act
            var result = await validator.ValidateAsync(notification);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Invalid notification type.", result.ErrorMessages);
        }
    }
}
