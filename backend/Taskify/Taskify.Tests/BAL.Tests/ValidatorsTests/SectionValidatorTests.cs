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
    public class SectionValidatorTests
    {
        [Fact]
        public async Task ValidateAsync_ValidSection_ReturnsValidResult()
        {
            // Arrange
            var section = new Section
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Section 1",
                SequenceNumber = 1,
                SectionType = SectionType.ToDo
            };

            var validator = new SectionValidator();

            // Act
            var result = await validator.ValidateAsync(section);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_SectionNameNullOrEmpty_ReturnsErrorMessage()
        {
            // Arrange
            var section = new Section
            {
                Id = Guid.NewGuid().ToString(),
                Name = null, // Invalid value
                SequenceNumber = 1,
                SectionType = SectionType.ToDo
            };

            var validator = new SectionValidator();

            // Act
            var result = await validator.ValidateAsync(section);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Section name cannot be null or empty.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_SectionSequenceNumberNegative_ReturnsErrorMessage()
        {
            // Arrange
            var section = new Section
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Section 1",
                SequenceNumber = -1, // Invalid value
                SectionType = SectionType.ToDo
            };

            var validator = new SectionValidator();

            // Act
            var result = await validator.ValidateAsync(section);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Section sequence number can not be less than 0.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_InvalidSectionType_ReturnsErrorMessage()
        {
            // Arrange
            var section = new Section
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Section 1",
                SequenceNumber = 1,
                SectionType = (SectionType)100 // Invalid value
            };

            var validator = new SectionValidator();

            // Act
            var result = await validator.ValidateAsync(section);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Invalid section type.", result.ErrorMessages);
        }

        [Fact]
        public async Task ValidateAsync_AllErrors_ReturnsAllErrorMessages()
        {
            // Arrange
            var section = new Section
            {
                Id = "invalid-id",
                Name = null, // Invalid value
                SequenceNumber = -1, // Invalid value
                SectionType = (SectionType)100 // Invalid value
            };

            var validator = new SectionValidator();

            // Act
            var result = await validator.ValidateAsync(section);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Invalid project Id format.", result.ErrorMessages);
            Assert.Contains("Section name cannot be null or empty.", result.ErrorMessages);
            Assert.Contains("Section sequence number can not be less than 0.", result.ErrorMessages);
            Assert.Contains("Invalid section type.", result.ErrorMessages);
        }
    }
}
