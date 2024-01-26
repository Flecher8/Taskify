using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface ISubscriptionService
    {
        Task<Result<Subscription>> GetSubscriptionByIdAsync(string id);

        Task<Result<List<Subscription>>> GetAllSubscriptionsAsync();

        Task<Result<Subscription>> CreateSubscriptionAsync(Subscription subscription);

        Task<Result<bool>> UpdateSubscriptionAsync(Subscription subscription);

        Task<Result<bool>> DeleteSubscriptionAsync(string id);
    }
}
