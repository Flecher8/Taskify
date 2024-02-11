using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Services;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;
using Taskify.DAL.Interfaces;

namespace Taskify.Tests.BAL.Tests.ServicesTests
{
    public class UsersServiceTests
    {
        [Fact]
        public async Task GetAllUsersAsync_Success_ReturnsSuccess()
        {
            // Arrange
            var users = new List<User> { new User(), new User() };
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAllAsync())
                              .ReturnsAsync(users);
            var validatorMock = new Mock<IValidator<User>>();
            var loggerMock = new Mock<ILogger<UsersService>>();
            var userService = new UsersService(userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await userService.GetAllUsersAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(users, result.Data);
            userRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllUsersAsync_Failure_ReturnsFailure()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAllAsync())
                              .ThrowsAsync(new Exception("Simulated error"));
            var validatorMock = new Mock<IValidator<User>>();
            var loggerMock = new Mock<ILogger<UsersService>>();
            var userService = new UsersService(userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await userService.GetAllUsersAsync();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Can not get all users.", result.Errors);
        }

        [Fact]
        public async Task GetUserByIdAsync_ValidId_ReturnsSuccess()
        {
            // Arrange
            var userId = "valid_user_id";
            var user = new User { Id = userId, FirstName = "John", LastName = "Doe" };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
                              .ReturnsAsync(user);

            var validatorMock = new Mock<IValidator<User>>();
            var loggerMock = new Mock<ILogger<UsersService>>();
            var userService = new UsersService(userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await userService.GetUserByIdAsync(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(user, result.Data);
        }

        [Fact]
        public async Task GetUserByIdAsync_InvalidId_ReturnsFailure()
        {
            // Arrange
            var userId = "invalid_user_id";

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
                              .ReturnsAsync((User)null);

            var validatorMock = new Mock<IValidator<User>>();
            var loggerMock = new Mock<ILogger<UsersService>>();
            var userService = new UsersService(userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await userService.GetUserByIdAsync(userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("User with such id does not exist.", result.Errors);
        }

        [Fact]
        public async Task UpdateUserAsync_ValidUser_ReturnsSuccess()
        {
            // Arrange
            var userId = "valid_user_id";
            var userToUpdate = new User { Id = userId, FirstName = "John", LastName = "Doe" };
            var updatedUser = new User { Id = userId, FirstName = "UpdatedFirstName", LastName = "UpdatedLastName" };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
                              .ReturnsAsync(userToUpdate);
            userRepositoryMock.Setup(x => x.UpdateAsync(userToUpdate))
                              .Returns(Task.CompletedTask);

            var validatorMock = new Mock<IValidator<User>>();
            validatorMock.Setup(x => x.ValidateAsync(updatedUser))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<UsersService>>();
            var userService = new UsersService(userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await userService.UpdateUserAsync(updatedUser);

            // Assert
            Assert.True(result.IsSuccess);
            userRepositoryMock.Verify(x => x.UpdateAsync(userToUpdate), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_UserNotFound_ReturnsFailure()
        {
            // Arrange
            var userId = "invalid_user_id";
            var updatedUser = new User { Id = userId, FirstName = "John", LastName = "Doe" };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
                              .ReturnsAsync((User)null); // Simulating user not found

            var validatorMock = new Mock<IValidator<User>>();
            var loggerMock = new Mock<ILogger<UsersService>>();
            var userService = new UsersService(userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await userService.UpdateUserAsync(updatedUser);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("User with such id does not exist.", result.Errors);
            userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task DeleteUserAsync_ValidId_ReturnsSuccess()
        {
            // Arrange
            var userId = "valid_user_id";

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.DeleteAsync(userId))
                              .Returns(Task.CompletedTask);

            var loggerMock = new Mock<ILogger<UsersService>>();
            var userService = new UsersService(userRepositoryMock.Object, It.IsAny<IValidator<User>>(), loggerMock.Object);

            // Act
            var result = await userService.DeleteUserAsync(userId);

            // Assert
            Assert.True(result.IsSuccess);
            userRepositoryMock.Verify(x => x.DeleteAsync(userId), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_UserNotFound_ReturnsFailure()
        {
            // Arrange
            var userId = "invalid_user_id";

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.DeleteAsync(userId))
                              .ThrowsAsync(new Exception("User not found")); // Simulating user not found

            var loggerMock = new Mock<ILogger<UsersService>>();
            var userService = new UsersService(userRepositoryMock.Object, It.IsAny<IValidator<User>>(), loggerMock.Object);

            // Act
            var result = await userService.DeleteUserAsync(userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Can not delete user.", result.Errors);
            userRepositoryMock.Verify(x => x.DeleteAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ValidEmail_ReturnsSuccess()
        {
            // Arrange
            var email = "test@example.com";
            var user = new User { Id = "valid_user_id", FirstName = "John", LastName = "Doe", Email = email };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Expression<Func<User, bool>>>()))
                  .ReturnsAsync(new List<User> { user });

            var validatorMock = new Mock<IValidator<User>>();
            var loggerMock = new Mock<ILogger<UsersService>>();
            var userService = new UsersService(userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await userService.GetUserByEmailAsync(email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(user, result.Data);
        }

        [Fact]
        public async Task GetUserByEmailAsync_InvalidEmail_ReturnsFailure()
        {
            // Arrange
            var email = "invalid_email";

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Expression<Func<User, bool>>>()))
                  .ReturnsAsync(new List<User> {});

            var validatorMock = new Mock<IValidator<User>>();
            var loggerMock = new Mock<ILogger<UsersService>>();
            var userService = new UsersService(userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await userService.GetUserByEmailAsync(email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("User with such email does not exist.", result.Errors);
        }

        [Fact]
        public async Task GetUserByEmailAsync_NullOrEmptyEmail_ReturnsFailure()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var validatorMock = new Mock<IValidator<User>>();
            var loggerMock = new Mock<ILogger<UsersService>>();
            var userService = new UsersService(userRepositoryMock.Object, validatorMock.Object, loggerMock.Object);

            // Act
            var result = await userService.GetUserByEmailAsync(null);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Email can not be empty.", result.Errors);
        }
    }
}
