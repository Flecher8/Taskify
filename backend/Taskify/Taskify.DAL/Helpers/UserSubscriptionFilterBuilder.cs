using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.DAL.Helpers
{
    public class UserSubscriptionFilterBuilder
    {
        public bool IncludeUser { get; private set; }
        public bool IncludeSubscription { get; private set; }
        public Expression<Func<UserSubscription, bool>> Filter { get; private set; } = _ => true;

        public UserSubscriptionFilterBuilder IncludeUserEntity()
        {
            IncludeUser = true;
            return this;
        }

        public UserSubscriptionFilterBuilder IncludeSubscriptionEntity()
        {
            IncludeSubscription = true;
            return this;
        }

        public UserSubscriptionFilterBuilder WithFilter(Expression<Func<UserSubscription, bool>> filter)
        {
            Filter = filter;
            return this;
        }
    }
}
