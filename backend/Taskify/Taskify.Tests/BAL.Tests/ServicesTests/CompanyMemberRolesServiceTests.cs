using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Interfaces;
using Taskify.BLL.Services;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;
using Taskify.DAL.Helpers;
using Taskify.DAL.Interfaces;

namespace Taskify.Tests.BAL.Tests.ServicesTests
{
    public class CompanyMemberRolesServiceTests
    {
        [Fact]
        public async Task CreateCompanyMemberRoleAsync_ValidCompanyMemberRole_ReturnsSuccess()
        {
            // Arrange
            var companyMemberRole = new CompanyMemberRole
            {
                Id = "valid_role_id",
                Name = "Test Role",
                Company = new Company { Id = "valid_company_id" }
            };

            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(x => x.GetByIdAsync(companyMemberRole.Company.Id))
                                 .ReturnsAsync(new Company { Id = "valid_company_id" });

            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            companyMemberRoleRepositoryMock.Setup(x => x.AddAsync(companyMemberRole))
                                          .ReturnsAsync(companyMemberRole);

            var validatorMock = new Mock<IValidator<CompanyMemberRole>>();
            validatorMock.Setup(x => x.ValidateAsync(companyMemberRole))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<CompanyMemberRolesService>>();

            var companyMemberRolesService = new CompanyMemberRolesService(
                companyMemberRoleRepositoryMock.Object,
                It.IsAny<ICompanyMemberRepository>(), // Mocked CompanyMemberRepository
                companyRepositoryMock.Object,
                validatorMock.Object,
                loggerMock.Object
            );

            // Act
            var result = await companyMemberRolesService.CreateCompanyMemberRoleAsync(companyMemberRole);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(companyMemberRole, result.Data);
            companyMemberRoleRepositoryMock.Verify(x => x.AddAsync(companyMemberRole), Times.Once);
        }

