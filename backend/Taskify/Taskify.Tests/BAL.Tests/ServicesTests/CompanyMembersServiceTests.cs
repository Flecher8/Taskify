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
    public class CompanyMembersServiceTests
    {
        [Fact]
        public async Task CreateCompanyMemberAsync_InvalidUserId_ReturnsFailure()
        {
            // Arrange
            var companyMember = new CompanyMember
            {
                Id = "1",
                User = new User { Id = "invalid_id" },
                Company = new Company { Id = "1" }
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetByIdAsync(companyMember.User.Id))
                              .ReturnsAsync((User)null);

            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CompanyFilterBuilder>>()))
                                .ReturnsAsync(new List<Company>() { new Company { Id = "1" } });

            var companyMemberRepositoryMock = new Mock<ICompanyMemberRepository>();
            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            var validatorMock = new Mock<IValidator<CompanyMember>>();
            validatorMock.Setup(x => x.ValidateAsync(companyMember))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<CompanyMembersService>>();

            var service = new CompanyMembersService(
                companyMemberRepositoryMock.Object,
                userRepositoryMock.Object,
                companyRepositoryMock.Object,
                companyMemberRoleRepositoryMock.Object,
                validatorMock.Object,
                loggerMock.Object
            );

            // Act
            var result = await service.CreateCompanyMemberAsync(companyMember);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Invalid user specified.", result.Errors);
        }

        [Fact]
        public async Task DeleteCompanyMemberAsync_ValidId_ReturnsSuccess()
        {
            // Arrange
            var memberId = "1";

            var companyMemberRepositoryMock = new Mock<ICompanyMemberRepository>();
            companyMemberRepositoryMock.Setup(x => x.DeleteAsync(memberId))
                                       .Returns(Task.CompletedTask);

            var userRepositoryMock = new Mock<IUserRepository>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            var validatorMock = new Mock<IValidator<CompanyMember>>();
            var loggerMock = new Mock<ILogger<CompanyMembersService>>();

            var service = new CompanyMembersService(
                companyMemberRepositoryMock.Object,
                userRepositoryMock.Object,
                companyRepositoryMock.Object,
                companyMemberRoleRepositoryMock.Object,
                validatorMock.Object,
                loggerMock.Object
            );

            // Act
            var result = await service.DeleteCompanyMemberAsync(memberId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task DeleteCompanyMemberAsync_InvalidId_ReturnsFailure()
        {
            // Arrange
            var memberId = "invalid_id";

            var companyMemberRepositoryMock = new Mock<ICompanyMemberRepository>();
            companyMemberRepositoryMock.Setup(x => x.DeleteAsync(memberId))
                                       .ThrowsAsync(new Exception("Simulated error"));

            var userRepositoryMock = new Mock<IUserRepository>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            var validatorMock = new Mock<IValidator<CompanyMember>>();
            var loggerMock = new Mock<ILogger<CompanyMembersService>>();

            var service = new CompanyMembersService(
                companyMemberRepositoryMock.Object,
                userRepositoryMock.Object,
                companyRepositoryMock.Object,
                companyMemberRoleRepositoryMock.Object,
                validatorMock.Object,
                loggerMock.Object
            );

            // Act
            var result = await service.DeleteCompanyMemberAsync(memberId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Contains("Can not delete the company member.", result.Errors);
        }

        [Fact]
        public async Task UpdateCompanyMemberAsync_ValidInput_ReturnsSuccess()
        {
            // Arrange
            var companyMember = new CompanyMember
            {
                Id = "1",
                Salary = 5000
            };

            var memberToUpdate = new CompanyMember
            {
                Id = "1",
                Salary = 3000
            };

            var companyMemberRepositoryMock = new Mock<ICompanyMemberRepository>();
            companyMemberRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CompanyMemberFilterBuilder>>()))
                                       .ReturnsAsync(new List<CompanyMember> { memberToUpdate });

            companyMemberRepositoryMock.Setup(x => x.UpdateAsync(memberToUpdate))
                                       .Returns(Task.CompletedTask);

            var userRepositoryMock = new Mock<IUserRepository>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            var validatorMock = new Mock<IValidator<CompanyMember>>();
            validatorMock.Setup(x => x.ValidateAsync(companyMember))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<CompanyMembersService>>();

            var service = new CompanyMembersService(
                companyMemberRepositoryMock.Object,
                userRepositoryMock.Object,
                companyRepositoryMock.Object,
                companyMemberRoleRepositoryMock.Object,
                validatorMock.Object,
                loggerMock.Object
            );

            // Act
            var result = await service.UpdateCompanyMemberAsync(companyMember);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task UpdateCompanyMemberAsync_InvalidId_ReturnsFailure()
        {
            // Arrange
            var companyMember = new CompanyMember
            {
                Id = "invalid_id",
                Salary = 5000
            };

            var companyMemberRepositoryMock = new Mock<ICompanyMemberRepository>();
            companyMemberRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CompanyMemberFilterBuilder>>()))
                                       .ReturnsAsync(new List<CompanyMember>());

            var userRepositoryMock = new Mock<IUserRepository>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            var validatorMock = new Mock<IValidator<CompanyMember>>();
            validatorMock.Setup(x => x.ValidateAsync(companyMember))
                        .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<CompanyMembersService>>();

            var service = new CompanyMembersService(
                companyMemberRepositoryMock.Object,
                userRepositoryMock.Object,
                companyRepositoryMock.Object,
                companyMemberRoleRepositoryMock.Object,
                validatorMock.Object,
                loggerMock.Object
            );

            // Act
            var result = await service.UpdateCompanyMemberAsync(companyMember);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Contains("Can not find company member with such id.", result.Errors);
        }

        [Fact]
        public async Task GetMembersByCompanyIdAsync_ValidCompanyId_ReturnsSuccess()
        {
            // Arrange
            var companyId = "valid_company_id";
            var expectedMembers = new List<CompanyMember>
    {
        new CompanyMember(),
        new CompanyMember()
    };

            var companyMemberRepositoryMock = new Mock<ICompanyMemberRepository>();
            companyMemberRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CompanyMemberFilterBuilder>>()))
                                       .ReturnsAsync(expectedMembers);

            var userRepositoryMock = new Mock<IUserRepository>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            var validatorMock = new Mock<IValidator<CompanyMember>>();
            var loggerMock = new Mock<ILogger<CompanyMembersService>>();

            var service = new CompanyMembersService(
                companyMemberRepositoryMock.Object,
                userRepositoryMock.Object,
                companyRepositoryMock.Object,
                companyMemberRoleRepositoryMock.Object,
                validatorMock.Object,
                loggerMock.Object
            );

            // Act
            var result = await service.GetMembersByCompanyIdAsync(companyId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedMembers, result.Data);
        }

        [Fact]
        public async Task GetMembersByRoleIdAsync_ValidRoleId_ReturnsSuccess()
        {
            // Arrange
            var roleId = "valid_role_id";
            var expectedMembers = new List<CompanyMember>
    {
        new CompanyMember(),
        new CompanyMember()
    };

            var companyMemberRepositoryMock = new Mock<ICompanyMemberRepository>();
            companyMemberRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CompanyMemberFilterBuilder>>()))
                                       .ReturnsAsync(expectedMembers);

            var userRepositoryMock = new Mock<IUserRepository>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            var validatorMock = new Mock<IValidator<CompanyMember>>();
            var loggerMock = new Mock<ILogger<CompanyMembersService>>();

            var service = new CompanyMembersService(
                companyMemberRepositoryMock.Object,
                userRepositoryMock.Object,
                companyRepositoryMock.Object,
                companyMemberRoleRepositoryMock.Object,
                validatorMock.Object,
                loggerMock.Object
            );

            // Act
            var result = await service.GetMembersByRoleIdAsync(roleId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedMembers, result.Data);
        }

        [Fact]
        public async Task GetMembersByRoleIdAsync_EmptyRoleId_ReturnsSuccess()
        {
            // Arrange
            var expectedMembers = new List<CompanyMember>
    {
        new CompanyMember(),
        new CompanyMember()
    };

            var companyMemberRepositoryMock = new Mock<ICompanyMemberRepository>();
            companyMemberRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CompanyMemberFilterBuilder>>()))
                                       .ReturnsAsync(expectedMembers);

            var userRepositoryMock = new Mock<IUserRepository>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            var validatorMock = new Mock<IValidator<CompanyMember>>();
            var loggerMock = new Mock<ILogger<CompanyMembersService>>();

            var service = new CompanyMembersService(
                companyMemberRepositoryMock.Object,
                userRepositoryMock.Object,
                companyRepositoryMock.Object,
                companyMemberRoleRepositoryMock.Object,
                validatorMock.Object,
                loggerMock.Object
            );

            // Act
            var result = await service.GetMembersByRoleIdAsync("");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedMembers, result.Data);
        }

        [Fact]
        public async Task GetMembersByRoleIdAsync_InvalidRoleId_ReturnsFailure()
        {
            // Arrange
            var roleId = "invalid_role_id";

            var companyMemberRepositoryMock = new Mock<ICompanyMemberRepository>();
            companyMemberRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CompanyMemberFilterBuilder>>()))
                                       .ThrowsAsync(new Exception("Simulated error"));

            var userRepositoryMock = new Mock<IUserRepository>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            var validatorMock = new Mock<IValidator<CompanyMember>>();
            var loggerMock = new Mock<ILogger<CompanyMembersService>>();

            var service = new CompanyMembersService(
                companyMemberRepositoryMock.Object,
                userRepositoryMock.Object,
                companyRepositoryMock.Object,
                companyMemberRoleRepositoryMock.Object,
                validatorMock.Object,
                loggerMock.Object
            );

            // Act
            var result = await service.GetMembersByRoleIdAsync(roleId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Can not get company members by role id.", result.Errors);
        }

        [Fact]
        public async Task GetCompanyMemberByIdAsync_ValidId_ReturnsSuccess()
        {
            // Arrange
            var memberId = "valid_member_id";
            var expectedMember = new CompanyMember { Id = memberId };

            var companyMemberRepositoryMock = new Mock<ICompanyMemberRepository>();
            companyMemberRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CompanyMemberFilterBuilder>>()))
                                       .ReturnsAsync(new List<CompanyMember> { expectedMember });

            var userRepositoryMock = new Mock<IUserRepository>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            var validatorMock = new Mock<IValidator<CompanyMember>>();
            var loggerMock = new Mock<ILogger<CompanyMembersService>>();

            var service = new CompanyMembersService(
                companyMemberRepositoryMock.Object,
                userRepositoryMock.Object,
                companyRepositoryMock.Object,
                companyMemberRoleRepositoryMock.Object,
                validatorMock.Object,
                loggerMock.Object
            );

            // Act
            var result = await service.GetCompanyMemberByIdAsync(memberId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedMember, result.Data);
        }

        [Fact]
        public async Task GetCompanyMemberByIdAsync_InvalidId_ReturnsFailure()
        {
            // Arrange
            var memberId = "invalid_member_id";

            var companyMemberRepositoryMock = new Mock<ICompanyMemberRepository>();
            companyMemberRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CompanyMemberFilterBuilder>>()))
                                       .ReturnsAsync(new List<CompanyMember>());

            var userRepositoryMock = new Mock<IUserRepository>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            var validatorMock = new Mock<IValidator<CompanyMember>>();
            var loggerMock = new Mock<ILogger<CompanyMembersService>>();

            var service = new CompanyMembersService(
                companyMemberRepositoryMock.Object,
                userRepositoryMock.Object,
                companyRepositoryMock.Object,
                companyMemberRoleRepositoryMock.Object,
                validatorMock.Object,
                loggerMock.Object
            );

            // Act
            var result = await service.GetCompanyMemberByIdAsync(memberId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Company member with such id does not exist.", result.Errors);
        }

        [Fact]
        public async Task IsUserAlreadyMemberAsync_UserIsAlreadyMember_ReturnsTrue()
        {
            // Arrange
            var userId = "valid_user_id";
            var companyId = "valid_company_id";

            var existingMembers = new List<CompanyMember>
    {
        new CompanyMember { User = new User { Id = userId }, Company = new Company { Id = companyId } }
    };

            var companyMemberRepositoryMock = new Mock<ICompanyMemberRepository>();
            companyMemberRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CompanyMemberFilterBuilder>>()))
                                       .ReturnsAsync(existingMembers);

            var userRepositoryMock = new Mock<IUserRepository>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            var validatorMock = new Mock<IValidator<CompanyMember>>();
            var loggerMock = new Mock<ILogger<CompanyMembersService>>();

            var service = new CompanyMembersService(
                companyMemberRepositoryMock.Object,
                userRepositoryMock.Object,
                companyRepositoryMock.Object,
                companyMemberRoleRepositoryMock.Object,
                validatorMock.Object,
                loggerMock.Object
            );

            // Act
            var result = await service.IsUserAlreadyMemberAsync(userId, companyId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task IsUserAlreadyMemberAsync_UserIsNotMember_ReturnsFalse()
        {
            // Arrange
            var userId = "valid_user_id";
            var companyId = "valid_company_id";

            var companyMemberRepositoryMock = new Mock<ICompanyMemberRepository>();
            companyMemberRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CompanyMemberFilterBuilder>>()))
                                       .ReturnsAsync(new List<CompanyMember>());

            var userRepositoryMock = new Mock<IUserRepository>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            var validatorMock = new Mock<IValidator<CompanyMember>>();
            var loggerMock = new Mock<ILogger<CompanyMembersService>>();

            var service = new CompanyMembersService(
                companyMemberRepositoryMock.Object,
                userRepositoryMock.Object,
                companyRepositoryMock.Object,
                companyMemberRoleRepositoryMock.Object,
                validatorMock.Object,
                loggerMock.Object
            );

            // Act
            var result = await service.IsUserAlreadyMemberAsync(userId, companyId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.Data);
        }

        [Fact]
        public async Task IsUserAlreadyMemberAsync_ExceptionThrown_ReturnsFailure()
        {
            // Arrange
            var userId = "valid_user_id";
            var companyId = "valid_company_id";

            var companyMemberRepositoryMock = new Mock<ICompanyMemberRepository>();
            companyMemberRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CompanyMemberFilterBuilder>>()))
                                       .ThrowsAsync(new Exception("Simulated error"));

            var userRepositoryMock = new Mock<IUserRepository>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            var validatorMock = new Mock<IValidator<CompanyMember>>();
            var loggerMock = new Mock<ILogger<CompanyMembersService>>();

            var service = new CompanyMembersService(
                companyMemberRepositoryMock.Object,
                userRepositoryMock.Object,
                companyRepositoryMock.Object,
                companyMemberRoleRepositoryMock.Object,
                validatorMock.Object,
                loggerMock.Object
            );

            // Act
            var result = await service.IsUserAlreadyMemberAsync(userId, companyId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Contains("Error checking if user is already a member.", result.Errors);
        }


    }
}
