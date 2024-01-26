using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.Core.Dtos
{
    public class CreateUserSubscriptionDto
    {
        public string UserId { get; set; }
        public string SubscriptionId { get; set; }
    }
}