        [Fact]
        public async Task CreateCompanyAsync_InvalidCompany_ReturnsFailure()
        {
            // Arrange
            var invalidCompany = new CompanyMemberRole(); // No properties set

            var companyRepositoryMock = new Mock<ICompanyRepository>();
            var validatorMock = new Mock<IValidator<CompanyMemberRole>>();
            validatorMock.Setup(x => x.ValidateAsync(invalidCompany))
                         .ReturnsAsync((false, new List<string> { "Validation error" }));

            var loggerMock = new Mock<ILogger<CompanyMemberRolesService>>();

            var companyMemberRolesService = new CompanyMemberRolesService(
                It.IsAny<ICompanyMemberRoleRepository>(), 
                It.IsAny<ICompanyMemberRepository>(), 
                companyRepositoryMock.Object,
                validatorMock.Object,
                loggerMock.Object
            );

            // Act
            var result = await companyMemberRolesService.CreateCompanyMemberRoleAsync(invalidCompany);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Validation error", result.Errors);
            companyRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Company>()), Times.Never);
        }

        [Fact]
        public async Task CreateCompanyMemberRoleAsync_CompanyRepositoryThrowsException_ReturnsFailure()
        {
            // Arrange
            var validCompanyMemberRole = new CompanyMemberRole {Company = new Company { Id = "1" }, Name = "Test Role" };

            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(x => x.GetByIdAsync(validCompanyMemberRole.Company.Id))
                                .ReturnsAsync(new Company { Id = "1" });

            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();

            companyMemberRoleRepositoryMock.Setup(x => x.AddAsync(validCompanyMemberRole))
                                           .ThrowsAsync(new Exception("Simulated error"));

            var validatorMock = new Mock<IValidator<CompanyMemberRole>>();
            validatorMock.Setup(x => x.ValidateAsync(validCompanyMemberRole))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<CompanyMemberRolesService>>();

            var companyMemberRolesService = new CompanyMemberRolesService(
                companyMemberRoleRepositoryMock.Object,
                It.IsAny<ICompanyMemberRepository>(),
                companyRepositoryMock.Object,
                validatorMock.Object,
                loggerMock.Object
            );

            // Act
            var result = await companyMemberRolesService.CreateCompanyMemberRoleAsync(validCompanyMemberRole);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Can not create a new company member role.", result.Errors);
        }

        [Fact]
        public async Task DeleteCompanyMemberRoleAsync_CompanyMemberRoleNotFound_ReturnsFailure()
        {
            // Arrange
            var companyMemberRoleId = "invalid_id";

            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            companyMemberRoleRepositoryMock.Setup(x => x.GetByIdAsync(companyMemberRoleId))
                                           .ReturnsAsync((CompanyMemberRole)null);

            var companyMemberRepositoryMock = new Mock<ICompanyMemberRepository>();

            var loggerMock = new Mock<ILogger<CompanyMemberRolesService>>();

            var companyMemberRolesService = new CompanyMemberRolesService(
                companyMemberRoleRepositoryMock.Object,
                companyMemberRepositoryMock.Object,
                It.IsAny<ICompanyRepository>(),
                It.IsAny<IValidator<CompanyMemberRole>>(),
                loggerMock.Object
            );

            // Act
            var result = await companyMemberRolesService.DeleteCompanyMemberRoleAsync(companyMemberRoleId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Company member role with such id does not exist.", result.Errors);
        }

        [Fact]
        public async Task DeleteCompanyMemberRoleAsync_ExceptionThrown_ReturnsFailure()
        {
            // Arrange
            var companyMemberRoleId = "valid_id";

            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            companyMemberRoleRepositoryMock.Setup(x => x.GetByIdAsync(companyMemberRoleId))
                                           .ReturnsAsync(new CompanyMemberRole());

            var companyMemberRepositoryMock = new Mock<ICompanyMemberRepository>();
            companyMemberRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CompanyMemberFilterBuilder>>()))
                                       .ThrowsAsync(new Exception("Simulated error"));

            var loggerMock = new Mock<ILogger<CompanyMemberRolesService>>();

            var companyMemberRolesService = new CompanyMemberRolesService(
                companyMemberRoleRepositoryMock.Object,
                companyMemberRepositoryMock.Object,
                It.IsAny<ICompanyRepository>(),
                It.IsAny<IValidator<CompanyMemberRole>>(),
                loggerMock.Object
            );

            // Act
            var result = await companyMemberRolesService.DeleteCompanyMemberRoleAsync(companyMemberRoleId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Can not delete the company member role.", result.Errors);
        }

        [Fact]
        public async Task GetCompanyMemberRoleByIdAsync_CompanyMemberRoleNotFound_ReturnsFailure()
        {
            // Arrange
            var companyMemberRoleId = "invalid_id";

            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            companyMemberRoleRepositoryMock.Setup(x => x.GetByIdAsync(companyMemberRoleId))
                                           .ReturnsAsync((CompanyMemberRole)null);

            var loggerMock = new Mock<ILogger<CompanyMemberRolesService>>();

            var companyMemberRolesService = new CompanyMemberRolesService(
                companyMemberRoleRepositoryMock.Object,
                It.IsAny<ICompanyMemberRepository>(),
                It.IsAny<ICompanyRepository>(),
                It.IsAny<IValidator<CompanyMemberRole>>(),
                loggerMock.Object
            );

            // Act
            var result = await companyMemberRolesService.GetCompanyMemberRoleByIdAsync(companyMemberRoleId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Company member role with such id does not exist.", result.Errors);
        }

        [Fact]
        public async Task GetCompanyMemberRoleByIdAsync_ExceptionThrown_ReturnsFailure()
        {
            // Arrange
            var companyMemberRoleId = "valid_id";

            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            companyMemberRoleRepositoryMock.Setup(x => x.GetByIdAsync(companyMemberRoleId))
                                           .ThrowsAsync(new Exception("Simulated error"));

            var loggerMock = new Mock<ILogger<CompanyMemberRolesService>>();

            var companyMemberRolesService = new CompanyMemberRolesService(
                companyMemberRoleRepositoryMock.Object,
                It.IsAny<ICompanyMemberRepository>(),
                It.IsAny<ICompanyRepository>(),
                It.IsAny<IValidator<CompanyMemberRole>>(),
                loggerMock.Object
            );

            // Act
            var result = await companyMemberRolesService.GetCompanyMemberRoleByIdAsync(companyMemberRoleId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Can not get the company member role by id.", result.Errors);
        }

        [Fact]
        public async Task UpdateCompanyMemberRoleAsync_CompanyMemberRoleNotFound_ReturnsFailure()
        {
            // Arrange
            var companyMemberRole = new CompanyMemberRole { Id = "invalid_id" };

            // Mocking the validation to return failure
            var validatorMock = new Mock<IValidator<CompanyMemberRole>>();
            validatorMock.Setup(x => x.ValidateAsync(companyMemberRole))
                         .ReturnsAsync((false, new List<string> { "Validation error" }));

            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            companyMemberRoleRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CompanyMemberRoleFilterBuilder>>()))
                                           .ReturnsAsync(new List<CompanyMemberRole>());

            var loggerMock = new Mock<ILogger<CompanyMemberRolesService>>();

            var companyMemberRolesService = new CompanyMemberRolesService(
                companyMemberRoleRepositoryMock.Object,
                It.IsAny<ICompanyMemberRepository>(),
                It.IsAny<ICompanyRepository>(),
                validatorMock.Object, // Injecting the mocked validator
                loggerMock.Object
            );

            // Act
            var result = await companyMemberRolesService.UpdateCompanyMemberRoleAsync(companyMemberRole);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Validation error", result.Errors);
        }

        [Fact]
        public async Task UpdateCompanyMemberRoleAsync_ExceptionThrown_ReturnsFailure()
        {
            // Arrange
            var companyMemberRole = new CompanyMemberRole { Id = "valid_id", Name = "New Name" };

            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            companyMemberRoleRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CompanyMemberRoleFilterBuilder>>()))
                                           .ThrowsAsync(new Exception("Simulated error"));

            var loggerMock = new Mock<ILogger<CompanyMemberRolesService>>();

            var companyMemberRolesService = new CompanyMemberRolesService(
                companyMemberRoleRepositoryMock.Object,
                It.IsAny<ICompanyMemberRepository>(),
                It.IsAny<ICompanyRepository>(),
                It.IsAny<IValidator<CompanyMemberRole>>(),
                loggerMock.Object
            );

            // Act
            var result = await companyMemberRolesService.UpdateCompanyMemberRoleAsync(companyMemberRole);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Can not update the company member role.", result.Errors);
        }

        [Fact]
        public async Task GetCompanyMemberRolesByCompanyIdAsync_CompanyMemberRolesFound_ReturnsSuccess()
        {
            // Arrange
            var companyId = "valid_company_id";
            var companyMemberRoles = new List<CompanyMemberRole> { new CompanyMemberRole(), new CompanyMemberRole() };

            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            companyMemberRoleRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CompanyMemberRoleFilterBuilder>>()))
                                           .ReturnsAsync(companyMemberRoles);

            var loggerMock = new Mock<ILogger<CompanyMemberRolesService>>();

            var companyMemberRolesService = new CompanyMemberRolesService(
                companyMemberRoleRepositoryMock.Object,
                It.IsAny<ICompanyMemberRepository>(),
                It.IsAny<ICompanyRepository>(),
                It.IsAny<IValidator<CompanyMemberRole>>(),
                loggerMock.Object
            );

            // Act
            var result = await companyMemberRolesService.GetCompanyMemberRolesByCompanyIdAsync(companyId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(companyMemberRoles, result.Data);
        }

        [Fact]
        public async Task GetCompanyMemberRolesByCompanyIdAsync_ExceptionThrown_ReturnsFailure()
        {
            // Arrange
            var companyId = "valid_company_id";

            var companyMemberRoleRepositoryMock = new Mock<ICompanyMemberRoleRepository>();
            companyMemberRoleRepositoryMock.Setup(x => x.GetFilteredItemsAsync(It.IsAny<Action<CompanyMemberRoleFilterBuilder>>()))
                                           .ThrowsAsync(new Exception("Simulated error"));

            var loggerMock = new Mock<ILogger<CompanyMemberRolesService>>();

            var companyMemberRolesService = new CompanyMemberRolesService(
                companyMemberRoleRepositoryMock.Object,
                It.IsAny<ICompanyMemberRepository>(),
                It.IsAny<ICompanyRepository>(),
                It.IsAny<IValidator<CompanyMemberRole>>(),
                loggerMock.Object
            );

            // Act
            var result = await companyMemberRolesService.GetCompanyMemberRolesByCompanyIdAsync(companyId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Can not get company member roles by company id.", result.Errors);
        }
    }
}
