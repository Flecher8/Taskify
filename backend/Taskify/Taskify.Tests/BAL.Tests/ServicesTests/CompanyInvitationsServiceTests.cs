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
using Taskify.Core.Result;
using Taskify.DAL.Helpers;
using Taskify.DAL.Interfaces;

namespace Taskify.Tests.BAL.Tests.ServicesTests
{
    public class CompanyInvitationsServiceTests
    {
        [Fact]
        public async Task CreateCompanyInvitationAsync_UserNotFound_ReturnsFailure()
        {
            // Arrange
            var userId = "invalid_user_id";
            var companyInvitation = new CompanyInvitation { Id = "valid_invitation_id", Company = new Company { Id = "valid_company_id" } };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId))
                              .ReturnsAsync((User)null); // Simulate user not found

            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(repo => repo.GetByIdAsync(companyInvitation.Company.Id))
                                 .ReturnsAsync(new Company());

            var notificationServiceMock = new Mock<INotificationService>();
            notificationServiceMock.Setup(service => service.CreateNotificationAsync(It.IsAny<Notification>()))
                                   .ReturnsAsync(ResultFactory.Success(new Notification()));

            var companyMembersServiceMock = new Mock<ICompanyMembersService>();
            companyMembersServiceMock.Setup(service => service.IsUserAlreadyMemberAsync(userId, It.IsAny<string>()))
                                     .ReturnsAsync(ResultFactory.Success(false));

            var validatorMock = new Mock<IValidator<CompanyInvitation>>();
            validatorMock.Setup(validator => validator.ValidateAsync(It.IsAny<CompanyInvitation>()))
                         .ReturnsAsync((true, new List<string>()));

            var loggerMock = new Mock<ILogger<CompanyInvitationsService>>();

            var service = new CompanyInvitationsService(
                It.IsAny<ICompanyInvitationRepository>(),
                userRepositoryMock.Object,
                companyRepositoryMock.Object,
                validatorMock.Object,
                loggerMock.Object,
                notificationServiceMock.Object,
                companyMembersServiceMock.Object
            );

