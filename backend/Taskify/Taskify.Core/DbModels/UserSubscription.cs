using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.DbModels
{
    public class UserSubscription
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public User User { get; set; }
        public Subscription Subscription { get; set; }
        public DateTime StartDateTimeUtc { get; set; }
        public DateTime EndDateTimeUtc { get; set; }
    }
}
