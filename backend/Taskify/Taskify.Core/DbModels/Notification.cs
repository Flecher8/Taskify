using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.Enums;

namespace Taskify.Core.DbModels
{
    public class Notification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public User User { get; set; }
        public NotificationType NotificationType { get; set; }
        public DateTime CreatedAt { get; set; } 
        public bool IsRead { get; set; }
    }
}
