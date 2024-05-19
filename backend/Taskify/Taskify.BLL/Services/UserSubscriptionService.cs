using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.BLL.Interfaces;
using Taskify.Core.DbModels;
using Taskify.Core.Result;
using Taskify.DAL.Interfaces;
using Taskify.DAL.Repositories;

namespace Taskify.BLL.Services
{
    public class UserSubscriptionService : IUserSubscriptionService
    {
        private readonly IUserSubscriptionRepository _userSubscriptionRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserSubscriptionService> _logger;

        public UserSubscriptionService(
            IUserSubscriptionRepository userSubscriptionRepository,
            ISubscriptionRepository subscriptionRepository,
            IUserRepository userRepository,
            ILogger<UserSubscriptionService> logger)
        {
            _userSubscriptionRepository = userSubscriptionRepository;
            _subscriptionRepository = subscriptionRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<Result<Subscription>> GetUserSubscription(string userId)
        {
            try
            {
                var userSubscriptions = (await _userSubscriptionRepository.GetFilteredItemsAsync(
                    builder => builder
                                    .IncludeSubscriptionEntity()
                                    .WithFilter(us => us.User.Id == userId && us.EndDateTimeUtc > DateTime.UtcNow)
                )).OrderBy(p => p.StartDateTimeUtc);

                var activeSubscription = userSubscriptions.FirstOrDefault();

                return activeSubscription != null
                    ? ResultFactory.Success(activeSubscription.Subscription)
                    : ResultFactory.Success(GetDefaultSubscription());
            }
            catch (Exception ex)
            {
                return ResultFactory.Failure<Subscription>("An error occurred while fetching user subscription.");
            }
        }

        private Subscription GetDefaultSubscription()
        {
            var subscription = new Subscription();
            subscription.Name = "Default";
            subscription.ProjectMembersLimit = 10;
            subscription.ProjectsLimit = 10;
            subscription.ProjectTasksLimit = 100;
            subscription.ProjectSectionsLimit = 100;
            return subscription;
        }

        public async Task<Result<bool>> CreateUserSubscription(string userId, string subscriptionId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ResultFactory.Failure<bool>("User not found.");
                }

                var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId);
                if (subscription == null)
                {
                    return ResultFactory.Failure<bool>("Subscription not found.");
                }

                var activeSubscriptions = await GetActiveSubscriptions(userId);

                if (activeSubscriptions.Any())
                {
                    await CreateNewUserSubscription(subscription, activeSubscriptions);
                }
                else
                {
                    await CreateUserSubscriptionWithoutActiveSubscriptions(user, subscription);
                }

                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating user subscription.");
                return ResultFactory.Failure<bool>("An error occurred while creating user subscription.");
            }
        }

        private async Task<List<UserSubscription>> GetActiveSubscriptions(string userId)
        {
            return await _userSubscriptionRepository
                .GetFilteredItemsAsync(us => us.User.Id == userId && us.EndDateTimeUtc > DateTime.UtcNow);
        }

        private async Task CreateNewUserSubscription(Subscription subscription, List<UserSubscription> activeSubscriptions)
        {
            var lastActiveSubscription = activeSubscriptions.OrderByDescending(us => us.EndDateTimeUtc).First();

            var newUserSubscription = new UserSubscription
            {
                User = lastActiveSubscription.User,
                Subscription = subscription,
                StartDateTimeUtc = lastActiveSubscription.EndDateTimeUtc,
                EndDateTimeUtc = lastActiveSubscription.EndDateTimeUtc.AddDays(subscription.DurationInDays),
                CreatedAt = DateTime.UtcNow
            };

            await _userSubscriptionRepository.AddAsync(newUserSubscription);
        }

        private async Task CreateUserSubscriptionWithoutActiveSubscriptions(User user, Subscription subscription)
        {
            var newUserSubscription = new UserSubscription
            {
                User = user,
                Subscription = subscription,
                StartDateTimeUtc = DateTime.UtcNow,
                EndDateTimeUtc = DateTime.UtcNow.AddDays(subscription.DurationInDays),
                CreatedAt = DateTime.UtcNow
            };

            await _userSubscriptionRepository.AddAsync(newUserSubscription);
        }
    }
}
