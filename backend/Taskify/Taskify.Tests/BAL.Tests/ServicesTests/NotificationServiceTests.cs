using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Services;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;
using Taskify.DAL.Helpers;
using Taskify.DAL.Interfaces;

namespace Taskify.Tests.BAL.Tests.ServicesTests
{
    public class NotificationServiceTests
    {
        [Fact]
        public async Task CreateNotificationAsync_ValidNotification_ReturnsSuccess()
        {
            // Arrange
            var notification = new Notification();
            notification.User = new User();
            notification.User.Id = Guid.NewGuid().ToString();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetByIdAsync(notification.User.Id))
                              .ReturnsAsync(new User { Id = "1", FirstName = "Test" , LastName = "Test", Email = "email@gmail.com"});

            var notificationRepositoryMock = new Mock<INotificationRepository>();
            notificationRepositoryMock.Setup(x => x.AddAsync(notification))
                                      .ReturnsAsync(notification);

            var validatorMock = new Mock<IValidator<Notification>>();

            validatorMock.Setup(x => x.ValidateAsync(notification))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<NotificationService>>();

            var notificationService = new NotificationService(notificationRepositoryMock.Object,
                                                              userRepositoryMock.Object,
                                                              validatorMock.Object,
                                                              loggerMock.Object);

            // Act
            var result = await notificationService.CreateNotificationAsync(notification);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(notification, result.Data);
            notificationRepositoryMock.Verify(x => x.AddAsync(notification), Times.Once);
        }

        [Fact]
        public async Task CreateNotificationAsync_InvalidNotificationId_ReturnsFailure()
        {
            // Arrange
            var notification = new Notification();
            notification.User = new User();
            notification.User.Id = "invalid_id"; // Setting an invalid ID

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetByIdAsync(notification.User.Id))
                                    .ReturnsAsync((User)null);

            var notificationRepositoryMock = new Mock<INotificationRepository>();
            var validatorMock = new Mock<IValidator<Notification>>();
            validatorMock.Setup(x => x.ValidateAsync(notification))
                .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<NotificationService>>();

            var notificationService = new NotificationService(notificationRepositoryMock.Object,
                                                              userRepositoryMock.Object,
                                                              validatorMock.Object,
                                                              loggerMock.Object);

