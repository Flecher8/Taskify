using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Interfaces;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;
using Taskify.Core.Enums;
using Taskify.Core.Result;
using Taskify.DAL.Interfaces;

namespace Taskify.BLL.Services
{
    public class CompanyInvitationsService : ICompanyInvitationsService
    {
        private readonly ICompanyInvitationRepository _companyInvitationRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IValidator<CompanyInvitation> _validator;
        private readonly ILogger<CompanyInvitationsService> _logger;
        private readonly INotificationService _notificationService;
        private readonly ICompanyMembersService _companyMembersService;

        public CompanyInvitationsService(
            ICompanyInvitationRepository companyInvitationRepository,
            IUserRepository userRepository,
            ICompanyRepository companyRepository,
            IValidator<CompanyInvitation> validator,
            ILogger<CompanyInvitationsService> logger,
            INotificationService notificationService,
            ICompanyMembersService companyMembersService
        )
        {
            _companyInvitationRepository = companyInvitationRepository;
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _validator = validator;
            _logger = logger;
            _notificationService = notificationService;
            _companyMembersService = companyMembersService;
        }

        public async Task<Result<CompanyInvitation>> CreateCompanyInvitationAsync(string userId, CompanyInvitation companyInvitation)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(companyInvitation);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<CompanyInvitation>(validation.ErrorMessages);
                }

                if (companyInvitation.Company == null || string.IsNullOrEmpty(companyInvitation.Company.Id))
                {
                    return ResultFactory.Failure<CompanyInvitation>("Invalid company specified.");
                }

                if (string.IsNullOrEmpty(userId))
                {
                    return ResultFactory.Failure<CompanyInvitation>("Invalid user specified.");
                }

                // Check if the user is already a member of the company using CompanyMembersService
                var isUserAlreadyMemberResult = await _companyMembersService.IsUserAlreadyMemberAsync(userId, companyInvitation.Company.Id);

                if (isUserAlreadyMemberResult.IsSuccess && isUserAlreadyMemberResult.Data)
                {
                    return ResultFactory.Failure<CompanyInvitation>("User is already a member of this company.");
                }

                var user = await _userRepository.GetByIdAsync(userId);
                var company = await _companyRepository.GetByIdAsync(companyInvitation.Company.Id);

                if (user == null)
                {
                    return ResultFactory.Failure<CompanyInvitation>("Can not find user with such id.");
                }

                if (company == null)
                {
                    return ResultFactory.Failure<CompanyInvitation>("Can not find company with such id.");
                }

                // Check if the user is not the creator of the company
                if (userId == company.User.Id)
                {
                    return ResultFactory.Failure<CompanyInvitation>("Company creators cannot send an invitation to themselves.");
                }

                Notification notification = new Notification();
                notification.User = user;
                notification.NotificationType = NotificationType.CompanyInvitation;

                var notificationResult = await _notificationService.CreateNotificationAsync(notification);
                if (!notificationResult.IsSuccess)
                {
                    return ResultFactory.Failure<CompanyInvitation>(notificationResult.Errors);
                }

                companyInvitation.Notification = notificationResult.Data;
                companyInvitation.IsAccepted = null;
                companyInvitation.Company = company;

