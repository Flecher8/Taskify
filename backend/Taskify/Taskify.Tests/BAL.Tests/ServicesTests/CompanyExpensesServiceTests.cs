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
using Taskify.Core.Enums;
using Taskify.DAL.Helpers;
using Taskify.DAL.Interfaces;

namespace Taskify.Tests.BAL.Tests.ServicesTests
{
    public class CompanyExpensesServiceTests
    {
        [Fact]
        public async Task CreateCompanyExpenseAsync_ValidCompanyExpense_ReturnsSuccess()
        {
            // Arrange
            var companyExpense = new CompanyExpense
            {
                Id = "valid_id",
                Name = "Expense Name",
                Amount = 100,
                Frequency = CompanyExpenseFrequency.Monthly,
                Company = new Company { Id = "valid_company_id" }
            };

            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(repo => repo.GetByIdAsync("valid_company_id"))
                                 .ReturnsAsync(new Company { Id = "valid_company_id" });

            var companyExpenseRepositoryMock = new Mock<ICompanyExpenseRepository>();
            companyExpenseRepositoryMock.Setup(repo => repo.AddAsync(companyExpense))
                                         .ReturnsAsync(companyExpense);

            var validatorMock = new Mock<IValidator<CompanyExpense>>();
            validatorMock.Setup(validator => validator.ValidateAsync(companyExpense))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<CompanyExpensesService>>();
            var service = new CompanyExpensesService(companyExpenseRepositoryMock.Object,
                                                     companyRepositoryMock.Object,
                                                     validatorMock.Object,
                                                     loggerMock.Object);

            // Act
            var result = await service.CreateCompanyExpenseAsync(companyExpense);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(companyExpense, result.Data);
        }

        [Fact]
        public async Task CreateCompanyExpenseAsync_InvalidCompanyExpense_ReturnsFailure()
        {
            // Arrange
            var companyExpense = new CompanyExpense { /* Initialize with invalid data */ };
            var companyExpenseRepositoryMock = new Mock<ICompanyExpenseRepository>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            var validatorMock = new Mock<IValidator<CompanyExpense>>();
            validatorMock.Setup(validator => validator.ValidateAsync(companyExpense))
                         .ReturnsAsync((false, new List<string>()));
            var loggerMock = new Mock<ILogger<CompanyExpensesService>>();
            var service = new CompanyExpensesService(companyExpenseRepositoryMock.Object,
                                                     companyRepositoryMock.Object,
                                                     validatorMock.Object,
                                                     loggerMock.Object);

            // Act
            var result = await service.CreateCompanyExpenseAsync(companyExpense);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            companyExpenseRepositoryMock.Verify(repo => repo.AddAsync(companyExpense), Times.Never);
        }

        [Fact]
        public async Task DeleteCompanyExpenseAsync_ValidId_ReturnsSuccess()
        {
            // Arrange
            var expenseId = "valid_expense_id";
            var companyExpenseRepositoryMock = new Mock<ICompanyExpenseRepository>();
            companyExpenseRepositoryMock.Setup(repo => repo.DeleteAsync(expenseId))
                                         .Returns(Task.CompletedTask);
            var loggerMock = new Mock<ILogger<CompanyExpensesService>>();
            var service = new CompanyExpensesService(companyExpenseRepositoryMock.Object,
                                                     It.IsAny<ICompanyRepository>(),
                                                     It.IsAny<IValidator<CompanyExpense>>(),
                                                     loggerMock.Object);

            // Act
            var result = await service.DeleteCompanyExpenseAsync(expenseId);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetCompanyExpenseByIdAsync_ValidId_ReturnsSuccess()
        {
            // Arrange
            var expenseId = "valid_expense_id";
            var companyExpense = new CompanyExpense { Id = expenseId };
            var companyExpenseRepositoryMock = new Mock<ICompanyExpenseRepository>();
            companyExpenseRepositoryMock.Setup(repo => repo.GetByIdAsync(expenseId))
                                         .ReturnsAsync(companyExpense);
            var loggerMock = new Mock<ILogger<CompanyExpensesService>>();
            var service = new CompanyExpensesService(companyExpenseRepositoryMock.Object,
                                                     It.IsAny<ICompanyRepository>(),
                                                     It.IsAny<IValidator<CompanyExpense>>(),
                                                     loggerMock.Object);

            // Act
            var result = await service.GetCompanyExpenseByIdAsync(expenseId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(companyExpense, result.Data);
        }

        [Fact]
        public async Task UpdateCompanyExpenseAsync_ValidCompanyExpense_ReturnsSuccess()
        {
            // Arrange
            var expenseId = "valid_expense_id";
            var companyExpenseToUpdate = new CompanyExpense { Id = expenseId, Name = "Updated Name" };
            var existingCompanyExpense = new CompanyExpense { Id = expenseId };
            var companyExpenseRepositoryMock = new Mock<ICompanyExpenseRepository>();
            companyExpenseRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<CompanyExpenseFilterBuilder>>()))
                                         .ReturnsAsync(new List<CompanyExpense> { existingCompanyExpense });
            companyExpenseRepositoryMock.Setup(repo => repo.UpdateAsync(existingCompanyExpense))
                                         .Returns(Task.CompletedTask);
            var validatorMock = new Mock<IValidator<CompanyExpense>>();
            validatorMock.Setup(validator => validator.ValidateAsync(companyExpenseToUpdate))
                         .ReturnsAsync((true, new List<string>()));
            var loggerMock = new Mock<ILogger<CompanyExpensesService>>();
            var service = new CompanyExpensesService(companyExpenseRepositoryMock.Object,
                                                     It.IsAny<ICompanyRepository>(),
                                                     validatorMock.Object,
                                                     loggerMock.Object);

            // Act
            var result = await service.UpdateCompanyExpenseAsync(companyExpenseToUpdate);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetCompanyExpensesByCompanyIdAsync_ValidId_ReturnsSuccess()
        {
            // Arrange
            var companyId = "valid_company_id";
            var expenses = new List<CompanyExpense> { new CompanyExpense(), new CompanyExpense() };
            var companyExpenseRepositoryMock = new Mock<ICompanyExpenseRepository>();
            companyExpenseRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<CompanyExpenseFilterBuilder>>()))
                                         .ReturnsAsync(expenses);
            var loggerMock = new Mock<ILogger<CompanyExpensesService>>();
            var service = new CompanyExpensesService(companyExpenseRepositoryMock.Object,
                                                     It.IsAny<ICompanyRepository>(),
                                                     It.IsAny<IValidator<CompanyExpense>>(),
                                                     loggerMock.Object);

