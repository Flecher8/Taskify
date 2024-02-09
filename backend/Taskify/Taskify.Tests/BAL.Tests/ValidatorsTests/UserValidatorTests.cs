using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;

namespace Taskify.Tests.BAL.Tests.ValidatorsTests
{
    public class UserValidatorTests
    {
        [Fact]
        public async Task ValidateAsync_ValidUser_ReturnsValidResult()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "John",
                LastName = "Doe",
                CreatedAt = DateTime.UtcNow
            };

            var validator = new UserValidator();

            // Act
            var result = await validator.ValidateAsync(user);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_WrongFirstName_ReturnsErrorMessage()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = null, // Invalid value
                LastName = "Doe",
                CreatedAt = DateTime.UtcNow
            };

            var validator = new UserValidator();

            // Act
            var result = await validator.ValidateAsync(user);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("First name cannot be null or empty.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_WrongLastName_ReturnsErrorMessage()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "John",
                LastName = null, // Invalid value
                CreatedAt = DateTime.UtcNow
            };

            var validator = new UserValidator();

            // Act
            var result = await validator.ValidateAsync(user);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Last name cannot be null or empty.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_WrongId_ReturnsErrorMessage()
        {
            // Arrange
            var user = new User
            {
                Id = "invalid-guid", // Invalid value
                FirstName = "John",
                LastName = "Doe",
                CreatedAt = DateTime.UtcNow
            };

            var validator = new UserValidator();

            // Act
            var result = await validator.ValidateAsync(user);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Id must be a valid Guid string.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_WrongDateTime_ReturnsErrorMessage()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "John",
                LastName = "Doe",
                CreatedAt = DateTime.MinValue // Invalid creation date
            };

            var validator = new UserValidator();

            // Act
            var result = await validator.ValidateAsync(user);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Creation date and time are not correct.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_AllDataWrong_ReturnsListOfAllErrors()
        {
            // Arrange
            var user = new User
            {
                Id = "invalid-guid", // Invalid value
                FirstName = null, // Invalid value
                LastName = "", // Invalid value
                CreatedAt = DateTime.MinValue // Invalid creation date
            };

            var validator = new UserValidator();

            // Act
            var result = await validator.ValidateAsync(user);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Id must be a valid Guid string.", result.ErrorMessages);
            Assert.Contains("First name cannot be null or empty.", result.ErrorMessages);
            Assert.Contains("Last name cannot be null or empty.", result.ErrorMessages);
            Assert.Contains("Creation date and time are not correct.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_NullUser_ReturnsErrorMessages()
        {
            // Arrange
            User user = null;

            var validator = new UserValidator();

            // Act
            var result = await validator.ValidateAsync(user);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Entity cannot be null.", result.ErrorMessages);
        }
    }
}