            // Act
            var result = await notificationService.CreateNotificationAsync(notification);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Can not find user with such id.", result.Errors);
            notificationRepositoryMock.Verify(x => x.AddAsync(notification), Times.Never);
        }

        [Fact]
        public async Task CreateNotificationAsync_NotificationRepositoryReturnsError_ReturnsFailure()
        {
            // Arrange
            var notification = new Notification();
            notification.User = new User();
            notification.User.Id = Guid.NewGuid().ToString();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetByIdAsync(notification.User.Id))
                              .ReturnsAsync(new User { Id = "1", FirstName = "Test", LastName = "Test", Email = "email@gmail.com" });

            var notificationRepositoryMock = new Mock<INotificationRepository>();
            notificationRepositoryMock.Setup(x => x.AddAsync(notification))
                                      .ThrowsAsync(new Exception("Database error")); // Simulating a repository error

            var validatorMock = new Mock<IValidator<Notification>>();
            validatorMock.Setup(x => x.ValidateAsync(notification))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<NotificationService>>();

            var notificationService = new NotificationService(notificationRepositoryMock.Object,
                                                              userRepositoryMock.Object,
                                                              validatorMock.Object,
                                                              loggerMock.Object);

            // Act
            var result = await notificationService.CreateNotificationAsync(notification);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Can not create a new notification.", result.Errors);
        }

        [Fact]
        public async Task CreateNotificationAsync_InvalidNotificationValidation_ReturnsFailure()
        {
            // Arrange
            var notification = new Notification();
            notification.User = new User();
            notification.User.Id = Guid.NewGuid().ToString();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetByIdAsync(notification.User.Id))
                              .ReturnsAsync(new User { Id = "1", FirstName = "Test", LastName = "Test", Email = "email@gmail.com" });

            var notificationRepositoryMock = new Mock<INotificationRepository>();
            var validatorMock = new Mock<IValidator<Notification>>();
            validatorMock.Setup(x => x.ValidateAsync(notification))
                         .ReturnsAsync((false, new List<string> { "Validation error" })); // Simulating a validation error

            var loggerMock = new Mock<ILogger<NotificationService>>();

            var notificationService = new NotificationService(notificationRepositoryMock.Object,
                                                              userRepositoryMock.Object,
                                                              validatorMock.Object,
                                                              loggerMock.Object);

            // Act
            var result = await notificationService.CreateNotificationAsync(notification);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Validation error", result.Errors);
            notificationRepositoryMock.Verify(x => x.AddAsync(notification), Times.Never);
        }

        [Fact]
        public async Task MarkNotificationAsReadAsync_ValidNotificationId_ReturnsSuccess()
        {
            // Arrange
            var notificationId = "valid_notification_id";
            var notification = new Notification { Id = notificationId };
            var notificationRepositoryMock = new Mock<INotificationRepository>();

            notificationRepositoryMock.Setup(x =>
                x.GetFilteredItemsAsync(It.IsAny<Action<NotificationFilterBuilder>>()))
                    .ReturnsAsync(new List<Notification> { notification });

            notificationRepositoryMock.Setup(x => x.UpdateAsync(notification))
                          .Returns(Task.CompletedTask);

            var validatorMock = new Mock<IValidator<Notification>>();

            var loggerMock = new Mock<ILogger<NotificationService>>();
            var notificationService = new NotificationService(notificationRepositoryMock.Object,
                                                              It.IsAny<IUserRepository>(), // Mocked UserRepository
                                                              validatorMock.Object, // Pass the validator mock object
                                                              loggerMock.Object);

            // Act
            var result = await notificationService.MarkNotificationAsReadAsync(notificationId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task MarkNotificationAsReadAsync_InvalidNotificationId_ReturnsFailure()
        {
            // Arrange
            var notificationId = "invalid_notification_id";
            var notificationRepositoryMock = new Mock<INotificationRepository>();

            notificationRepositoryMock.Setup(x =>
                x.GetFilteredItemsAsync(It.IsAny<Action<NotificationFilterBuilder>>()))
                    .ReturnsAsync(new List<Notification>());

            var validatorMock = new Mock<IValidator<Notification>>();

            var loggerMock = new Mock<ILogger<NotificationService>>();
            var notificationService = new NotificationService(notificationRepositoryMock.Object,
                                                              It.IsAny<IUserRepository>(), // Mocked UserRepository
                                                              validatorMock.Object, // Pass the validator mock object
                                                              loggerMock.Object);

            // Act
            var result = await notificationService.MarkNotificationAsReadAsync(notificationId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Notification with such id does not exist.", result.Errors.Single());
        }

        [Fact]
        public async Task GetNotificationsByUserIdAsync_ValidUserId_ReturnsSuccess()
        {
            // Arrange
            var userId = "valid_user_id";
            var notifications = new List<Notification> { new Notification(), new Notification() };

            var notificationRepositoryMock = new Mock<INotificationRepository>();
            notificationRepositoryMock.Setup(x =>
                x.GetFilteredItemsAsync(It.IsAny<Action<NotificationFilterBuilder>>()))
                    .ReturnsAsync(notifications);

            var loggerMock = new Mock<ILogger<NotificationService>>();
            var notificationService = new NotificationService(notificationRepositoryMock.Object,
                                                              It.IsAny<IUserRepository>(), // Mocked UserRepository
                                                              It.IsAny<IValidator<Notification>>(), // Mocked Validator
                                                              loggerMock.Object);

            // Act
            var result = await notificationService.GetNotificationsByUserIdAsync(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(notifications, result.Data);
        }

        [Fact]
        public async Task GetNotificationsByUserIdAsync_InvalidUserId_ReturnsFailure()
        {
            // Arrange
            var userId = "invalid_user_id";

            var notificationRepositoryMock = new Mock<INotificationRepository>();
            notificationRepositoryMock.Setup(x =>
                x.GetFilteredItemsAsync(It.IsAny<Action<NotificationFilterBuilder>>()))
                    .ThrowsAsync(new Exception("Simulated error"));

            var loggerMock = new Mock<ILogger<NotificationService>>();
            var notificationService = new NotificationService(notificationRepositoryMock.Object,
                                                              It.IsAny<IUserRepository>(), // Mocked UserRepository
                                                              It.IsAny<IValidator<Notification>>(), // Mocked Validator
                                                              loggerMock.Object);

            // Act
            var result = await notificationService.GetNotificationsByUserIdAsync(userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal("Can not get notifications by user id.", result.Errors.Single());
        }

        [Fact]
        public async Task GetUnreadNotificationsByUserIdAsync_ValidUserId_ReturnsSuccess()
        {
            // Arrange
            var userId = "valid_user_id";
            var unreadNotifications = new List<Notification> { new Notification(), new Notification() };

            var notificationRepositoryMock = new Mock<INotificationRepository>();
            notificationRepositoryMock.Setup(x =>
                x.GetFilteredItemsAsync(It.IsAny<Action<NotificationFilterBuilder>>()))
                    .ReturnsAsync(unreadNotifications);

            var loggerMock = new Mock<ILogger<NotificationService>>();
            var notificationService = new NotificationService(notificationRepositoryMock.Object,
                                                              It.IsAny<IUserRepository>(), // Mocked UserRepository
                                                              It.IsAny<IValidator<Notification>>(), // Mocked Validator
                                                              loggerMock.Object);

            // Act
            var result = await notificationService.GetUnreadNotificationsByUserIdAsync(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(unreadNotifications, result.Data);
        }

        [Fact]
        public async Task GetUnreadNotificationsByUserIdAsync_InvalidUserId_ReturnsFailure()
        {
            // Arrange
            var userId = "invalid_user_id";

            var notificationRepositoryMock = new Mock<INotificationRepository>();
            notificationRepositoryMock.Setup(x =>
                x.GetFilteredItemsAsync(It.IsAny<Action<NotificationFilterBuilder>>()))
                    .ThrowsAsync(new Exception("Simulated error"));

            var loggerMock = new Mock<ILogger<NotificationService>>();
            var notificationService = new NotificationService(notificationRepositoryMock.Object,
                                                              It.IsAny<IUserRepository>(), // Mocked UserRepository
                                                              It.IsAny<IValidator<Notification>>(), // Mocked Validator
                                                              loggerMock.Object);

            // Act
            var result = await notificationService.GetUnreadNotificationsByUserIdAsync(userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal("Can not get unread notifications by user id.", result.Errors.Single());
        }

        [Fact]
        public async Task DeleteNotificationAsync_ValidNotificationId_ReturnsSuccess()
        {
            // Arrange
            var notificationId = "valid_notification_id";

            var notificationRepositoryMock = new Mock<INotificationRepository>();
            notificationRepositoryMock.Setup(x =>
                x.DeleteAsync(notificationId)).Returns(Task.CompletedTask);

            var loggerMock = new Mock<ILogger<NotificationService>>();
            var notificationService = new NotificationService(notificationRepositoryMock.Object,
                                                              It.IsAny<IUserRepository>(), // Mocked UserRepository
                                                              It.IsAny<IValidator<Notification>>(), // Mocked Validator
                                                              loggerMock.Object);

            // Act
            var result = await notificationService.DeleteNotificationAsync(notificationId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task DeleteNotificationAsync_InvalidNotificationId_ReturnsFailure()
        {
            // Arrange
            var notificationId = "invalid_notification_id";

            var notificationRepositoryMock = new Mock<INotificationRepository>();
            notificationRepositoryMock.Setup(x =>
                x.DeleteAsync(notificationId)).ThrowsAsync(new Exception("Simulated error"));

            var loggerMock = new Mock<ILogger<NotificationService>>();
            var notificationService = new NotificationService(notificationRepositoryMock.Object,
                                                              It.IsAny<IUserRepository>(), // Mocked UserRepository
                                                              It.IsAny<IValidator<Notification>>(), // Mocked Validator
                                                              loggerMock.Object);

            // Act
            var result = await notificationService.DeleteNotificationAsync(notificationId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Equal("Can not delete the notification.", result.Errors.Single());
        }

        [Fact]
        public async Task GetNotificationByIdAsync_ValidNotificationId_ReturnsSuccess()
        {
            // Arrange
            var notificationId = "valid_notification_id";
            var notification = new Notification { Id = notificationId };

            var notificationRepositoryMock = new Mock<INotificationRepository>();
            notificationRepositoryMock.Setup(x =>
                x.GetFilteredItemsAsync(It.IsAny<Action<NotificationFilterBuilder>>()))
                    .ReturnsAsync(new List<Notification> { notification });

            var loggerMock = new Mock<ILogger<NotificationService>>();
            var notificationService = new NotificationService(notificationRepositoryMock.Object,
                                                              It.IsAny<IUserRepository>(), // Mocked UserRepository
                                                              It.IsAny<IValidator<Notification>>(), // Mocked Validator
                                                              loggerMock.Object);

            // Act
            var result = await notificationService.GetNotificationByIdAsync(notificationId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(notification, result.Data);
        }

        [Fact]
        public async Task GetNotificationByIdAsync_InvalidNotificationId_ReturnsFailure()
        {
            // Arrange
            var notificationId = "invalid_notification_id";

            var notificationRepositoryMock = new Mock<INotificationRepository>();
            notificationRepositoryMock.Setup(x =>
                x.GetFilteredItemsAsync(It.IsAny<Action<NotificationFilterBuilder>>()))
                    .ReturnsAsync(new List<Notification>());

            var loggerMock = new Mock<ILogger<NotificationService>>();
            var notificationService = new NotificationService(notificationRepositoryMock.Object,
                                                              It.IsAny<IUserRepository>(), // Mocked UserRepository
                                                              It.IsAny<IValidator<Notification>>(), // Mocked Validator
                                                              loggerMock.Object);

            // Act
            var result = await notificationService.GetNotificationByIdAsync(notificationId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal("Notification with such id does not exist.", result.Errors.Single());
        }

        [Fact]
        public async Task GetNotificationByIdAsync_ExceptionThrown_ReturnsFailure()
        {
            // Arrange
            var notificationId = "valid_notification_id";

            var notificationRepositoryMock = new Mock<INotificationRepository>();
            notificationRepositoryMock.Setup(x =>
                x.GetFilteredItemsAsync(It.IsAny<Action<NotificationFilterBuilder>>()))
                    .ThrowsAsync(new Exception("Simulated error"));

            var loggerMock = new Mock<ILogger<NotificationService>>();
            var notificationService = new NotificationService(notificationRepositoryMock.Object,
                                                              It.IsAny<IUserRepository>(), // Mocked UserRepository
                                                              It.IsAny<IValidator<Notification>>(), // Mocked Validator
                                                              loggerMock.Object);

            // Act
            var result = await notificationService.GetNotificationByIdAsync(notificationId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal("Can not get the notification by id.", result.Errors.Single());
        }
    }
}