            // Act
            var result = await service.CreateCompanyInvitationAsync(userId, companyInvitation);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Contains("Can not find user with such id.", result.Errors);
        }

        [Fact]
        public async Task RespondToCompanyInvitationAsync_ValidInput_ReturnsSuccess()
        {
            // Arrange
            var companyInvitationId = "valid_company_invitation_id";
            var isAccepted = true;

            var companyInvitation = new CompanyInvitation
            {
                Id = companyInvitationId,
                Notification = new Notification { Id = "valid_notification_id" },
                Company = new Company()
            };

            var notificationServiceMock = new Mock<INotificationService>();
            notificationServiceMock.Setup(service => service.GetNotificationByIdAsync(companyInvitation.Notification.Id))
                                   .ReturnsAsync(ResultFactory.Success(companyInvitation.Notification));

            var companyInvitationRepositoryMock = new Mock<ICompanyInvitationRepository>();
            companyInvitationRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<CompanyInvitationFilterBuilder>>()))
                                          .ReturnsAsync(new List<CompanyInvitation> { companyInvitation });

            companyInvitationRepositoryMock.Setup(repo => repo.UpdateAsync(companyInvitation))
                                          .Returns(Task.CompletedTask);

            var companyMembersServiceMock = new Mock<ICompanyMembersService>();

            var loggerMock = new Mock<ILogger<CompanyInvitationsService>>();

            var service = new CompanyInvitationsService(companyInvitationRepositoryMock.Object,
                                                        It.IsAny<IUserRepository>(),
                                                        It.IsAny<ICompanyRepository>(),
                                                        It.IsAny<IValidator<CompanyInvitation>>(),
                                                        loggerMock.Object,
                                                        notificationServiceMock.Object,
                                                        companyMembersServiceMock.Object);

            // Act
            var result = await service.RespondToCompanyInvitationAsync(companyInvitationId, isAccepted);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);

            // Verify that MarkNotificationAsReadAsync was called with the correct notification ID
            notificationServiceMock.Verify(service => service.MarkNotificationAsReadAsync(companyInvitation.Notification.Id), Times.Once);
        }

        [Fact]
        public async Task GetCompanyInvitationsByUserIdAsync_ValidUserId_ReturnsSuccess()
        {
            // Arrange
            var userId = "valid_user_id";
            var notifications = new List<Notification> { new Notification(), new Notification() };
            var companyInvitations = notifications.Select(notification => new CompanyInvitation { Notification = notification }).ToList();

            var notificationServiceMock = new Mock<INotificationService>();
            notificationServiceMock.Setup(service => service.GetNotificationsByUserIdAsync(userId))
                                   .ReturnsAsync(ResultFactory.Success(notifications));

            var companyInvitationRepositoryMock = new Mock<ICompanyInvitationRepository>();
            companyInvitationRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<CompanyInvitationFilterBuilder>>()))
                                          .ReturnsAsync(companyInvitations);

            var companyMembersServiceMock = new Mock<ICompanyMembersService>();

            var loggerMock = new Mock<ILogger<CompanyInvitationsService>>();

            var service = new CompanyInvitationsService(companyInvitationRepositoryMock.Object,
                                                        It.IsAny<IUserRepository>(),
                                                        It.IsAny<ICompanyRepository>(),
                                                        It.IsAny<IValidator<CompanyInvitation>>(),
                                                        loggerMock.Object,
                                                        notificationServiceMock.Object,
                                                        companyMembersServiceMock.Object);

            // Act
            var result = await service.GetCompanyInvitationsByUserIdAsync(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(companyInvitations, result.Data);
        }

        [Fact]
        public async Task GetUnreadCompanyInvitationsByUserIdAsync_Success_ReturnsSuccess()
        {
            // Arrange
            var userId = "valid_user_id";
            var notifications = new List<Notification> { new Notification(), new Notification() };
            var companyInvitations = notifications.Select(notification => new CompanyInvitation { Notification = notification }).ToList();

            var notificationServiceMock = new Mock<INotificationService>();
            notificationServiceMock.Setup(service => service.GetUnreadNotificationsByUserIdAsync(userId))
                                   .ReturnsAsync(ResultFactory.Success(notifications));

            var companyInvitationRepositoryMock = new Mock<ICompanyInvitationRepository>();
            companyInvitationRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<CompanyInvitationFilterBuilder>>()))
                                          .ReturnsAsync(companyInvitations);

            var companyMembersServiceMock = new Mock<ICompanyMembersService>();

            var loggerMock = new Mock<ILogger<CompanyInvitationsService>>();

            var service = new CompanyInvitationsService(companyInvitationRepositoryMock.Object,
                                                        It.IsAny<IUserRepository>(),
                                                        It.IsAny<ICompanyRepository>(),
                                                        It.IsAny<IValidator<CompanyInvitation>>(),
                                                        loggerMock.Object,
                                                        notificationServiceMock.Object,
                                                        companyMembersServiceMock.Object);

            // Act
            var result = await service.GetUnreadCompanyInvitationsByUserIdAsync(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(companyInvitations, result.Data);
        }

        [Fact]
        public async Task MarkCompanyInvitationAsReadAsync_Success_ReturnsSuccess()
        {
            // Arrange
            var companyInvitationId = "valid_company_invitation_id";
            var companyInvitation = new CompanyInvitation
            {
                Id = companyInvitationId,
                Notification = new Notification { Id = "valid_notification_id" }
            };

            var notificationServiceMock = new Mock<INotificationService>();
            notificationServiceMock.Setup(service => service.MarkNotificationAsReadAsync(companyInvitation.Notification.Id))
                                   .ReturnsAsync(ResultFactory.Success(true));

            var companyInvitationRepositoryMock = new Mock<ICompanyInvitationRepository>();
            companyInvitationRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<CompanyInvitationFilterBuilder>>()))
                                          .ReturnsAsync(new List<CompanyInvitation> { companyInvitation });

            var companyMembersServiceMock = new Mock<ICompanyMembersService>();

            var loggerMock = new Mock<ILogger<CompanyInvitationsService>>();

            var service = new CompanyInvitationsService(companyInvitationRepositoryMock.Object,
                                                        It.IsAny<IUserRepository>(),
                                                        It.IsAny<ICompanyRepository>(),
                                                        It.IsAny<IValidator<CompanyInvitation>>(),
                                                        loggerMock.Object,
                                                        notificationServiceMock.Object,
                                                        companyMembersServiceMock.Object);

            // Act
            var result = await service.MarkCompanyInvitationAsReadAsync(companyInvitationId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task GetUnreadCompanyInvitationsByUserIdAsync_Failure_ReturnsFailure()
        {
            // Arrange
            var userId = "valid_user_id";
            var error = "An error occurred while fetching notifications.";

            var notificationServiceMock = new Mock<INotificationService>();
            notificationServiceMock.Setup(service => service.GetUnreadNotificationsByUserIdAsync(userId))
                                   .ReturnsAsync(ResultFactory.Failure<List<Notification>>(error));

            var companyInvitationRepositoryMock = new Mock<ICompanyInvitationRepository>();
            var companyMembersServiceMock = new Mock<ICompanyMembersService>();
            var loggerMock = new Mock<ILogger<CompanyInvitationsService>>();

            var service = new CompanyInvitationsService(companyInvitationRepositoryMock.Object,
                                                        It.IsAny<IUserRepository>(),
                                                        It.IsAny<ICompanyRepository>(),
                                                        It.IsAny<IValidator<CompanyInvitation>>(),
                                                        loggerMock.Object,
                                                        notificationServiceMock.Object,
                                                        companyMembersServiceMock.Object);

            // Act
            var result = await service.GetUnreadCompanyInvitationsByUserIdAsync(userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(error, result.Errors.FirstOrDefault());
        }

        [Fact]
        public async Task MarkCompanyInvitationAsReadAsync_Failure_ReturnsFailure()
        {
            // Arrange
            var companyInvitationId = "valid_company_invitation_id";
            var error = "Can not mark the company invitation as read.";

            var notificationServiceMock = new Mock<INotificationService>();
            notificationServiceMock.Setup(service => service.MarkNotificationAsReadAsync(It.IsAny<string>()))
                                   .ReturnsAsync(ResultFactory.Failure<bool>(error));

            var companyInvitationRepositoryMock = new Mock<ICompanyInvitationRepository>();
            var companyMembersServiceMock = new Mock<ICompanyMembersService>();
            var loggerMock = new Mock<ILogger<CompanyInvitationsService>>();

            var service = new CompanyInvitationsService(companyInvitationRepositoryMock.Object,
                                                        It.IsAny<IUserRepository>(),
                                                        It.IsAny<ICompanyRepository>(),
                                                        It.IsAny<IValidator<CompanyInvitation>>(),
                                                        loggerMock.Object,
                                                        notificationServiceMock.Object,
                                                        companyMembersServiceMock.Object);

            // Act
            var result = await service.MarkCompanyInvitationAsReadAsync(companyInvitationId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(error, result.Errors.FirstOrDefault());
        }

        [Fact]
        public async Task DeleteCompanyInvitationAsync_Success_ReturnsSuccess()
        {
            // Arrange
            var companyInvitationId = "valid_company_invitation_id";
            var companyInvitation = new CompanyInvitation
            {
                Id = companyInvitationId,
                Notification = new Notification { Id = "valid_notification_id" }
            };

            var notificationServiceMock = new Mock<INotificationService>();
            notificationServiceMock.Setup(service => service.DeleteNotificationAsync(companyInvitation.Notification.Id))
                                   .ReturnsAsync(ResultFactory.Success(true));

            var companyInvitationRepositoryMock = new Mock<ICompanyInvitationRepository>();
            companyInvitationRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<CompanyInvitationFilterBuilder>>()))
                                          .ReturnsAsync(new List<CompanyInvitation> { companyInvitation });
            companyInvitationRepositoryMock.Setup(repo => repo.DeleteAsync(companyInvitation.Id))
                                          .Returns(Task.CompletedTask);

            var companyMembersServiceMock = new Mock<ICompanyMembersService>();
            var loggerMock = new Mock<ILogger<CompanyInvitationsService>>();

            var service = new CompanyInvitationsService(companyInvitationRepositoryMock.Object,
                                                        It.IsAny<IUserRepository>(),
                                                        It.IsAny<ICompanyRepository>(),
                                                        It.IsAny<IValidator<CompanyInvitation>>(),
                                                        loggerMock.Object,
                                                        notificationServiceMock.Object,
                                                        companyMembersServiceMock.Object);

            // Act
            var result = await service.DeleteCompanyInvitationAsync(companyInvitationId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task GetCompanyInvitationByIdAsync_Success_ReturnsSuccess()
        {
            // Arrange
            var companyInvitationId = "valid_company_invitation_id";
            var companyInvitation = new CompanyInvitation { Id = companyInvitationId };

            var companyInvitationRepositoryMock = new Mock<ICompanyInvitationRepository>();
            companyInvitationRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<CompanyInvitationFilterBuilder>>()))
                                          .ReturnsAsync(new List<CompanyInvitation> { companyInvitation });

            var companyMembersServiceMock = new Mock<ICompanyMembersService>();
            var notificationServiceMock = new Mock<INotificationService>();
            var loggerMock = new Mock<ILogger<CompanyInvitationsService>>();

            var service = new CompanyInvitationsService(companyInvitationRepositoryMock.Object,
                                                        It.IsAny<IUserRepository>(),
                                                        It.IsAny<ICompanyRepository>(),
                                                        It.IsAny<IValidator<CompanyInvitation>>(),
                                                        loggerMock.Object,
                                                        notificationServiceMock.Object,
                                                        companyMembersServiceMock.Object);

            // Act
            var result = await service.GetCompanyInvitationByIdAsync(companyInvitationId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(companyInvitation, result.Data);
        }

        [Fact]
        public async Task DeleteCompanyInvitationAsync_Failure_ReturnsFailure()
        {
            // Arrange
            var companyInvitationId = "invalid_company_invitation_id";
            var error = "Can not find company invitation with such id.";

            var notificationServiceMock = new Mock<INotificationService>();
            var companyInvitationRepositoryMock = new Mock<ICompanyInvitationRepository>();
            companyInvitationRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<CompanyInvitationFilterBuilder>>()))
                                          .ReturnsAsync(new List<CompanyInvitation>());
            var companyMembersServiceMock = new Mock<ICompanyMembersService>();
            var loggerMock = new Mock<ILogger<CompanyInvitationsService>>();

            var service = new CompanyInvitationsService(companyInvitationRepositoryMock.Object,
                                                        It.IsAny<IUserRepository>(),
                                                        It.IsAny<ICompanyRepository>(),
                                                        It.IsAny<IValidator<CompanyInvitation>>(),
                                                        loggerMock.Object,
                                                        notificationServiceMock.Object,
                                                        companyMembersServiceMock.Object);

            // Act
            var result = await service.DeleteCompanyInvitationAsync(companyInvitationId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(error, result.Errors.FirstOrDefault());
        }

        [Fact]
        public async Task GetCompanyInvitationByIdAsync_Failure_ReturnsFailure()
        {
            // Arrange
            var companyInvitationId = "invalid_company_invitation_id";
            var error = "Company invitation with such id does not exist.";

            var companyInvitationRepositoryMock = new Mock<ICompanyInvitationRepository>();
            companyInvitationRepositoryMock.Setup(repo => repo.GetFilteredItemsAsync(It.IsAny<Action<CompanyInvitationFilterBuilder>>()))
                                          .ReturnsAsync(new List<CompanyInvitation>());
            var companyMembersServiceMock = new Mock<ICompanyMembersService>();
            var notificationServiceMock = new Mock<INotificationService>();
            var loggerMock = new Mock<ILogger<CompanyInvitationsService>>();

            var service = new CompanyInvitationsService(companyInvitationRepositoryMock.Object,
                                                        It.IsAny<IUserRepository>(),
                                                        It.IsAny<ICompanyRepository>(),
                                                        It.IsAny<IValidator<CompanyInvitation>>(),
                                                        loggerMock.Object,
                                                        notificationServiceMock.Object,
                                                        companyMembersServiceMock.Object);

            // Act
            var result = await service.GetCompanyInvitationByIdAsync(companyInvitationId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(error, result.Errors.FirstOrDefault());
        }
    }
}