            // Act
            var result = await service.GetCompanyExpensesByCompanyIdAsync(companyId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expenses, result.Data);
        }

        [Fact]
        public async Task DeleteCompanyExpenseAsync_InvalidCompanyExpenseId_ReturnsFailure()
        {
            // Arrange
            var companyExpenseId = "invalid_id";

            var companyExpenseRepositoryMock = new Mock<ICompanyExpenseRepository>();
            companyExpenseRepositoryMock.Setup(repo => repo.DeleteAsync(companyExpenseId))
                                         .ThrowsAsync(new Exception("Unable to delete company expense."));

            var loggerMock = new Mock<ILogger<CompanyExpensesService>>();
            var service = new CompanyExpensesService(companyExpenseRepositoryMock.Object,
                                                     null,
                                                     null,
                                                     loggerMock.Object);

            // Act
            var result = await service.DeleteCompanyExpenseAsync(companyExpenseId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Can not delete the company expense.", result.Errors);
        }

        [Fact]
        public async Task GetCompanyExpenseByIdAsync_InvalidCompanyExpenseId_ReturnsFailure()
        {
            // Arrange
            var companyExpenseId = "invalid_id";

            var companyExpenseRepositoryMock = new Mock<ICompanyExpenseRepository>();
            companyExpenseRepositoryMock.Setup(repo => repo.GetByIdAsync(companyExpenseId))
                                         .ThrowsAsync(new Exception("Unable to retrieve company expense by id."));

            var loggerMock = new Mock<ILogger<CompanyExpensesService>>();
            var service = new CompanyExpensesService(companyExpenseRepositoryMock.Object,
                                                     null,
                                                     null,
                                                     loggerMock.Object);

            // Act
            var result = await service.GetCompanyExpenseByIdAsync(companyExpenseId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Can not get the company expense by id.", result.Errors);
        }

        [Fact]
        public async Task UpdateCompanyExpenseAsync_InvalidCompanyExpenseId_ReturnsFailure()
        {
            // Arrange
            var companyExpense = new CompanyExpense { Id = "invalid_id" };

            var companyExpenseRepositoryMock = new Mock<ICompanyExpenseRepository>();
            companyExpenseRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<CompanyExpenseFilterBuilder>>()))
                                         .ReturnsAsync(new List<CompanyExpense>());

            var validatorMock = new Mock<IValidator<CompanyExpense>>();
            validatorMock.Setup(validator => validator.ValidateAsync(companyExpense))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<CompanyExpensesService>>();
            var service = new CompanyExpensesService(companyExpenseRepositoryMock.Object,
                                             null,
                                             validatorMock.Object,
                                             loggerMock.Object);

            // Act
            var result = await service.UpdateCompanyExpenseAsync(companyExpense);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Can not find company expense with such id.", result.Errors);
        }

        [Fact]
        public async Task GetCompanyExpensesByCompanyIdAsync_InvalidCompanyId_ReturnsFailure()
        {
            // Arrange
            var companyId = "invalid_id";

            var companyExpenseRepositoryMock = new Mock<ICompanyExpenseRepository>();
            companyExpenseRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<CompanyExpenseFilterBuilder>>()))
                                         .ThrowsAsync(new Exception("Unable to retrieve company expenses by company id."));

            var loggerMock = new Mock<ILogger<CompanyExpensesService>>();
            var service = new CompanyExpensesService(companyExpenseRepositoryMock.Object,
                                                     null,
                                                     null,
                                                     loggerMock.Object);

            // Act
            var result = await service.GetCompanyExpensesByCompanyIdAsync(companyId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Can not get company expenses by company id.", result.Errors);
        }
    }
}