                var result = await _companyInvitationRepository.AddAsync(companyInvitation);

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<CompanyInvitation>("Can not create a new company invitation.");
            }
        }

        public async Task<Result<bool>> RespondToCompanyInvitationAsync(string companyInvitationId, bool isAccepted)
        {
            try
            {
                var companyInvitation = (await _companyInvitationRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeNotificationEntity()
                        .IncludeCompanyEntity()
                        .WithFilter(ci => ci.Id == companyInvitationId)
                    )).FirstOrDefault();

                if (companyInvitation == null)
                {
                    return ResultFactory.Failure<bool>("Company invitation with such id does not exist.");
                }

                if (companyInvitation.IsAccepted != null)
                {
                    return ResultFactory.Failure<bool>("A response to this company invitation has already been received.");
                }

                var notificationFindResult = await _notificationService.GetNotificationByIdAsync(companyInvitation.Notification.Id);

                if (!notificationFindResult.IsSuccess)
                {
                    return ResultFactory.Failure<bool>(notificationFindResult.Errors);
                }

                var notification = notificationFindResult.Data;

                companyInvitation.IsAccepted = isAccepted;

                await _notificationService.MarkNotificationAsReadAsync(companyInvitation.Notification.Id);

                await _companyInvitationRepository.UpdateAsync(companyInvitation);

                if (isAccepted)
                {
                    await AddUserToCompany(companyInvitation.Company, notification.User);
                }

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not respond to the company invitation.");
            }
        }

        public async Task<Result<List<CompanyInvitation>>> GetCompanyInvitationsByUserIdAsync(string userId)
        {
            try
            {
                var notificationResult = await _notificationService.GetNotificationsByUserIdAsync(userId);

                if (!notificationResult.IsSuccess)
                {
                    return ResultFactory.Failure<List<CompanyInvitation>>(notificationResult.Errors);
                }

                var companyInvitationNotifications = notificationResult.Data
                    .Where(n => n.NotificationType == NotificationType.CompanyInvitation)
                    .ToList();

                var notificationIds = companyInvitationNotifications.Select(n => n.Id).ToList();

                var companyInvitationResult = await _companyInvitationRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeNotificationEntity()
                        .IncludeCompanyEntity()
                        .WithFilter(ci => notificationIds.Contains(ci.Notification.Id))
                );

                return ResultFactory.Success(companyInvitationResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<CompanyInvitation>>("Can not get company invitations by user id.");
            }
        }

        public async Task<Result<List<CompanyInvitation>>> GetUnreadCompanyInvitationsByUserIdAsync(string userId)
        {
            try
            {
                var notificationResult = await _notificationService.GetUnreadNotificationsByUserIdAsync(userId);

                if (!notificationResult.IsSuccess)
                {
                    return ResultFactory.Failure<List<CompanyInvitation>>(notificationResult.Errors);
                }

                var notificationIds = notificationResult.Data.Select(n => n.Id).ToList();

                var companyInvitationResult = await _companyInvitationRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeNotificationEntity()
                        .IncludeCompanyEntity()
                        .WithFilter(ci => notificationIds.Contains(ci.Notification.Id))
                );

                return ResultFactory.Success(companyInvitationResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<CompanyInvitation>>("Can not get unread company invitations by user id.");
            }
        }

        public async Task<Result<bool>> MarkCompanyInvitationAsReadAsync(string companyInvitationId)
        {
            try
            {
                var companyInvitation = (await _companyInvitationRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeNotificationEntity()
                        .WithFilter(ci => ci.Id == companyInvitationId)
                    )).FirstOrDefault();

                if (companyInvitation == null)
                {
                    return ResultFactory.Failure<bool>("Can not find company invitation with such id.");
                }

                await _notificationService.MarkNotificationAsReadAsync(companyInvitation.Notification.Id);
                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not mark the company invitation as read.");
            }
        }

        public async Task<Result<bool>> DeleteCompanyInvitationAsync(string companyInvitationId)
        {
            try
            {
                var companyInvitation = (await _companyInvitationRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeNotificationEntity()
                        .WithFilter(ci => ci.Id == companyInvitationId)
                    )).FirstOrDefault();

                if (companyInvitation == null)
                {
                    return ResultFactory.Failure<bool>("Can not find company invitation with such id.");
                }

                await _notificationService.DeleteNotificationAsync(companyInvitation.Notification.Id);
                await _companyInvitationRepository.DeleteAsync(companyInvitation.Id);

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not delete the company invitation.");
            }
        }

        public async Task<Result<CompanyInvitation>> GetCompanyInvitationByIdAsync(string companyInvitationId)
        {
            try
            {
                var companyInvitation = (await _companyInvitationRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeNotificationEntity()
                        .IncludeCompanyEntity()
                        .WithFilter(ci => ci.Id == companyInvitationId)
                    )).FirstOrDefault();

                if (companyInvitation == null)
                {
                    return ResultFactory.Failure<CompanyInvitation>("Company invitation with such id does not exist.");
                }

                return ResultFactory.Success(companyInvitation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<CompanyInvitation>("Can not get company invitation by id.");
            }
        }

        private async Task AddUserToCompany(Company company, User user)
        {
            CompanyMember companyMember = new CompanyMember();
            companyMember.Company = company;
            companyMember.User = user;

            await _companyMembersService.CreateCompanyMemberAsync(companyMember);
        }
    }
}
