using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Services;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;
using Taskify.DAL.Interfaces;

namespace Taskify.Tests.BAL.Tests.ServicesTests
{
    public class SubscriptionServiceTests
    {
        [Fact]
        public async Task CreateSubscriptionAsync_ValidSubscription_ReturnsSuccess()
        {
            // Arrange
            var subscription = new Subscription();
            var subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
            var validatorMock = new Mock<IValidator<Subscription>>();
            var loggerMock = new Mock<ILogger<SubscriptionService>>();
            var subscriptionService = new SubscriptionService(subscriptionRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            subscriptionRepositoryMock.Setup(x => x.AddAsync(subscription))
                                      .ReturnsAsync(subscription);

            validatorMock.Setup(x => x.ValidateAsync(subscription))
                         .ReturnsAsync((true, new List<string>()));

            // Act
            var result = await subscriptionService.CreateSubscriptionAsync(subscription);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(subscription, result.Data);
            subscriptionRepositoryMock.Verify(x => x.AddAsync(subscription), Times.Once);
        }

        [Fact]
        public async Task CreateSubscriptionAsync_InvalidSubscription_ReturnsFailure()
        {
            // Arrange
            var subscription = new Subscription();
            var subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
            var validatorMock = new Mock<IValidator<Subscription>>();
            var loggerMock = new Mock<ILogger<SubscriptionService>>();
            var subscriptionService = new SubscriptionService(subscriptionRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            subscriptionRepositoryMock.Setup(x => x.AddAsync(subscription))
                                      .ThrowsAsync(new Exception("Simulated error"));

            validatorMock.Setup(x => x.ValidateAsync(subscription))
                         .ReturnsAsync((false, new List<string> { "Validation error" }));

            // Act
            var result = await subscriptionService.CreateSubscriptionAsync(subscription);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Validation error", result.Errors);
        }

        [Fact]
        public async Task DeleteSubscriptionAsync_ValidId_ReturnsSuccess()
        {
            // Arrange
            var id = "valid_id";
            var subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
            var validatorMock = new Mock<IValidator<Subscription>>();
            var loggerMock = new Mock<ILogger<SubscriptionService>>();
            var subscriptionService = new SubscriptionService(subscriptionRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            subscriptionRepositoryMock.Setup(x => x.DeleteAsync(id))
                                      .Returns(Task.CompletedTask);

            // Act
            var result = await subscriptionService.DeleteSubscriptionAsync(id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            subscriptionRepositoryMock.Verify(x => x.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteSubscriptionAsync_InvalidId_ReturnsFailure()
        {
            // Arrange
            var id = "invalid_id";
            var subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
            var validatorMock = new Mock<IValidator<Subscription>>();
            var loggerMock = new Mock<ILogger<SubscriptionService>>();
            var subscriptionService = new SubscriptionService(subscriptionRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            subscriptionRepositoryMock.Setup(x => x.DeleteAsync(id))
                                      .ThrowsAsync(new Exception("Simulated error"));

            // Act
            var result = await subscriptionService.DeleteSubscriptionAsync(id);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Contains("Can not delete subscription.", result.Errors);
        }

        [Fact]
        public async Task GetAllSubscriptionsAsync_Success_ReturnsSuccess()
        {
            // Arrange
            var subscriptions = new List<Subscription> { new Subscription(), new Subscription() };
            var subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
            subscriptionRepositoryMock.Setup(x => x.GetAllAsync())
                                      .ReturnsAsync(subscriptions);
            var validatorMock = new Mock<IValidator<Subscription>>();
            var loggerMock = new Mock<ILogger<SubscriptionService>>();
            var subscriptionService = new SubscriptionService(subscriptionRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await subscriptionService.GetAllSubscriptionsAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(subscriptions, result.Data);
            subscriptionRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllSubscriptionsAsync_Failure_ReturnsFailure()
        {
            // Arrange
            var subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
            subscriptionRepositoryMock.Setup(x => x.GetAllAsync())
                                      .ThrowsAsync(new Exception("Simulated error"));
            var validatorMock = new Mock<IValidator<Subscription>>();
            var loggerMock = new Mock<ILogger<SubscriptionService>>();
            var subscriptionService = new SubscriptionService(subscriptionRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await subscriptionService.GetAllSubscriptionsAsync();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Can not get all subscriptions.", result.Errors);
        }

        [Fact]
        public async Task GetSubscriptionByIdAsync_ExistingId_ReturnsSuccess()
        {
            // Arrange
            var id = "existing_id";
            var subscription = new Subscription { Id = id };
            var subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
            subscriptionRepositoryMock.Setup(x => x.GetByIdAsync(id))
                                      .ReturnsAsync(subscription);
            var validatorMock = new Mock<IValidator<Subscription>>();
            var loggerMock = new Mock<ILogger<SubscriptionService>>();
            var subscriptionService = new SubscriptionService(subscriptionRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await subscriptionService.GetSubscriptionByIdAsync(id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(subscription, result.Data);
            subscriptionRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetSubscriptionByIdAsync_NonExistingId_ReturnsFailure()
        {
            // Arrange
            var id = "non_existing_id";
            var subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
            subscriptionRepositoryMock.Setup(x => x.GetByIdAsync(id))
                                      .ReturnsAsync((Subscription)null);
            var validatorMock = new Mock<IValidator<Subscription>>();
            var loggerMock = new Mock<ILogger<SubscriptionService>>();
            var subscriptionService = new SubscriptionService(subscriptionRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await subscriptionService.GetSubscriptionByIdAsync(id);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Subscription with such id does not exist.", result.Errors);
        }

        [Fact]
        public async Task GetSubscriptionByIdAsync_ExceptionThrown_ReturnsFailure()
        {
            // Arrange
            var id = "valid_id";
            var subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
            subscriptionRepositoryMock.Setup(x => x.GetByIdAsync(id))
                                      .ThrowsAsync(new Exception("Simulated error"));
            var validatorMock = new Mock<IValidator<Subscription>>();
            var loggerMock = new Mock<ILogger<SubscriptionService>>();
            var subscriptionService = new SubscriptionService(subscriptionRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await subscriptionService.GetSubscriptionByIdAsync(id);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Can not get subscription by id.", result.Errors);
        }

        [Fact]
        public async Task UpdateSubscriptionAsync_ValidSubscription_ReturnsSuccess()
        {
            // Arrange
            var subscription = new Subscription();
            var subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
            var validatorMock = new Mock<IValidator<Subscription>>();
            validatorMock.Setup(x => x.ValidateAsync(subscription))
                         .ReturnsAsync((true, new List<string>()));
            var loggerMock = new Mock<ILogger<SubscriptionService>>();
            var subscriptionService = new SubscriptionService(subscriptionRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await subscriptionService.UpdateSubscriptionAsync(subscription);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            subscriptionRepositoryMock.Verify(x => x.UpdateAsync(subscription), Times.Once);
        }

        [Fact]
        public async Task UpdateSubscriptionAsync_InvalidSubscription_ReturnsFailure()
        {
            // Arrange
            var subscription = new Subscription();
            var subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
            var validatorMock = new Mock<IValidator<Subscription>>();
            validatorMock.Setup(x => x.ValidateAsync(subscription))
                         .ReturnsAsync((false, new List<string> { "Validation error" }));
            var loggerMock = new Mock<ILogger<SubscriptionService>>();
            var subscriptionService = new SubscriptionService(subscriptionRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await subscriptionService.UpdateSubscriptionAsync(subscription);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Contains("Validation error", result.Errors);
            subscriptionRepositoryMock.Verify(x => x.UpdateAsync(subscription), Times.Never);
        }

        [Fact]
        public async Task UpdateSubscriptionAsync_ExceptionThrown_ReturnsFailure()
        {
            // Arrange
            var subscription = new Subscription();
            var subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
            var validatorMock = new Mock<IValidator<Subscription>>();
            validatorMock.Setup(x => x.ValidateAsync(subscription))
                         .ReturnsAsync((true, new List<string>()));
            subscriptionRepositoryMock.Setup(x => x.UpdateAsync(subscription))
                                      .ThrowsAsync(new Exception("Simulated error"));
            var loggerMock = new Mock<ILogger<SubscriptionService>>();
            var subscriptionService = new SubscriptionService(subscriptionRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await subscriptionService.UpdateSubscriptionAsync(subscription);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Contains("Can not update subscription.", result.Errors);
        }
    }
}
