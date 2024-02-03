using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Interfaces;
using Taskify.BLL.Validation;
using Taskify.Core.DbModels;
using Taskify.Core.Result;
using Taskify.DAL.Interfaces;

namespace Taskify.BLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<Notification> _validator;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(INotificationRepository notificationRepository,
            IUserRepository userRepository,
            IValidator<Notification> validator,
            ILogger<NotificationService> logger)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Notification>> CreateNotificationAsync(Notification notification)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(notification);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<Notification>(validation.ErrorMessages);
                }

                if (notification.User == null || string.IsNullOrEmpty(notification.User.Id))
                {
                    return ResultFactory.Failure<Notification>("Invalid user specified.");
                }

                var user = await _userRepository.GetByIdAsync(notification.User.Id);

                if (user == null)
                {
                    return ResultFactory.Failure<Notification>("Can not find user with such id.");
                }

                notification.User = user;
                notification.CreatedAt = DateTime.UtcNow;
                notification.IsRead = false;

                var result = await _notificationRepository.AddAsync(notification);

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<Notification>("Can not create a new notification.");
            }
        }

        public async Task<Result<bool>> MarkNotificationAsReadAsync(string notificationId)
        {
            try
            {
                var result = await _notificationRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeUserEntity()
                        .WithFilter(n => n.Id == notificationId)
                    );

                var notification = result.FirstOrDefault();

                if (notification == null)
                {
                    return ResultFactory.Failure<bool>("Notification with such id does not exist.");
                }

                notification.IsRead = true;

                await _notificationRepository.UpdateAsync(notification);

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not mark the notification as read.");
            }
        }

        public async Task<Result<List<Notification>>> GetNotificationsByUserIdAsync(string userId)
        {
            try
            {
                var result = await _notificationRepository.GetFilteredItemsAsync(
                    builder => builder
                                    .IncludeUserEntity()
                                    .WithFilter(n => n.User.Id == userId)
                    );
                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<Notification>>("Can not get notifications by user id.");
            }
        }

        public async Task<Result<List<Notification>>> GetUnreadNotificationsByUserIdAsync(string userId)
        {
            try
            {
                var result = await _notificationRepository.GetFilteredItemsAsync(
                    builder => builder
                                    .IncludeUserEntity()
                                    .WithFilter(n => n.User.Id == userId && !n.IsRead)
                    );
                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<Notification>>("Can not get unread notifications by user id.");
            }
        }

        public async Task<Result<bool>> DeleteNotificationAsync(string notificationId)
        {
            try
            {
                await _notificationRepository.DeleteAsync(notificationId);
                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not delete the notification.");
            }
        }

        public async Task<Result<Notification>> GetNotificationByIdAsync(string notificationId)
        {
            try
            {
                var result = await _notificationRepository.GetFilteredItemsAsync(
                    builder => builder
                        .IncludeUserEntity()
                        .WithFilter(n => n.Id == notificationId)
                    );

                var notification = result.FirstOrDefault();

                if (notification == null)
                {
                    return ResultFactory.Failure<Notification>("Notification with such id does not exist.");
                }

                return ResultFactory.Success(notification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<Notification>("Can not get the notification by id.");
            }
        }

    }
}
