using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Services;
using Taskify.Core.DbModels;
using Taskify.DAL.Helpers;
using Taskify.DAL.Interfaces;

namespace Taskify.Tests.BAL.Tests.ServicesTests
{
    public class UserSubscriptionServiceTests
    {
        [Fact]
        public async Task GetUserSubscription_ValidUserId_ReturnsSuccess()
        {
            // Arrange
            var userId = "valid_user_id";
            var subscription = new Subscription { Id = "subscription_id", DurationInDays = 30 };
            var userSubscription = new UserSubscription { Subscription = subscription, EndDateTimeUtc = DateTime.UtcNow.AddDays(15) };

            var userSubscriptionRepositoryMock = new Mock<IUserSubscriptionRepository>();
            userSubscriptionRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<UserSubscriptionFilterBuilder>>()))
                                           .ReturnsAsync(new List<UserSubscription> { userSubscription });

            var subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
            subscriptionRepositoryMock.Setup(x => x.GetByIdAsync(subscription.Id))
                                       .ReturnsAsync(subscription);

            var userRepositoryMock = new Mock<IUserRepository>();
            var loggerMock = new Mock<ILogger<UserSubscriptionService>>();

            var userSubscriptionService = new UserSubscriptionService(userSubscriptionRepositoryMock.Object,
                                                                     subscriptionRepositoryMock.Object,
                                                                     userRepositoryMock.Object,
                                                                     loggerMock.Object);

            // Act
            var result = await userSubscriptionService.GetUserSubscription(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(subscription, result.Data);
        }

        [Fact]
        public async Task GetUserSubscription_ExceptionThrown_ReturnsFailure()
        {
            // Arrange
            var userId = "valid_user_id";

            var userSubscriptionRepositoryMock = new Mock<IUserSubscriptionRepository>();
            userSubscriptionRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<UserSubscriptionFilterBuilder>>()))
                                           .ThrowsAsync(new Exception("Simulated error"));

            var userRepositoryMock = new Mock<IUserRepository>();
            var subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
            var loggerMock = new Mock<ILogger<UserSubscriptionService>>();

            var userSubscriptionService = new UserSubscriptionService(userSubscriptionRepositoryMock.Object,
                                                                     subscriptionRepositoryMock.Object,
                                                                     userRepositoryMock.Object,
                                                                     loggerMock.Object);

            // Act
            var result = await userSubscriptionService.GetUserSubscription(userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Single(result.Errors);
        }

        [Fact]
        public async Task CreateUserSubscription_InvalidUser_ReturnsFailure()
        {
            // Arrange
            var userId = "invalid_user_id";
            var subscriptionId = "valid_subscription_id";

            var userSubscriptionRepositoryMock = new Mock<IUserSubscriptionRepository>();
            var subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
            subscriptionRepositoryMock.Setup(x => x.GetByIdAsync(subscriptionId))
                                       .ReturnsAsync(new Subscription());

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
                              .ReturnsAsync((User)null);

            var loggerMock = new Mock<ILogger<UserSubscriptionService>>();

            var userSubscriptionService = new UserSubscriptionService(userSubscriptionRepositoryMock.Object,
                                                                     subscriptionRepositoryMock.Object,
                                                                     userRepositoryMock.Object,
                                                                     loggerMock.Object);

            // Act
            var result = await userSubscriptionService.CreateUserSubscription(userId, subscriptionId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("User not found.", result.Errors);
            userSubscriptionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<UserSubscription>()), Times.Never);
        }

        [Fact]
        public async Task CreateUserSubscription_InvalidSubscription_ReturnsFailure()
        {
            // Arrange
            var userId = "valid_user_id";
            var subscriptionId = "invalid_subscription_id";

            var userSubscriptionRepositoryMock = new Mock<IUserSubscriptionRepository>();
            var subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
            subscriptionRepositoryMock.Setup(x => x.GetByIdAsync(subscriptionId))
                                       .ReturnsAsync((Subscription)null);

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
                              .ReturnsAsync(new User());

            var loggerMock = new Mock<ILogger<UserSubscriptionService>>();

            var userSubscriptionService = new UserSubscriptionService(userSubscriptionRepositoryMock.Object,
                                                                     subscriptionRepositoryMock.Object,
                                                                     userRepositoryMock.Object,
                                                                     loggerMock.Object);

            // Act
            var result = await userSubscriptionService.CreateUserSubscription(userId, subscriptionId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Subscription not found.", result.Errors);
            userSubscriptionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<UserSubscription>()), Times.Never);
        }

        [Fact]
        public async Task CreateUserSubscription_ExceptionThrown_ReturnsFailure()
        {
            // Arrange
            var userId = "valid_user_id";
            var subscriptionId = "valid_subscription_id";

            var userSubscriptionRepositoryMock = new Mock<IUserSubscriptionRepository>();
            var subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
            subscriptionRepositoryMock.Setup(x => x.GetByIdAsync(subscriptionId))
                                       .ThrowsAsync(new Exception("Simulated error"));

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
                              .ReturnsAsync(new User());

            var loggerMock = new Mock<ILogger<UserSubscriptionService>>();

            var userSubscriptionService = new UserSubscriptionService(userSubscriptionRepositoryMock.Object,
                                                                     subscriptionRepositoryMock.Object,
                                                                     userRepositoryMock.Object,
                                                                     loggerMock.Object);

            // Act
            var result = await userSubscriptionService.CreateUserSubscription(userId, subscriptionId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Single(result.Errors);
        }
    }
}
