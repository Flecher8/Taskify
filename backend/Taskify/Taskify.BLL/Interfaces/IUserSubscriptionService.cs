using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface IUserSubscriptionService
    {
        Task<Result<Subscription>> GetUserSubscription(string userId);
        Task<Result<bool>> CreateUserSubscription(string userId, string subscriptionId);
    }
}
