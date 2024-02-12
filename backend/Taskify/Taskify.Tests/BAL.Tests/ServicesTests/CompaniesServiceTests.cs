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
using Taskify.DAL.Helpers;
using Taskify.DAL.Interfaces;

namespace Taskify.Tests.BAL.Tests.ServicesTests
{
    public class CompaniesServiceTests
    {
        [Fact]
        public async Task CreateCompanyAsync_ValidCompany_ReturnsSuccess()
        {
            // Arrange
            var company = new Company { Id = "valid_id", User = new User { Id = "valid_user_id" } };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetByIdAsync(company.User.Id))
                              .ReturnsAsync(new User());

            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<CompanyFilterBuilder>>()))
                                 .ReturnsAsync(new List<Company>());

            companyRepositoryMock.Setup(repo => repo.AddAsync(company))
                                 .ReturnsAsync(company);

            var validatorMock = new Mock<IValidator<Company>>();
            validatorMock.Setup(validator => validator.ValidateAsync(company))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<CompaniesService>>();
            var service = new CompaniesService(companyRepositoryMock.Object,
                                                userRepositoryMock.Object,
                                                validatorMock.Object,
                                                loggerMock.Object);

            // Act
            var result = await service.CreateCompanyAsync(company);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(company, result.Data);
            companyRepositoryMock.Verify(repo => repo.AddAsync(company), Times.Once);
        }

        [Fact]
        public async Task CreateCompanyAsync_InvalidCompany_ReturnsFailure()
        {
            // Arrange
            var company = new Company { User = new User { Id = "invalid_user_id" } };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetByIdAsync(company.User.Id))
                              .ReturnsAsync((User)null); // Simulating user not found

            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<CompanyFilterBuilder>>()))
                                 .ReturnsAsync(new List<Company>());

            var validatorMock = new Mock<IValidator<Company>>();
            validatorMock.Setup(validator => validator.ValidateAsync(company))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<CompaniesService>>();
            var service = new CompaniesService(companyRepositoryMock.Object,
                                                userRepositoryMock.Object,
                                                validatorMock.Object,
                                                loggerMock.Object);

            // Act
            var result = await service.CreateCompanyAsync(company);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Can not find user with such id.", result.Errors);
            companyRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Company>()), Times.Never);
        }

        [Fact]
        public async Task DeleteCompanyAsync_ValidCompanyId_ReturnsSuccess()
        {
            // Arrange
            var companyId = "valid_company_id";

            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(repo => repo.DeleteAsync(companyId))
                                 .Returns(Task.CompletedTask); // Simulating successful deletion

            var loggerMock = new Mock<ILogger<CompaniesService>>();
            var service = new CompaniesService(companyRepositoryMock.Object,
                                                It.IsAny<IUserRepository>(), // Mocked UserRepository
                                                It.IsAny<IValidator<Company>>(), // Mocked Validator
                                                loggerMock.Object);

            // Act
            var result = await service.DeleteCompanyAsync(companyId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task DeleteCompanyAsync_InvalidCompanyId_ReturnsFailure()
        {
            // Arrange
            var companyId = "invalid_company_id";

            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(repo => repo.DeleteAsync(companyId))
                                 .ThrowsAsync(new Exception("Simulated error")); // Simulating deletion failure

            var loggerMock = new Mock<ILogger<CompaniesService>>();
            var service = new CompaniesService(companyRepositoryMock.Object,
                                                It.IsAny<IUserRepository>(), // Mocked UserRepository
                                                It.IsAny<IValidator<Company>>(), // Mocked Validator
                                                loggerMock.Object);

            // Act
            var result = await service.DeleteCompanyAsync(companyId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Contains("Can not delete the company.", result.Errors);
        }

        [Fact]
        public async Task GetCompanyByIdAsync_ValidCompanyId_ReturnsSuccess()
        {
            // Arrange
            var companyId = "valid_company_id";
            var company = new Company { Id = companyId };

            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(repo => repo.GetByIdAsync(companyId))
                                 .ReturnsAsync(company); // Simulating successful retrieval

            var loggerMock = new Mock<ILogger<CompaniesService>>();
            var service = new CompaniesService(companyRepositoryMock.Object,
                                                It.IsAny<IUserRepository>(), // Mocked UserRepository
                                                It.IsAny<IValidator<Company>>(), // Mocked Validator
                                                loggerMock.Object);

            // Act
            var result = await service.GetCompanyByIdAsync(companyId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(company, result.Data);
        }

        [Fact]
        public async Task GetCompanyByIdAsync_InvalidCompanyId_ReturnsFailure()
        {
            // Arrange
            var companyId = "invalid_company_id";

            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(repo => repo.GetByIdAsync(companyId))
                                 .ReturnsAsync((Company)null); // Simulating company not found

            var loggerMock = new Mock<ILogger<CompaniesService>>();
            var service = new CompaniesService(companyRepositoryMock.Object,
                                                It.IsAny<IUserRepository>(), // Mocked UserRepository
                                                It.IsAny<IValidator<Company>>(), // Mocked Validator
                                                loggerMock.Object);

            // Act
            var result = await service.GetCompanyByIdAsync(companyId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Company with such id does not exist.", result.Errors);
        }

        [Fact]
        public async Task UpdateCompanyAsync_ValidCompany_ReturnsSuccess()
        {
            // Arrange
            var company = new Company { Id = "valid_company_id", Name = "New Company Name" };

            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<CompanyFilterBuilder>>()))
                                 .ReturnsAsync(new List<Company> { company }); // Simulating successful retrieval

            var validatorMock = new Mock<IValidator<Company>>();
            validatorMock.Setup(x => x.ValidateAsync(company))
                         .ReturnsAsync((true, new List<string>())); // Mocking validation result

            var loggerMock = new Mock<ILogger<CompaniesService>>();
            var service = new CompaniesService(companyRepositoryMock.Object,
                                                It.IsAny<IUserRepository>(), // Mocked UserRepository
                                                validatorMock.Object, // Mocked Validator
                                                loggerMock.Object);

            // Act
            var result = await service.UpdateCompanyAsync(company);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task UpdateCompanyAsync_InvalidCompanyId_ReturnsFailure()
        {
            // Arrange
            var company = new Company { Id = "invalid_company_id", Name = "New Company Name" };

            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<CompanyFilterBuilder>>()))
                                 .ReturnsAsync(new List<Company>()); // Simulating company not found

            var validatorMock = new Mock<IValidator<Company>>();
            validatorMock.Setup(x => x.ValidateAsync(company))
                         .ReturnsAsync((true, new List<string>())); // Mocking validation result

            var loggerMock = new Mock<ILogger<CompaniesService>>();
            var service = new CompaniesService(companyRepositoryMock.Object,
                                                It.IsAny<IUserRepository>(), // Mocked UserRepository
                                                validatorMock.Object, // Mocked Validator
                                                loggerMock.Object);

            // Act
            var result = await service.UpdateCompanyAsync(company);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Data);
            Assert.Contains("Can not find company with such id.", result.Errors);
        }

        [Fact]
        public async Task GetCompaniesByUserIdAsync_ValidUserId_ReturnsSuccess()
        {
            // Arrange
            var userId = "valid_user_id";
            var companies = new List<Company> { new Company(), new Company() };

            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<CompanyFilterBuilder>>()))
                                 .ReturnsAsync(companies); // Simulating successful retrieval

            var loggerMock = new Mock<ILogger<CompaniesService>>();
            var service = new CompaniesService(companyRepositoryMock.Object,
                                                It.IsAny<IUserRepository>(), // Mocked UserRepository
                                                It.IsAny<IValidator<Company>>(), // Mocked Validator
                                                loggerMock.Object);

            // Act
            var result = await service.GetCompaniesByUserIdAsync(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(companies, result.Data);
        }

        [Fact]
        public async Task GetCompaniesByUserIdAsync_InvalidUserId_ReturnsFailure()
        {
            // Arrange
            var userId = "invalid_user_id";

            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<CompanyFilterBuilder>>()))
                                 .ThrowsAsync(new Exception("Simulated error")); // Simulating retrieval failure

            var loggerMock = new Mock<ILogger<CompaniesService>>();
            var service = new CompaniesService(companyRepositoryMock.Object,
                                                It.IsAny<IUserRepository>(), // Mocked UserRepository
                                                It.IsAny<IValidator<Company>>(), // Mocked Validator
                                                loggerMock.Object);

            // Act
            var result = await service.GetCompaniesByUserIdAsync(userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Can not get companies by user id.", result.Errors);
        }
    }
}
