using Microsoft.EntityFrameworkCore.Query.Internal;
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
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IValidator<Subscription> _validator;
        private readonly ILogger<SubscriptionService> _logger;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository, 
            IValidator<Subscription> validator,
            ILogger<SubscriptionService> logger
            ) 
        {
            _subscriptionRepository = subscriptionRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<Subscription>> CreateSubscriptionAsync(Subscription subscription)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(subscription);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<Subscription>(validation.ErrorMessages);
                }

                var result = await _subscriptionRepository.AddAsync(subscription);
                
                return ResultFactory.Success(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<Subscription>("Can not create new subscription");
            }
        }

        public async Task<Result<bool>> DeleteSubscriptionAsync(string id)
        {
            try
            {
                await _subscriptionRepository.DeleteAsync(id);
                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not delete subscription");
                
            }
        }

        public async Task<Result<List<Subscription>>> GetAllSubscriptionsAsync()
        {
            try
            {
                var result = await _subscriptionRepository.GetAllAsync();
                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<List<Subscription>>("Can not get all subscriptions");
            }
        }

        public async Task<Result<Subscription>> GetSubscriptionByIdAsync(string id)
        {
            try
            {
                var result = await _subscriptionRepository.GetByIdAsync(id);

                if (result == null)
                {
                    return ResultFactory.Failure<Subscription>("Subscription with such id does not exist");
                }

                return ResultFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<Subscription>("Can not get subscription by id");
            }
        }

        public async Task<Result<bool>> UpdateSubscriptionAsync(Subscription subscription)
        {
            try
            {
                // Validation
                var validation = await _validator.ValidateAsync(subscription);
                if (!validation.IsValid)
                {
                    return ResultFactory.Failure<bool>(validation.ErrorMessages);
                }

                await _subscriptionRepository.UpdateAsync(subscription);
                return ResultFactory.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ResultFactory.Failure<bool>("Can not update subscription");
            }
        }
    }
}
